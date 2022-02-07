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
        public List<Saves.WorldSave> worldSaves = new List<Saves.WorldSave>();

        public MapSelectScreenTestScene()
        {
            worldSaves.Add(new Saves.WorldSave(new List<ChunkSave>())
            {
                WorldName = "Map 1"
            });
            worldSaves.Add(new Saves.WorldSave(new List<ChunkSave>())
            {
                WorldName = "Map 2"
            });
            worldSaves.Add(new Saves.WorldSave(new List<ChunkSave>())
            {
                WorldName = "Map 3"
            });
            AddChild(new MapSelectScreen(worldSaves));
        }
    }
}