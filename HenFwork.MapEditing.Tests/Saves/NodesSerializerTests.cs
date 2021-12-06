// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Saves.PropertySerializers;
using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using System.Collections.Generic;

namespace HenFwork.MapEditing.Tests.Saves
{
    [TestFixture(TestOf = typeof(NodesSerializer))]
    public class NodesSerializerTests
    {
        private const string data_string = "HenFwork.MapEditing.Tests|Saves.TestNodeForSaving\ttestStringField:hy\tTestStringProperty:da";

        private NodesSerializer nodesSerializer;

        [SetUp]
        public void SetUp() => nodesSerializer = new NodesSerializer();

        [Test]
        public void DeserializeTest()
        {
            var nodeSave = new NodeSave(data_string);
            var node = nodesSerializer.Deserialize(nodeSave.AssemblyName, nodeSave.FullTypeName, nodeSave.MembersValues) as TestNodeForSaving;

            Assert.NotNull(node);
            Assert.AreEqual(typeof(TestNodeForSaving), node.GetType());

            Assert.AreEqual("da", node.TestStringProperty);
            Assert.AreEqual("hy", node.TestStringField);
        }

        [Test(Description = "When deserializing and a member mentioned in the data string doesn't exist in a class, an appropriate exception should be thrown.")]
        public void DeserializeNonExistingMemberTest()
        {
            var nodeSave = new NodeSave(data_string + "\tNonExistingMember:SomeValue");
            Assert.Throws<MemberDeserializationException>(() => nodesSerializer.Deserialize(nodeSave.AssemblyName, nodeSave.FullTypeName, nodeSave.MembersValues));
        }

        [Test(Description = "When there is no " + nameof(SaveableMemberSerializer) + " for a given member's type, an appropriate exception should be thrown.")]
        public void NoMemberSerializerAvailableSerializationTest()
        {
            var node = new NodeWithMemberWithNoSerializer { NonserializableMember = "a" };
            Assert.Throws<NoSerializerForTypeException>(() => nodesSerializer.Serialize(node));
        }

        [Test(Description = "When there is no " + nameof(SaveableMemberSerializer) + " for a given member's type, an appropriate exception should be thrown.")]
        public void NoMemberSerializerAvailableDeserializationTest()
        {
            var nodeType = typeof(NodeWithMemberWithNoSerializer);
            var nodeSave = new NodeSave(nodeType.Assembly.GetName().Name, nodeType.FullName.Replace(nodeType.Assembly.GetName().Name + '.', null), new KeyValuePair<string, string>[] { new("NonserializableMember", "Some value") });
            Assert.Throws<NoSerializerForTypeException>(() => nodesSerializer.Deserialize(nodeSave));
        }

        [Test]
        public void SerializeTest()
        {
            var node = new TestNodeForSaving { TestStringField = "hy", TestStringProperty = "da" };
            var nodeSave = nodesSerializer.Serialize(node);

            Assert.AreEqual(data_string, nodeSave.ToStringData());
        }

        private class NodeWithMemberWithNoSerializer : Node
        {
            [Saveable]
            public object NonserializableMember { get; set; }
        }
    }
}