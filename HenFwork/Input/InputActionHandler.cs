// Copyright (c) Affectionate Dove <contact@affectionatedove.com>.
// Licensed under the Affectionate Dove Limited Code Viewing License.
// See the LICENSE file in the repository root for full license text.

using HenBstractions.Input;
using System.Collections.Generic;
using System.Linq;

namespace HenFwork.Input
{
    /// <summary>
    /// Translates keyboard input into <see cref="TInputAction"/>s.
    /// Use <see cref="Propagator"/> to propagate them to listeners.
    /// </summary>
    /// <remarks>
    /// For example, you want to walk forward in your game,
    /// so you create a WalkForward value in a <see cref="TInputAction"/>
    /// enum. You then bind it to key W.
    /// This class then manages monitoring the key "W" and translates
    /// it into the WalkForward action.
    /// If you want to change the keybinding, you only have to change it
    /// in this class. Everywhere else, the WalkForward action
    /// is the same as it was before.
    /// </remarks>
    public abstract class InputActionHandler<TInputAction> where TInputAction : System.Enum
    {
        /// <summary>
        /// For each <see cref="TInputAction"/>, a list of
        /// keyboard keys that have to be pressed together
        /// to trigger that <see cref="TInputAction"/>.
        /// </summary>
        private readonly Dictionary<TInputAction, Keybind> actionKeyBindings = new();

        /// <summary>
        /// It may be expensive to check all keyboard keys
        /// for input at every game update, so this list stores
        /// only the keys that can trigger a <see cref="TInputAction"/>.
        /// </summary>
        private readonly List<KeyInfo> keysToMonitor = new();

        /// <summary>
        /// A list of <see cref="TInputAction"/>s that were
        /// pressed but not yet released.
        /// </summary>
        private readonly List<TInputAction> activeInputActions = new();

        public Inputs Inputs { get; set; }

        public InputPropagator<TInputAction> Propagator { get; }

        /// <summary>
        /// For each <see cref="TInputAction"/>, a list of
        /// keyboard keys that have to be pressed together
        /// to trigger that <see cref="TInputAction"/>.
        /// </summary>
        public IReadOnlyDictionary<TInputAction, Keybind> ActionKeyBindings => actionKeyBindings;

        /// <summary>
        /// A list of <see cref="TInputAction"/>s that were
        /// pressed but not yet released.
        /// </summary>
        public IReadOnlyList<TInputAction> ActiveInputActions => activeInputActions;

        public InputActionHandler(Inputs inputs)
        {
            SetKeyBindings(CreateDefaultKeybindings());
            Inputs = inputs;
            Propagator = CreatePropagator();
        }

        /// <remarks>
        /// Automatically releases all active actions,
        /// triggering <see cref="OnActionRelease"/>
        /// for each of them.
        /// </remarks>
        public void SetKeyBindings(IReadOnlyDictionary<TInputAction, Keybind> keyBindings)
        {
            ReleaseAllActions();
            actionKeyBindings.Clear();
            foreach (var keyValue in keyBindings)
                actionKeyBindings.Add(keyValue.Key, keyValue.Value);
            SetKeysToMonitor();
        }

        public void Update()
        {
            HandleMonitoredKeysPresses();
            HandleActiveActionsReleases();
        }

        /// <summary>
        /// Whether all keys for a <see cref="TInputAction"/>
        /// keybinding are pressed.
        /// </summary>
        public bool AreAllKeysPressedForAction(TInputAction action)
        {
            foreach (var key in ActionKeyBindings[action])
            {
                if (!Inputs.IsKeyDown(key))
                    return false;
            }
            return true;
        }

        protected abstract Dictionary<TInputAction, Keybind> CreateDefaultKeybindings();

        protected virtual InputPropagator<TInputAction> CreatePropagator() => new();

        /// <summary>
        /// Called when all keys in a keybinding
        /// for a given <see cref="TInputAction"/> were pressed.
        /// </summary>
        /// <param name="inputAction">The action that was triggered.</param>
        protected void OnActionPress(TInputAction inputAction) => Propagator.OnActionPressed(inputAction);

        /// <summary>
        /// Triggered when at least one key in a keybinding
        /// for a pressed <see cref="TInputAction"/> was released.
        /// </summary>
        /// <param name="inputAction">The action that was triggered.</param>
        protected void OnActionRelease(TInputAction inputAction) => Propagator.OnActionReleased(inputAction);

        private void HandleMonitoredKeysPresses()
        {
            foreach (var monitoredKeyInfo in keysToMonitor)
            {
                var isPressed = Inputs.IsKeyDown(monitoredKeyInfo.Key);
                var justPressed = isPressed && !monitoredKeyInfo.Pressed;
                monitoredKeyInfo.Pressed = isPressed;

                if (justPressed)
                {
                    foreach (var possibleAction in monitoredKeyInfo.RelatedActions)
                    {
                        // if an action is already active, don't want to trigger it again
                        if (activeInputActions.Contains(possibleAction))
                            continue;

                        if (AreAllKeysPressedForAction(possibleAction))
                        {
                            activeInputActions.Add(possibleAction);
                            OnActionPress(possibleAction);
                        }
                    }
                }
            }
        }

        private void HandleActiveActionsReleases()
        {
            TInputAction activeAction;
            var i = 0;
            while (i < activeInputActions.Count)
            {
                activeAction = activeInputActions[i];
                if (!AreAllKeysPressedForAction(activeAction))
                {
                    activeInputActions.RemoveAt(i);
                    OnActionRelease(activeAction);
                }
                else
                    i++;
            }
        }

        private void ReleaseAllActions()
        {
            foreach (var activeAction in activeInputActions)
                OnActionRelease(activeAction);
            activeInputActions.Clear();
        }

        /// <summary>
        /// Generates contents of <see cref="keysToMonitor"/>
        /// based on <see cref="actionKeyBindings"/>.
        /// </summary>
        private void SetKeysToMonitor()
        {
            keysToMonitor.Clear();
            foreach (var (action, keys) in actionKeyBindings)
            {
                foreach (var key in keys)
                {
                    var keyInfo = keysToMonitor.FirstOrDefault(ki => ki.Key == key);
                    if (keyInfo is null)
                    {
                        keyInfo = new KeyInfo(key);
                        keysToMonitor.Add(keyInfo);
                    }
                    keyInfo.RelatedActions.Add(action);
                }
            }
        }

        private class KeyInfo
        {
            public KeyboardKey Key { get; }
            public bool Pressed { get; set; }
            public List<TInputAction> RelatedActions { get; } = new();

            public KeyInfo(KeyboardKey key) => Key = key;
        }
    }
}