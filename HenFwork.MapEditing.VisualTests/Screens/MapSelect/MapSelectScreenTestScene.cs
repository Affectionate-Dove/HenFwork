// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.MapEditing.Screens.MapSelect;
using HenFwork.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenFwork.MapEditing.VisualTests.Screens.MapSelect
{
    public class MapSelectScreenTestScene : VisualTestScene
    {
        public MapSelectScreenTestScene() => AddChild(new MapSelectScreen());
    }
}
