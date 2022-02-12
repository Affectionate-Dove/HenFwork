// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Worlds.Functional;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.MapEditing.Saves
{
    /// <summary>
    ///     A state of a <see cref="NodeWorld"/>.
    /// </summary>
    public record WorldSave
    {
        private const char chunk_separator = '\n';

        public IReadOnlyList<ChunkSave> ChunkSaves { get; }

        public WorldSave(IReadOnlyList<ChunkSave> chunkSaves) => ChunkSaves = chunkSaves;

        public WorldSave(string data)
        {
            var chunkSaves = new List<ChunkSave>();
            ChunkSaves = chunkSaves;
            foreach (var line in data.Split(chunk_separator, System.StringSplitOptions.RemoveEmptyEntries))
                chunkSaves.Add(new ChunkSave(line));
        }

        public string ToDataString() => string.Join(chunk_separator, ChunkSaves.Select(cs => cs.ToDataString()));

        public NodeWorld ToWorld(NodesSerializer nodesSerializer = null)
        {
            var chunks = ChunkSaves.Select(cs => cs.ToChunk(nodesSerializer));
            return new NodeWorld(chunks);
        }
    }
}