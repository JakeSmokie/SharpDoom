using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using QuickFont;
using QuickFont.Configuration;

namespace SharpDoom
{
    public static class Game
    {
        public static void InitWindow()
        {
            window = new GameWindow(1920, 1080, new GraphicsMode(), "SharpDoom 0.1a", GameWindowFlags.Fullscreen);

            using (window)
            {
                window.Load += (sender, e) =>
                {
                    Load();
                };

                window.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, window.Width, window.Height);
                };

                window.UpdateFrame += (sender, e) =>
                {
                    Input.Handle();
                };

                window.RenderFrame += (sender, e) =>
                {
                    Renderer.RenderFrame();
                };

                window.MouseDown += Input.MouseDown;

                window.Run(60.0);
            }
        }

        private static void Load()
        {
            window.VSync            = VSyncMode.On;
            window.CursorVisible    = false;

            Cursor.Position = new Point(Game.window.Width / 2, Game.window.Height / 2);

            player = new Player();
            World.Init();

            //Renderer.font = new QFont("Fonts/HappySans.ttf", 32.0f, new QFontBuilderConfiguration(true));
            //Renderer.drawing = new QFontDrawing();

            Renderer.projectionMatrix = Matrix4.CreateOrthographicOffCenter(-Renderer.renderScale / 9.0f, Renderer.renderScale / 9.0f, -Renderer.renderScale / 16.0f, Renderer.renderScale / 16.0f, 0.0f, 4.0f);
        }

        public static GameWindow window;
        public static Player player;
    }
}
