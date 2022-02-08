// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.Input
{
    public record Keybind : IReadOnlySet<KeyboardKey>
    {
        private readonly IReadOnlySet<KeyboardKey> set;

        public Keybind(IEnumerable<KeyboardKey> keys)
        {
            var set = new HashSet<KeyboardKey>(keys);
            if (set.Count == 0)
                throw new ArgumentException("The amount of provided keys has to be positive.", nameof(keys));
            this.set = set;
        }

        public Keybind(params KeyboardKey[] keys) : this(keys.AsEnumerable())
        {
        }

        public int Count => set.Count;

        public bool Contains(KeyboardKey item) => set.Contains(item);
        public IEnumerator<KeyboardKey> GetEnumerator() => set.GetEnumerator();
        public bool IsProperSubsetOf(IEnumerable<KeyboardKey> other) => set.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<KeyboardKey> other) => set.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<KeyboardKey> other) => set.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<KeyboardKey> other) => set.IsSupersetOf(other);
        public bool Overlaps(IEnumerable<KeyboardKey> other) => set.Overlaps(other);
        public bool SetEquals(IEnumerable<KeyboardKey> other) => set.SetEquals(other);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)set).GetEnumerator();
    }
}