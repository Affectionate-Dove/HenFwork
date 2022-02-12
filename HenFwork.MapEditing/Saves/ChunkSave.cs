// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves.PropertySerializers;
using HenFwork.Worlds.Functional.Chunks;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HenFwork.MapEditing.Saves
{
    public record ChunkSave
    {
        private const char separator = '#';
        private const char sub_separator = '/';
        private static readonly Vector2Serializer vector2Serializer = new();

        public IReadOnlyList<NodeSave> NodeSaves { get; }
        public IReadOnlyList<MediumSave> MediumSaves { get; }
        public (int x, int y) Index { get; }
        public float Size { get; }

        public ChunkSave(IReadOnlyList<NodeSave> nodeSaves, IReadOnlyList<MediumSave> mediumSaves, (int x, int y) index, float size)
        {
            NodeSaves = nodeSaves;
            MediumSaves = mediumSaves;
            Index = index;
            Size = size;
        }

        public ChunkSave(string data)
        {
            var splitData = data.Split(separator);

            var indexVector2 = (Vector2)vector2Serializer.Deserialize(splitData[0]);
            Index = ((int)indexVector2.X, (int)indexVector2.Y);

            Size = float.Parse(splitData[1]);

            var mediumSaves = new List<MediumSave>();
            var serializedMediumSaves = splitData[2].Split(sub_separator, System.StringSplitOptions.RemoveEmptyEntries);
            mediumSaves.AddRange(serializedMediumSaves.Select(s => new MediumSave(s)));
            MediumSaves = mediumSaves;

            var nodeSaves = new List<NodeSave>();
            var serializedNodeSaves = splitData[3].Split(sub_separator, System.StringSplitOptions.RemoveEmptyEntries);
            nodeSaves.AddRange(serializedNodeSaves.Select(s => new NodeSave(s)));
            NodeSaves = nodeSaves;
        }

        public string ToDataString() => $"{vector2Serializer.Serialize(new Vector2(Index.x, Index.y))}#{Size}#{string.Join('/', MediumSaves.Select(ms => ms.ToStringData()))}#{string.Join('/', NodeSaves.Select(ns => ns.ToStringData()))}";

        public Chunk ToChunk(NodesSerializer nodesSerializer = null)
        {
            var chunk = new Chunk(new(Index.x, Index.y), Size);

            nodesSerializer ??= new NodesSerializer();
            foreach (var nodeSave in NodeSaves)
                chunk.AddNode(nodesSerializer.Deserialize(nodeSave));

            foreach (var mediumSave in MediumSaves)
                chunk.AddMedium(mediumSave.ToMedium());

            return chunk;
        }
    }
}