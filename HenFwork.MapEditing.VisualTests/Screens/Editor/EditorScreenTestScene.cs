// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.MapEditing.Screens.Editor;
using HenFwork.Testing;
using HenFwork.Testing.Input;

namespace HenFwork.MapEditing.VisualTests.Screens.Editor
{
    public class EditorScreenTestScene : VisualTestScene
    {
        public EditorScreenTestScene() => AddChild(new EditorScreen<SceneControls> { RelativeSizeAxes = Axes.Both });
    }
}