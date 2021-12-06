// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using System;

namespace HenFwork.Tests.Worlds.Functional.Nodes
{
    public class NodeTests
    {
        private TestNode node;

        [SetUp]
        public void SetUp() => node = new TestNode();

        [Test]
        public void InteractionTest()
        {
            Assert.IsNull(node.Interaction);
            node.SetInteraction();
            Assert.IsNotNull(node.Interaction);
            node.Interaction();
            Assert.IsTrue(node.InteractedWith);
        }

        private class TestNode : Node
        {
            private Action interaction;

            public override Action Interaction => interaction;

            public bool InteractedWith { get; private set; }

            public void SetInteraction() => interaction = () => InteractedWith = true;
        }
    }
}