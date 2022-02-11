// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Screens.Editor.Tools;
using System;

namespace HenFwork.MapEditing.Screens.Editor
{
    /// <summary>
    ///     Manages the <see cref="Tool"/>s for the <see cref="EditorScreen"/>.
    /// </summary>
    public class ToolsManager
    {
        private Tool selectedTool;

        public event Action<Tool> SelectedToolChanged;

        public Tool SelectedTool
        {
            get => selectedTool;
            set
            {
                selectedTool = value;
                SelectedToolChanged?.Invoke(selectedTool);
            }
        }
    }
}