// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves.PropertySerializers;
using HenFwork.Worlds;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

namespace HenFwork.MapEditing.Saves
{
    /// <summary>
    ///     Handles serializing <see cref="Node"/>s into <see cref="NodeSave"/>s
    ///     and vice versa.
    /// </summary>
    public class NodesSerializer
    {
        /// <summary>
        ///     Binding flags used for member reflection.
        /// </summary>
        private const BindingFlags binding_attr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        ///     Pairs of types and <see cref="SaveableMemberSerializer"/>s
        ///     used for their serialization.
        /// </summary>
        public Dictionary<Type, SaveableMemberSerializer> MembersSerializers { get; } = new()
        {
            [typeof(string)] = new StringSerializer(),
            [typeof(double)] = new DoubleSerializer(),
            [typeof(float)] = new FloatSerializer(),
            [typeof(int)] = new IntSerializer(),
            [typeof(bool)] = new BooleanSerializer(),
            [typeof(Vector2)] = new Vector2Serializer(),
            [typeof(Vector3)] = new Vector3Serializer(),
        };

        public Node Deserialize(string assemblyName, string fullTypeName, in IReadOnlyDictionary<string, string> kv)
        {
            var node = Activator.CreateInstance(assemblyName, fullTypeName)?.Unwrap() as Node;
            var nodeType = node!.GetType();

            foreach (var (key, value) in kv)
                SetMemberValue(node, nodeType, key, value);

            return node;
        }

        public Node Deserialize(NodeSave nodeSave) => Deserialize(nodeSave.AssemblyName, nodeSave.FullTypeName, nodeSave.MembersValues);

        public NodeSave Serialize(Node node)
        {
            var nodeType = node.GetType();
            var assemblyName = nodeType.Assembly.GetName().Name;
            var typeName = nodeType.FullName;
            if (typeName is null)
                throw new Exception($"Couldn't get the type name of the node.");
            if (assemblyName is null)
                throw new NullReferenceException($"{nameof(assemblyName)} was null.");
            typeName = typeName.Replace(assemblyName, null).TrimStart('.');

            var keyValues = new Dictionary<string, string>();

            var fieldsInfos = nodeType.GetFields(binding_attr);
            foreach (var fieldInfo in fieldsInfos.Where(pi => pi.GetCustomAttribute(typeof(SaveableAttribute)) != null))
            {
                var fieldValue = fieldInfo.GetValue(node);
                if (fieldValue is null)
                    continue;

                var memberTypeSerializer = GetMemberSerializer(fieldInfo.FieldType);
                var serializedField = memberTypeSerializer.Serialize(fieldValue);

                keyValues.Add(fieldInfo.Name, serializedField);
            }

            var propertiesInfos = nodeType.GetProperties(binding_attr);
            foreach (var propertyInfo in propertiesInfos.Where(pi => pi.GetCustomAttribute(typeof(SaveableAttribute)) != null))
            {
                var propertyValue = propertyInfo.GetValue(node);
                if (propertyValue is null)
                    continue;

                var memberTypeSerializer = GetMemberSerializer(propertyInfo.PropertyType);
                var serializedProperty = memberTypeSerializer.Serialize(propertyValue);

                keyValues.Add(propertyInfo.Name, serializedProperty);
            }

            return new(assemblyName, typeName, keyValues);
        }

        private void SetMemberValue(Node node, Type nodeType, string memberName, string memberValue)
        {
            var member = nodeType.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, binding_attr).SingleOrDefault();

            if (member is null)
                throw new MemberDeserializationException(memberName, $"No such member found in type \"{nodeType.FullName}\".");

            if (member.MemberType == MemberTypes.Property)
                SetPropertyValue(node, nodeType, memberName, memberValue, binding_attr);
            else
                SetFieldValue(node, nodeType, memberName, memberValue, binding_attr);
        }

        private void SetFieldValue(Node node, Type nodeType, string memberName, string memberValue, BindingFlags bindingAttr)
        {
            var field = nodeType.GetField(memberName, bindingAttr);
            if (field is null)
                throw new Exception($"Couldn't find field \"{memberName}\" in type \"{nodeType}\".");
            var fieldType = field.FieldType;
            var memberSerializer = GetMemberSerializer(fieldType);
            var deserializedValue = memberSerializer.Deserialize(memberValue);
            field.SetValue(node, deserializedValue);
        }

        private void SetPropertyValue(Node node, Type nodeType, string memberName, string memberValue, BindingFlags bindingAttr)
        {
            var property = nodeType.GetProperty(memberName, bindingAttr);
            property = property?.DeclaringType!.GetProperty(memberName);
            if (property is null)
                throw new Exception($"Couldn't find field \"{memberName}\" in type \"{nodeType}\".");
            var propertyType = property.PropertyType;
            var memberSerializer = GetMemberSerializer(propertyType);
            var deserializedValue = memberSerializer.Deserialize(memberValue);

            if (property.SetMethod is null)
                throw new NullReferenceException($"The property {nodeType.Name}.{memberName} doesn't have a setter.");

            property.SetValue(node, deserializedValue);
        }

        private SaveableMemberSerializer GetMemberSerializer(Type fieldType)
        {
            if (!MembersSerializers.TryGetValue(fieldType, out var memberSerializer))
                throw new NoSerializerForTypeException(fieldType);

            return memberSerializer;
        }
    }
}