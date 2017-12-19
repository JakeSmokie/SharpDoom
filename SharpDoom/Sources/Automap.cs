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
    public static class Automap
    {
        public const float viewLength = 30000.0f;
        private const float mapScale = 25.0f;

        public static void Draw()
        {
            DrawPlayer();
            DrawVertices();
            DrawSectors();
        }

        private static void DrawVertices()
        {
            const float vertexWidth = 4.0f;
            GL.Begin(PrimitiveType.Quads);

            foreach (QPoint vertex in World.vertices)
            {
                GL.Color3(Color.Black);

                int[] a = { -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    GL.Vertex2(mapScale * vertex.x + a[0] * vertexWidth + Game.window.Width / 4,
                               mapScale * vertex.y + a[1] * vertexWidth + Game.window.Height / 4);

                    a[i % 2] *= -1;
                }
            }

            GL.End();
        }

        private static void DrawPlayer()
        {
            GL.Begin(PrimitiveType.Lines);

            #region drawBasis 
            /*
            GL.Color3(Color.Yellow);

            GL.Vertex2(Game.player.pos.x - QMath.Cos(Game.player.viewAngle) * viewLength - Game.window.Width / 2,
                        Game.player.pos.y - QMath.Sin(Game.player.viewAngle) * viewLength);
            GL.Vertex2(Game.player.pos.x + QMath.Cos(Game.player.viewAngle) * viewLength - Game.window.Width / 2,
                        Game.player.pos.y + QMath.Sin(Game.player.viewAngle) * viewLength);

            GL.Vertex2(Game.player.pos.x + QMath.Sin(Game.player.viewAngle) * viewLength - Game.window.Width / 2,
                        Game.player.pos.y - QMath.Cos(Game.player.viewAngle) * viewLength);
            GL.Vertex2(Game.player.pos.x - QMath.Sin(Game.player.viewAngle) * viewLength - Game.window.Width / 2,
                        Game.player.pos.y + QMath.Cos(Game.player.viewAngle) * viewLength);
            */
            #endregion
            #region drawFOV
            GL.Color3(Color.Gold);

            GL.Vertex2(mapScale * Game.player.pos.x + Game.window.Width / 4,
                       mapScale * Game.player.pos.y + Game.window.Height / 4);
            GL.Vertex2(mapScale * Game.player.pos.x + QMath.Cos(Game.player.viewAngle + Player.FOV / 2) * viewLength + Game.window.Width / 4,
                       mapScale * Game.player.pos.y + QMath.Sin(Game.player.viewAngle + Player.FOV / 2) * viewLength + Game.window.Height / 4);

            GL.Vertex2(mapScale * Game.player.pos.x + Game.window.Width / 4,
                       mapScale * Game.player.pos.y + Game.window.Height / 4);
            GL.Vertex2(mapScale * Game.player.pos.x + QMath.Cos(Game.player.viewAngle - Player.FOV / 2) * viewLength + Game.window.Width / 4,
                       mapScale * Game.player.pos.y + QMath.Sin(Game.player.viewAngle - Player.FOV / 2) * viewLength + Game.window.Height / 4);

            GL.End();
            #endregion
            #region drawPosition
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Thistle);

            int[] a = { -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                GL.Vertex2(mapScale * Game.player.pos.x + a[0] * Player.width + Game.window.Width / 4,
                           mapScale * Game.player.pos.y + a[1] * Player.width + Game.window.Height / 4);

                a[i % 2] *= -1;
            }

            GL.End();
            #endregion
        }

        private static void DrawSectors()
        {
            GL.LineWidth(2.0f);
            GL.Begin(PrimitiveType.Lines);

            foreach (Sector sector in World.sectors)
            {
                for (int i = 0; i < sector.vertices.Count - 1; i++)
                {
                    //Console.WriteLine("V: " + sector.vertices.Count);
                    //Console.WriteLine("N: " + sector.neighbors.Count);

                    int vertexIndex = sector.vertices[i];
                    int vertexIndexNext = sector.vertices[i + 1];

                    QPoint a = World.vertices[vertexIndex];
                    QPoint b = World.vertices[vertexIndexNext];

                    if (sector.neighbors[(i) % sector.vertices.Count] != -1)
                    {
                        GL.Color3(Color.Red);
                    }
                    else
                    {
                        GL.Color3(Color.Lime);

                        //GL.End();
                        //continue;
                    }

                    GL.Vertex2(mapScale * a.x + Game.window.Width / 4, mapScale * a.y + Game.window.Height / 4);
                    GL.Vertex2(mapScale * b.x + Game.window.Width / 4, mapScale * b.y + Game.window.Height / 4);
                }
            }

            GL.End();
        }
    }
}
