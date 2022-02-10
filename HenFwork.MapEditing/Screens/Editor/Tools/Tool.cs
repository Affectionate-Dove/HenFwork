// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Generic;

namespace HenFwork.MapEditing.Screens.Editor.Tools
{
    public class Tool
    {
        public string TextureName { get; }

        public List<EditorAction> Actions { get; } = new();

        public List<EditorAction> Modes { get; } = new();

        public Tool(string textureName) => TextureName = textureName;
    }
}