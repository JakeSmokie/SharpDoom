using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SharpDoom
{
    class MyApplication
    {
        [STAThread]
        public static void Main()
        {
            Game.InitWindow();
        }
    }
}