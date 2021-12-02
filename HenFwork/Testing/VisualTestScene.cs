// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenFwork.Graphics2d;
using HenFwork.Input;
using HenFwork.Screens;
using HenFwork.Testing.Input;
using System.Collections.Generic;

namespace HenFwork.Testing
{
    public abstract class VisualTestScene : Screen, IInputListener<SceneControls>
    {
        public bool IsSceneDone { get; protected set; }

        public virtual string Description { get; }

        public virtual Dictionary<List<SceneControls>, string> ControlsDescriptions { get; }

        public VisualTestScene() => RelativeSizeAxes = Axes.Both;

        public virtual bool OnActionPressed(SceneControls action) => false;

        public virtual void OnActionReleased(SceneControls action)
        {
        }
    }
}