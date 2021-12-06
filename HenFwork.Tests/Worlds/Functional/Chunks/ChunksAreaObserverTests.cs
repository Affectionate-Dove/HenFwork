// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using System.Linq;
using System.Numerics;

namespace HenFwork.Tests.Worlds.Functional.Chunks
{
    public class ChunksAreaObserverTests
    {
        private ChunksManager chunksManager;
        private ChunksAreaObserver observer;

        [SetUp]
        public void SetUp()
        {
            chunksManager = new ChunksManager(new(5, 5), 10);
            observer = new ChunksAreaObserver(chunksManager)
            {
                ObservedArea = new(15, 25, 15, 25)
            };
            observer.NodeEnteredChunksArea += OnNodeEntered;
            observer.NodeLeftChunksArea += OnNodeLeft;
        }

        public void NodesInAreaTest()
        {
            Assert.IsEmpty(observer.NodesInArea);
            chunksManager.AddNode(new TestNode { Position = Vector3.Zero });
            Assert.IsEmpty(observer.NodesInArea);
            var node = new TestNode { Position = new(20) };

            chunksManager.AddNode(node);
            Assert.AreEqual(1, node.EnteredEvents);
            Assert.Zero(node.LeftEvents);
            Assert.IsTrue(observer.NodesInArea.Contains(node));

            chunksManager.GetChunksForNode(node).Single().RemoveNode(node);
            Assert.AreEqual(1, node.EnteredEvents);
            Assert.AreEqual(1, node.LeftEvents);
            Assert.IsFalse(observer.NodesInArea.Contains(node));
        }

        public void ObservedAreaChangeTest()
        {
            var node = new TestNode { Position = new(40) };

            chunksManager.AddNode(node);
            Assert.Zero(node.EnteredEvents);
            Assert.Zero(node.LeftEvents);

            observer.ObservedArea = new(30, 50, 30, 50);
            Assert.AreEqual(1, node.EnteredEvents);
            Assert.Zero(node.LeftEvents);

            observer.ObservedArea = new(0, 1, 0, 1);
            Assert.AreEqual(1, node.EnteredEvents);
            Assert.AreEqual(1, node.LeftEvents);
        }

        private void OnNodeEntered(Node node) => (node as TestNode).EnteredEvents++;

        private void OnNodeLeft(Node node) => (node as TestNode).LeftEvents++;

        private class TestNode : Node
        {
            public int EnteredEvents;
            public int LeftEvents;
        }
    }
}