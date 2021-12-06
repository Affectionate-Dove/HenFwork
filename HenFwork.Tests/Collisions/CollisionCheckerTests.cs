// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Collisions;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Tests.Collisions
{
    public class CollisionCheckerTests
    {
        private Medium[] mediums;

        public static IEnumerable<(Node node, bool expected)> NodeInMediumsCases()
        {
            yield return (new TestCollisionNode(1, new Vector3(0, 0, 2), new Sphere[] { new() { Radius = 2 } }), false);
            yield return (new TestCollisionNode(2, new Vector3(-1.5f, 0, 2.5f), new Sphere[]
            { new() { Radius = 0.5f } }), true);
            yield return (new TestCollisionNode(3, new Vector3(1.5f, 0, 4.5f), new Sphere[] { new() { Radius = 2 } }), false);
            yield return (new TestCollisionNode(4, new Vector3(5.5f, 0, -2.5f), new Sphere[]
                { new() { Radius = 0.5f } }), false);
            yield return (new TestCollisionNode(5, new Vector3(0.5f, 0, 5.5f), new Sphere[]
                { new() { Radius = 0.5f }, new() { Radius = 0.2f, CenterPosition = new Vector3(0.4f) } }), false);
        }

        [SetUp]
        public void SetUpMediums()
        {
            mediums = new Medium[8];
            mediums[0] = new Medium
            {
                Triangle = new Triangle3(new(1, 0, 0), new(4, 0, -2), new(1, 0, -2))
            };
            mediums[1] = new Medium
            {
                Triangle = new Triangle3(mediums[0].Triangle.A, new(3, 0, 2), mediums[0].Triangle.B)
            };
            mediums[2] = new Medium
            {
                Triangle = new Triangle3(mediums[0].Triangle.C, new(-1, 0, -1), new(1, 0, 2))
            };
            mediums[3] = new Medium
            {
                Triangle = new Triangle3(mediums[2].Triangle.B, new(-2, 0, 1), mediums[2].Triangle.C)
            };
            mediums[4] = new Medium
            {
                Triangle = new Triangle3(mediums[0].Triangle.A, mediums[2].Triangle.C, mediums[1].Triangle.B)
            };
            mediums[5] = new Medium
            {
                Triangle = new Triangle3(mediums[3].Triangle.B, new(-2, 0, 4), mediums[2].Triangle.C)
            };
            mediums[6] = new Medium
            {
                Triangle = new Triangle3(mediums[5].Triangle.C, mediums[5].Triangle.B, new(1, 0, 8))
            };
            mediums[7] = new Medium
            {
                Triangle = new Triangle3(new(1.0001f, 0, 2), mediums[6].Triangle.B, mediums[1].Triangle.B)
            };
        }

        [Test]
        public void CheckNodeCollisionsTest()
        {
            var tc = new NodeCollisionTestManager();
            {
                var a = new TestCollisionNode(1, new Vector3(), new[] { new Sphere { Radius = 1 } });
                var b = new TestCollisionNode(2, new Vector3(1.5f, 0, 1.5f), new[] { new Sphere { Radius = 1.5f } });
                var c = new TestCollisionNode(3, new Vector3(-3, 0, 3), new[] { new Sphere { Radius = 3 } });
                var d = new TestCollisionNode(4, new Vector3(-3, -6, 3), new[] { new Sphere { Radius = 3 } });
                var e = new TestCollisionNode(5, new Vector3(0, 0, 2), new[] { new Sphere { Radius = 2 } });
                tc.AddExpectedCollision(a, b);
                tc.AddExpectedCollision(c, d);
                tc.AddExpectedCollision(e, a);
                tc.AddExpectedCollision(b, e);
                tc.AddExpectedCollision(e, c);
            }

            var collisionHandler = new TestCollisionHandler();
            CollisionChecker.CheckNodeCollisions(tc.Nodes, collisionHandler.OnCollision);
            foreach (var (a, b) in tc.ExpectedCollisions)
            {
                Assert.IsTrue(collisionHandler.DetectedCollisions.Contains((a, b)) || collisionHandler.DetectedCollisions.Contains((b, a)), $"Failed to detect an expected collision between two nodes.\nNode a: {a}\nNode b: {b}");
            }
            foreach (var (a, b) in collisionHandler.DetectedCollisions)
            {
                Assert.IsTrue(tc.ExpectedCollisions.Contains((a, b)) || tc.ExpectedCollisions.Contains((b, a)), $"Detected an unexpected collision between two nodes.\nNode a: {a}\nNode b: {b}");
            }
        }

        [TestCaseSource(nameof(NodeInMediumsCases))]
        public void IsNodeContainedInMediumsTest((Node node, bool expected) tc) => Assert.AreEqual(tc.expected, CollisionChecker.IsNodeContainedInMediums(tc.node, mediums), $"Node id: {tc.node.Id}");

        private class NodeCollisionTestManager
        {
            private readonly HashSet<(Node a, Node b)> expectedCollisions = new();
            private readonly List<Node> nodes = new();

            public IReadOnlySet<(Node a, Node b)> ExpectedCollisions => expectedCollisions;
            public IReadOnlyList<Node> Nodes => nodes;

            public void AddExpectedCollision(Node a, Node b)
            {
                if (!nodes.Contains(a))
                    nodes.Add(a);
                if (!nodes.Contains(b))
                    nodes.Add(b);
                expectedCollisions.Add((a, b));
            }
        }

        private class TestCollisionHandler
        {
            private readonly HashSet<(Node, Node)> detectedCollisions = new();
            public IReadOnlySet<(Node a, Node b)> DetectedCollisions => detectedCollisions;

            public void OnCollision(Node a, Node b)
            {
                Assert.IsFalse(detectedCollisions.Contains((a, b)));
                detectedCollisions.Add((a, b));
            }
        }
    }
}