// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Extensions;
using HenBstractions.Numerics;
using HenFwork.Collisions;
using HenFwork.Worlds.Functional.Mediums;
using HenFwork.Worlds.Functional.Nodes;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace HenFwork.Worlds.Functional.Chunks
{
    /// <summary>
    /// Manages and exposes chunks and utilities related to them.
    /// </summary>
    public class ChunksManager
    {
        public IReadOnlyDictionary<Vector2, Chunk> Chunks { get; }
        public Vector2 ChunkCount { get; }
        public RectangleF ChunksIndexesBoundingRect { get; }
        public float ChunkSize { get; }

        public ChunksManager(Vector2 chunkCount, float chunkSize)
        {
            Chunks = CreateChunks(chunkCount, chunkSize);
            ChunkCount = chunkCount;
            ChunkSize = chunkSize;
            ChunksIndexesBoundingRect = new(0, ChunkCount.X - 1, 0, ChunkCount.Y - 1);
        }

        public ChunksManager(IEnumerable<Chunk> chunks)
        {
            var _chunks = new Dictionary<Vector2, Chunk>();
            float? chunkSize = null;
            var chunkCount = Vector2.Zero;
            foreach (var chunk in chunks)
            {
                _chunks.Add(chunk.Index, chunk);
                chunkCount = Vector2.Max(chunk.Index + Vector2.One, chunkCount);

                if (chunkSize is null)
                    chunkSize = chunk.Coordinates.Width;
                else if (chunkSize.Value != chunk.Coordinates.Width)
                    throw new Exception($"The {nameof(ChunkSize)} is not uniform for all chunks.");
            }

            Chunks = _chunks;
            ChunkCount = chunkCount;
            ChunkSize = chunkSize.Value;
            ChunksIndexesBoundingRect = new(0, ChunkCount.X - 1, 0, ChunkCount.Y - 1);
        }

        /// <remarks>
        /// Will return an index even if
        /// there is no chunk at that index.
        /// </remarks>
        public Vector2 GetChunkIndexForPosition(Vector2 position) => new((int)(position.X / ChunkSize), (int)(position.Y / ChunkSize));

        public Chunk GetChunkForPosition(Vector2 position) => Chunks[GetChunkIndexForPosition(position)];

        public void AddMedium(Medium medium)
        {
            foreach (var chunk in GetChunksForMedium(medium))
                chunk.AddMedium(medium);
        }

        public void AddNode(Node node)
        {
            foreach (var chunk in GetChunksForNode(node))
                chunk.AddNode(node);
        }

        public IEnumerable<Chunk> GetChunksForRectangle(RectangleF rectangle)
        {
            rectangle /= new Vector2(ChunkSize);

            var intersection = rectangle.GetIntersection(ChunksIndexesBoundingRect);
            if (!intersection.HasValue)
                yield break;
            rectangle = intersection.Value;

            for (var x = (int)rectangle.Left; x <= rectangle.Right; x++)
                for (var y = (int)rectangle.Bottom; y <= rectangle.Top; y++)
                    yield return Chunks[new Vector2(x, y)];
        }

        public IEnumerable<Chunk> GetChunksForCircle(Circle circle)
        {
            var rect = new RectangleF(circle.CenterPosition.X - circle.Radius, circle.CenterPosition.X + circle.Radius, circle.CenterPosition.Y - circle.Radius, circle.CenterPosition.Y + circle.Radius);
            foreach (var chunk in GetChunksForRectangle(rect))
                if (ElementaryCollisions.IsPointInCircle(chunk.Coordinates.Center, circle))
                    yield return chunk;
        }

        public IEnumerable<Chunk> GetChunksForNode(Node node)
        {
            if (node.CollisionBody is null)
            {
                yield return GetChunkForPosition(node.Position.ToTopDownPoint());
                yield break;
            }
            var rect = (node.CollisionBody.BoundingBox + node.Position).ToTopDownRectangle();
            foreach (var chunk in GetChunksForRectangle(rect))
                yield return chunk;
        }

        public IEnumerable<Chunk> GetChunksForMedium(Medium medium)
        {
            var vertexEnumerator = EnumerateTopDownVerticesOfMedium(medium).GetEnumerator();
            vertexEnumerator.MoveNext();
            var vertex = vertexEnumerator.Current;
            var mostLeftPos = vertex.X;
            var mostRightPos = vertex.X;
            var mostUpPos = vertex.Y;
            var mostDownPos = vertex.Y;
            while (vertexEnumerator.MoveNext())
            {
                mostLeftPos = Math.Min(mostLeftPos, vertexEnumerator.Current.X);
                mostDownPos = Math.Min(mostDownPos, vertexEnumerator.Current.Y);
                mostRightPos = Math.Max(mostRightPos, vertexEnumerator.Current.X);
                mostUpPos = Math.Max(mostUpPos, vertexEnumerator.Current.Y);
            }
            var rect = new RectangleF(mostLeftPos, mostRightPos, mostDownPos, mostUpPos);
            foreach (var chunk in GetChunksForRectangle(rect))
                yield return chunk;
        }

        private static Dictionary<Vector2, Chunk> CreateChunks(Vector2 chunkCount, float chunkSize)
        {
            var chunks = new Dictionary<Vector2, Chunk>((int)(chunkCount.X * chunkCount.Y));
            for (var x = 0; x < chunkCount.X; x++)
                for (var y = 0; y < chunkCount.Y; y++)
                {
                    var index = new Vector2(x, y);
                    chunks.Add(index, new Chunk(index, chunkSize));
                }
            return chunks;
        }

        private static IEnumerable<Vector2> EnumerateTopDownVerticesOfMedium(Medium medium)
        {
            yield return medium.Triangle.A.ToTopDownPoint();
            yield return medium.Triangle.B.ToTopDownPoint();
            yield return medium.Triangle.C.ToTopDownPoint();
        }
    }
}