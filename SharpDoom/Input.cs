using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SharpDoom
{
    static class Input
    {
        public static void Handle()
        {
            GetKeyboardInput();
            GetMouseInput();

        }

        private static void GetKeyboardInput()
        {
            if (Game.window.Keyboard[Key.Escape])
            {
                Game.window.Exit();
            }

            if (Game.window.Keyboard[Key.W] || Game.window.Keyboard[Key.Up])
            {
                Game.player.pos.x += Player.speed * QMath.Cos(Game.player.viewAngle);
                Game.player.pos.y += Player.speed * QMath.Sin(Game.player.viewAngle);
            }
            if (Game.window.Keyboard[Key.S] || Game.window.Keyboard[Key.Down])
            {
                Game.player.pos.x -= Player.speed * QMath.Cos(Game.player.viewAngle);
                Game.player.pos.y -= Player.speed * QMath.Sin(Game.player.viewAngle);
            }
            if (Game.window.Keyboard[Key.A])
            {
                Game.player.pos.x += Player.speed * QMath.Cos(Game.player.viewAngle + (float)Math.PI / 2);
                Game.player.pos.y += Player.speed * QMath.Sin(Game.player.viewAngle + (float)Math.PI / 2);
            }
            if (Game.window.Keyboard[Key.D])
            {
                Game.player.pos.x += Player.speed * QMath.Cos(Game.player.viewAngle - (float)Math.PI / 2);
                Game.player.pos.y += Player.speed * QMath.Sin(Game.player.viewAngle - (float)Math.PI / 2);
            }
            if (Game.window.Keyboard[Key.Left])
            {
                Game.player.viewAngle += keyboardSensitivity;
            }
            if (Game.window.Keyboard[Key.Right])
            {
                Game.player.viewAngle -= keyboardSensitivity;
            }
        }

        private static void GetMouseInput()
        {
            float deltaX = Game.window.Mouse.X - Game.window.Width / 2;
            Game.player.viewAngle += (deltaX) * mouseSensitivity;

            Cursor.Position = new Point(Game.window.Width / 2, Game.window.Height / 2);
        }

        public static void MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private static float mouseSensitivity = -0.001f;
        private static float keyboardSensitivity = 0.0f;
    }
}
