using System;
using amulware.Graphics;
using Bearded.Utilities.Input;
using Clouds.Game;
using OpenTK;
using OpenTK.Graphics;

namespace Clouds
{
    sealed class GameWindow : amulware.Graphics.Program
    {
        private GameRenderer renderer;
        private GameState game;

        public GameWindow()
            : base(1280, 720, GraphicsMode.Default, "The Game", GameWindowFlags.Default, DisplayDevice.Default, 3, 2, GraphicsContextFlags.Default)
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            this.renderer = new GameRenderer();

            this.game = new GameState();

            InputManager.Initialize(null);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
            InputManager.Update();

            this.game.Update(e);
        }

        protected override void OnRender(UpdateEventArgs e)
        {
            this.renderer.PrepareFrame(this.Width, this.Height);

            this.renderer.Draw(this.game);
            this.renderer.FinaliseFrame();

            this.SwapBuffers();
        }
    }
}