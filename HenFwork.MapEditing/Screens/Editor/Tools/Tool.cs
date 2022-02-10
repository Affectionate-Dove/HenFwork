// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.Editor.Tools
{
    /// <summary>
    ///     Represents a group of <see cref="EditorAction"/>s.
    /// </summary>
    public class Tool
    {
        public string TextureName { get; }

        /// <summary>
        ///     Single-activation actions.
        /// </summary>
        public List<EditorAction> Actions { get; } = new();

        /// <summary>
        ///     Persistent multi-use actions.
        /// </summary>
        public List<EditorAction> Modes { get; } = new();

        public Tool(string textureName) => TextureName = textureName;
    }
}