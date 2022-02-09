// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Saves;
using HenFwork.MapEditing.Screens.MapSelect;
using HenFwork.Testing;
using HenFwork.UI;
using System.Collections.Generic;

namespace HenFwork.MapEditing.VisualTests.Screens.MapSelect
{
    public class MapSelectScreenTestScene : VisualTestScene
    {
        public MapSelectScreenTestScene()
        {
            var confirmedMapText = new SpriteText();
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
                confirmedMapText.Text = $"confirmed map {worldSave.Name}";
            };
            var mapSelectScreen = new MapSelectScreen(worldSaves, confirmAction);
            AddChild(mapSelectScreen);
            AddChild(confirmedMapText);
        }
    }
}