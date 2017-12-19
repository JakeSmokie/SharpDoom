using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SharpDoom
{
    public static class World
    {
        public static void Init()
        {
            walls = new List<Wall> { };

            walls.Add(new Wall(new QLine(new QPoint(-400, 400), new QPoint(400, 400)), Color.Peru));
            walls.Add(new Wall(new QLine(new QPoint(400, 400), new QPoint(600, 0)), Color.Red));
            walls.Add(new Wall(new QLine(new QPoint(600, 0), new QPoint(400, -400)), Color.Yellow));
            walls.Add(new Wall(new QLine(new QPoint(400, -400), new QPoint(-400, -400)), Color.Lime));
            walls.Add(new Wall(new QLine(new QPoint(400, -400), new QPoint(-400, -400)), Color.Lime));
            walls.Add(new Wall(new QLine(new QPoint(-400, -400), new QPoint(-400, 400)), Color.Blue));
        }

        public static List<Wall> walls;
    }
}
