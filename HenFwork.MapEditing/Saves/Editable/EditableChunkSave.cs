// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace HenFwork.MapEditing.Saves.Editable
{
    /// <summary>
    ///     A mutable version of <see cref="ChunkSave"/>
    ///     with events firing upon adding/removing <see cref="NodeSave"/>s
    ///     or <see cref="MediumSave"/>s.
    /// </summary>
    public class EditableChunkSave : ICollection<NodeSave>, ICollection<MediumSave>
    {
        private readonly List<NodeSave> nodeSaves = new();
        private readonly List<MediumSave> mediumSaves = new();

        public event Action<NodeSave> NodeAdded = delegate { };

        public event Action<NodeSave> NodeRemoved = delegate { };

        public event Action<MediumSave> MediumAdded = delegate { };

        public event Action<MediumSave> MediumRemoved = delegate { };

        public bool IsReadOnly => false;

        public int Count => ((ICollection<MediumSave>)mediumSaves).Count + ((ICollection<NodeSave>)nodeSaves).Count;

        public float Size { get; }
        public (int x, int y) Index { get; }
        public IEnumerable<NodeSave> Nodes => nodeSaves;

        public IEnumerable<MediumSave> Mediums => mediumSaves;

        public EditableChunkSave(IEnumerable<NodeSave> nodeSaves, IEnumerable<MediumSave> mediumSaves, float size, (int x, int y) index)
        {
            this.nodeSaves.AddRange(nodeSaves);
            this.mediumSaves.AddRange(mediumSaves);
            Size = size;
            Index = index;
        }

        public EditableChunkSave(ChunkSave chunkSave) : this(chunkSave.NodeSaves, chunkSave.MediumSaves, chunkSave.Size, chunkSave.Index)
        {
        }

        /// <summary>
        ///     Creates an empty <see cref="EditableChunkSave"/>.
        /// </summary>
        public EditableChunkSave(float size, (int x, int y) index)
        {
            Size = size;
            Index = index;
        }

        public void Add(NodeSave nodeSave)
        {
            nodeSaves.Add(nodeSave);
            NodeAdded?.Invoke(nodeSave);
        }

        public void Clear()
        {
            var _nodeSaves = nodeSaves.ToImmutableArray();
            nodeSaves.Clear();
            foreach (var nodeSave in _nodeSaves)
                NodeRemoved?.Invoke(nodeSave);

            var _mediumSaves = mediumSaves.ToImmutableArray();
            mediumSaves.Clear();
            foreach (var mediumSave in _mediumSaves)
                MediumRemoved?.Invoke(mediumSave);
        }

        public bool Contains(NodeSave nodeSave) => nodeSaves.Contains(nodeSave);

        public void CopyTo(NodeSave[] array, int arrayIndex) => nodeSaves.CopyTo(array, arrayIndex);

        public bool Remove(NodeSave nodeSave)
        {
            if (nodeSaves.Remove(nodeSave))
            {
                NodeRemoved?.Invoke(nodeSave);
                return true;
            }

            return false;
        }

        /// <remarks>
        ///     The <paramref name="mediumSave"/> won't be added if it already exists.
        /// </remarks>
        public void Add(MediumSave mediumSave)
        {
            if (mediumSaves.Contains(mediumSave))
                return;
            ((ICollection<MediumSave>)mediumSaves).Add(mediumSave);
            MediumAdded?.Invoke(mediumSave);
        }

        public bool Contains(MediumSave mediumSave) => ((ICollection<MediumSave>)mediumSaves).Contains(mediumSave);

        public void CopyTo(MediumSave[] array, int arrayIndex) => ((ICollection<MediumSave>)mediumSaves).CopyTo(array, arrayIndex);

        public bool Remove(MediumSave mediumSave)
        {
            if (((ICollection<MediumSave>)mediumSaves).Remove(mediumSave))
            {
                MediumRemoved?.Invoke(mediumSave);
                return true;
            }

            return false;
        }

        IEnumerator<MediumSave> IEnumerable<MediumSave>.GetEnumerator() => ((IEnumerable<MediumSave>)mediumSaves).GetEnumerator();

        public IEnumerator<NodeSave> GetEnumerator() => ((IEnumerable<NodeSave>)nodeSaves).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => nodeSaves.GetEnumerator();

        public ChunkSave ToChunkSave() => new(nodeSaves, mediumSaves, Index, Size);
    }
}