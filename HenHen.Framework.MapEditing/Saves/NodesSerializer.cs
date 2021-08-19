﻿// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenHen.Framework.MapEditing.Saves.PropertySerializers;
using HenHen.Framework.Worlds.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HenHen.Framework.MapEditing.Saves
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
        };

        public Node Deserialize(string assemblyName, string fullTypeName, in IReadOnlyDictionary<string, string> kv)
        {
            var node = Activator.CreateInstance(assemblyName, fullTypeName).Unwrap() as Node;
            var nodeType = node.GetType();

            foreach (var (key, value) in kv)
                SetMemberValue(node, nodeType, key, value);

            return node;
        }

        public Node Deserialize(NodeSave nodeSave) => Deserialize(nodeSave.AssemblyName, nodeSave.FullTypeName, nodeSave.MembersValues);

        public NodeSave Serialize(Node node)
        {
            var nodeType = node.GetType();
            var assemblyName = nodeType.Assembly.GetName().Name;
            var typeName = nodeType.FullName.Replace(assemblyName, null).TrimStart('.');

            var keyValues = new Dictionary<string, string>();

            var fieldsInfos = nodeType.GetFields(binding_attr);
            foreach (var fieldInfo in fieldsInfos.Where(pi => pi.GetCustomAttribute(typeof(SaveableAttribute)) != null))
            {
                var memberTypeSerializer = GetMemberSerializer(fieldInfo.FieldType);
                var fieldValue = fieldInfo.GetValue(node);
                var serializedField = memberTypeSerializer.Serialize(fieldValue);

                keyValues.Add(fieldInfo.Name, serializedField);
            }

            var propertiesInfos = nodeType.GetProperties(binding_attr);
            foreach (var propertyInfo in propertiesInfos.Where(pi => pi.GetCustomAttribute(typeof(SaveableAttribute)) != null))
            {
                var memberTypeSerializer = GetMemberSerializer(propertyInfo.PropertyType);
                var propertyValue = propertyInfo.GetValue(node);
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
            var fieldType = field.FieldType;
            var memberSerializer = GetMemberSerializer(fieldType);
            var deserializedValue = memberSerializer.Deserialize(memberValue);
            field.SetValue(node, deserializedValue);
        }

        private void SetPropertyValue(Node node, Type nodeType, string memberName, string memberValue, BindingFlags bindingAttr)
        {
            var property = nodeType.GetProperty(memberName, bindingAttr);
            var propertyType = property.PropertyType;
            var memberSerializer = GetMemberSerializer(propertyType);
            var deserializedValue = memberSerializer.Deserialize(memberValue);
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