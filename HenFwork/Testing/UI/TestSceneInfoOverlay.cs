// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Graphics;
using HenFwork.Graphics2d;
using HenFwork.UI;

namespace HenFwork.Testing.UI
{
    public class TestSceneInfoOverlay : Container
    {
        private readonly FillFlowContainer flowContainer;

        public TestSceneInfoOverlay()
        {
            // background
            AddChild(new Rectangle { RelativeSizeAxes = Axes.Both, Color = ColorInfo.DARKGRAY });

            AddChild(flowContainer = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,
                Direction = Direction.Vertical,
                Padding = new() { Horizontal = 10, Vertical = 10 },
                Spacing = 10
            });
        }

        public void ChangeScene(VisualTestScene testScene)
        {
            flowContainer.Clear();

            if (testScene.Description is not null)
            {
                flowContainer.AddChild(new SpriteText { Text = "Description", FontSize = 20 });
                flowContainer.AddChild(new SpriteText { Text = testScene.Description });
            }

            if (testScene.ControlsDescriptions is not null && testScene.ControlsDescriptions.Count > 0)
            {
                flowContainer.AddChild(new SpriteText { Text = "Controls", FontSize = 20 });
                foreach (var (controls, description) in testScene.ControlsDescriptions)
                    flowContainer.AddChild(new SpriteText { Text = $"{string.Join(", ", controls)}: {description}" });
            }

            Visible = testScene.Description is not null || (testScene.ControlsDescriptions is not null && testScene.ControlsDescriptions.Count > 0);
        }
    }
}