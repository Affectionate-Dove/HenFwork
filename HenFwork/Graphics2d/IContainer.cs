// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d.Layouts;
using System.Collections.Generic;

namespace HenFwork.Graphics2d
{
    public interface IContainer
    {
        ContainerLayoutInfo ContainerLayoutInfo { get; }
    }

    public interface IContainer<out T> : IContainer
    {
        public IEnumerable<T> Children { get; }
    }
}