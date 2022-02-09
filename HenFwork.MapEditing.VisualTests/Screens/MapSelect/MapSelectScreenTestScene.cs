// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Screens.MapSelect;
using HenFwork.Testing;
using System.Collections.Generic;

namespace HenFwork.MapEditing.VisualTests.Screens.MapSelect
{
    public class MapSelectScreenTestScene : VisualTestScene
    {
        public MapSelectScreenTestScene()
        {
            var worldSaves = new List<WorldSave>();
            for (var i = 1; i < 9; i++)
            {
                worldSaves.Add(new WorldSave(new List<ChunkSave>())
                {
                    Name = $"Map {i}"
                });
            }
            var confirmAction = (WorldSave worldSave) =>
            {
                MapSelectScreen.confirmMapName.Text = worldSave.Name;
            };
            AddChild(new MapSelectScreen(worldSaves, confirmAction));
        }
    }
}