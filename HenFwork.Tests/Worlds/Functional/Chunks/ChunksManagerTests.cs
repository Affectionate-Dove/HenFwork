// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Numerics;
using HenFwork.Tests.Collisions;
using HenFwork.Worlds.Functional.Chunks;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HenFwork.Tests.Worlds.Functional.Chunks
{
    public class ChunksManagerTests
    {
        private static IReadOnlyCollection<AddMediumTestCase> AddMediumTestCases => new List<AddMediumTestCase>
        {
            new AddMediumTestCase
            (
                new Medium
                {
                    Triangle = new Triangle3(new(0, 0, 0), new(2, 0, 0.5f), new(1, 0, 0.8f))
                },
                new HashSet<Vector2>
                {
                    new Vector2(0),
                    new Vector2(1, 0),
                    new Vector2(2, 0)
                }
            ),
            new AddMediumTestCase
            (
                new Medium
                {
                    Triangle = new Triangle3(new(0.5f, 0, 2.5f), new(2.9f, 0, 1.9f), new(1.2f, 0, 1.2f))
                },
                new HashSet<Vector2>
                {
                    new Vector2(0, 2),
                    new Vector2(1, 2),
                    new Vector2(2, 2),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(2, 1)
                }
            )
        };

        private static IReadOnlyCollection<AddNodeTestCase> AddNodeTestCases => new List<AddNodeTestCase>
        {
            new AddNodeTestCase
            (
                new TestCollisionNode(1, new Vector3(1, 0, 2), new[] { new Sphere { Radius = 0.5f } }),
                new HashSet<Vector2>
                {
                    new Vector2(0, 2),
                    new Vector2(1, 2),
                    new Vector2(0, 1),
                    new Vector2(1, 1)
                }
            ),
            new AddNodeTestCase
            (
                new TestCollisionNode(2, new Vector3(3, 1, 1), new[]
                {
                    new Sphere { Radius = 0.7f },
                    new Sphere { CenterPosition = new Vector3(-0.8f, 0, 0), Radius = 0.3f }
                }),
                new HashSet<Vector2>
                {
                    new Vector2(1, 1),
                    new Vector2(2, 1),
                    new Vector2(3, 1),
                    new Vector2(1, 0),
                    new Vector2(2, 0),
                    new Vector2(3, 0),
                })
        };

        [Test]
        public void CtorTest()
        {
            const int chunk_size = 15;
            var chunkCount = new Vector2(6, 8);
            var chunksManager = new ChunksManager(chunkCount, chunk_size);
            Assert.AreEqual(chunk_size, chunksManager.ChunkSize);
            Assert.AreEqual(chunkCount.X * chunkCount.Y, chunksManager.Chunks.Count);
        }

        [TestCase(1, 0, 0, 0, 0)]
        [TestCase(1, 1, 1, 1, 1)]
        [TestCase(2, 1, 1, 0, 0)]
        [TestCase(5, 6.5f, 21, 1, 4)]
        [TestCase(3, 2.99f, 2.99f, 0, 0)]
        public void GetChunkIndexForPositionTest(float chunkSize, float posX, float posY, float expectedX, float expectedY)
        {
            var expected = new Vector2(expectedX, expectedY);
            var position = new Vector2(posX, posY);
            var chunksManager = new ChunksManager(new Vector2(10), chunkSize);
            Assert.AreEqual(expected, chunksManager.GetChunkIndexForPosition(position));
        }

        [Test]
        public void GetChunksForRectangleDoesntThrowTest()
        {
            var cm = CreateChunksManager();
            var rect = new RectangleF(-1, 100, -1, 100);
            List<Chunk> chunks = new();
            Assert.DoesNotThrow(() => chunks.AddRange(cm.GetChunksForRectangle(rect)));
            Assert.AreEqual(100, chunks.Count);
        }

        [TestCaseSource(nameof(AddMediumTestCases))]
        public void AddMediumTest(AddMediumTestCase tc)
        {
            var chunksManager = CreateChunksManager();
            chunksManager.AddMedium(tc.Medium);
            foreach ((var index, var chunk) in chunksManager.Chunks)
            {
                var chunkShouldHaveMedium = tc.ExpectedChunkIndexes.Contains(index);
                var chunkHasMedium = chunk.Mediums.Contains(tc.Medium);
                Assert.AreEqual(chunkShouldHaveMedium, chunkHasMedium, $"Chunk index: {index}");
            }
        }

        [TestCaseSource(nameof(AddNodeTestCases))]
        public void AddNodeTest(AddNodeTestCase tc)
        {
            var chunksManager = CreateChunksManager();
            chunksManager.AddNode(tc.Node);
            foreach ((var index, var chunk) in chunksManager.Chunks)
            {
                var chunkShouldHaveNode = tc.ExpectedChunkIndexes.Contains(index);
                var chunkHasNode = chunk.Nodes.Contains(tc.Node);
                Assert.AreEqual(chunkShouldHaveNode, chunkHasNode, $"Chunk index: {index}");
            }
        }

        private static ChunksManager CreateChunksManager() => new(new Vector2(10), 1);

        public record AddMediumTestCase(Medium Medium, IReadOnlySet<Vector2> ExpectedChunkIndexes);

        public record AddNodeTestCase(Node Node, IReadOnlySet<Vector2> ExpectedChunkIndexes);
    }
}