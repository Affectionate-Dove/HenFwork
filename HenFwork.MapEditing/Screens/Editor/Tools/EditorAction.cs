// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using System;

namespace HenFwork.MapEditing.Screens.Editor.Tools
{
    public class EditorAction
    {
        public string Name { get; set; }

        public Action Action { get; set; }

        public string TextureName { get; set; }

        public EditorAction(string name, Action action, string textureName)
        {
            Name = name;
            Action = action;
            TextureName = textureName;
        }
    }
}