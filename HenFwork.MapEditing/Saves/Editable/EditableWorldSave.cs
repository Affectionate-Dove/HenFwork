// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace HenFwork.MapEditing.Saves.Editable
{
    /// <summary>
    ///     A mutable version of <see cref="WorldSave"/>
    ///     with events firing upon adding/removing <see cref="EditableChunkSave"/>s.
    /// </summary>
    public class EditableWorldSave : ICollection<EditableChunkSave>
    {
        private readonly List<EditableChunkSave> chunkSaves = new();

        public event Action<EditableChunkSave> ChunkAdded;

        public event Action<EditableChunkSave> ChunkRemoved;

        public string Name { get; set; }
        public int Count => ((ICollection<EditableChunkSave>)chunkSaves).Count;

        public bool IsReadOnly => ((ICollection<EditableChunkSave>)chunkSaves).IsReadOnly;

        public EditableWorldSave(string name, IEnumerable<EditableChunkSave> chunkSaves)
        {
            Name = name;
            this.chunkSaves.AddRange(chunkSaves);
        }

        public EditableWorldSave(WorldSave worldSave) : this(worldSave.Name, worldSave.ChunkSaves.Select(cs => new EditableChunkSave(cs)))
        {
        }

        public void Add(EditableChunkSave chunkSave)
        {
            ((ICollection<EditableChunkSave>)chunkSaves).Add(chunkSave);
            ChunkAdded?.Invoke(chunkSave);
        }

        public void Clear()
        {
            var _chunkSaves = chunkSaves.ToImmutableArray();

            ((ICollection<EditableChunkSave>)chunkSaves).Clear();

            foreach (var chunkSave in _chunkSaves)
                ChunkRemoved?.Invoke(chunkSave);
        }

        public bool Contains(EditableChunkSave chunkSave) => ((ICollection<EditableChunkSave>)chunkSaves).Contains(chunkSave);

        public void CopyTo(EditableChunkSave[] array, int arrayIndex) => ((ICollection<EditableChunkSave>)chunkSaves).CopyTo(array, arrayIndex);

        public IEnumerator<EditableChunkSave> GetEnumerator() => ((IEnumerable<EditableChunkSave>)chunkSaves).GetEnumerator();

        public bool Remove(EditableChunkSave chunkSave)
        {
            if (((ICollection<EditableChunkSave>)chunkSaves).Remove(chunkSave))
            {
                ChunkRemoved?.Invoke(chunkSave);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)chunkSaves).GetEnumerator();

        public WorldSave ToWorldSave()
        {
            var chunkSaves = this.chunkSaves.Select(ecs => ecs.ToChunkSave());
            return new WorldSave(Name, chunkSaves);
        }
    }
}