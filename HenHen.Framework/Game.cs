﻿using HenHen.Framework.Input;
using HenHen.Framework.IO.Stores;
using HenHen.Framework.Screens;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace HenHen.Framework
{
    public class Game
    {
        private static readonly TextureStore TextureStore;

        public Window Window { get; }

        public ScreenStack ScreenStack { get; }

        public InputManager InputManager { get; }

        public Game()
        {
            Window = new Window(new Vector2(600, 400), "HenHen");
            InputManager = CreateInputManager();
            ScreenStack = new ScreenStack();
        }

        /// <param name="timeDelta">In seconds.</param>
        public void Loop(float timeDelta)
        {
            Update(timeDelta);
            Draw();
        }

        protected virtual InputManager CreateInputManager() => new InputManager();

        protected virtual void OnUpdate()
        {
        }

        protected virtual void OnRender()
        {
        }

        private static TextureStore GetTextureStore() => TextureStore;

        private void Draw()
        {
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.BLACK);
            ScreenStack.Render();
            OnRender();
            EndDrawing();
        }

        /// <param name="timeDelta">In seconds.</param>
        private void Update(float timeDelta)
        {
            ScreenStack.Size = Window.Size;
            ScreenStack.Update();
            InputManager.Update(timeDelta);
            OnUpdate();
        }
    }
}