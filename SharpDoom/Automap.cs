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
        public static void Draw()
        {
            DrawPlayer();
            DrawWalls();
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
            GL.Color3(Color.Gray);

            GL.Vertex2(Game.player.pos.x - Game.window.Width / 2,
                        Game.player.pos.y);
            GL.Vertex2(Game.player.pos.x + QMath.Cos(Game.player.viewAngle + Player.FOV / 2) * viewLength - Game.window.Width / 2,
                        Game.player.pos.y + QMath.Sin(Game.player.viewAngle + Player.FOV / 2) * viewLength);

            GL.Vertex2(Game.player.pos.x - Game.window.Width / 2,
                        Game.player.pos.y);
            GL.Vertex2(Game.player.pos.x + QMath.Cos(Game.player.viewAngle - Player.FOV / 2) * viewLength - Game.window.Width / 2,
                       Game.player.pos.y + QMath.Sin(Game.player.viewAngle - Player.FOV / 2) * viewLength);

            GL.End();
            #endregion
            #region drawPosition
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Thistle);

            int[] a = { -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                GL.Vertex2(Game.player.pos.x + a[0] * Player.width - Game.window.Width / 2,
                            Game.player.pos.y + a[1] * Player.width);

                a[i % 2] *= -1;
            }

            GL.End();
            #endregion
        }

        private static void DrawWalls()
        {
            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.Lines);

            // Delimiter
            GL.Color3(Color.Red);
            GL.Vertex2(0.0f, Game.window.Height);
            GL.Vertex2(0.0f, -Game.window.Height);

            GL.Color3(Color.DarkBlue);
            GL.Vertex2(-Game.window.Width / 2, Game.window.Height);
            GL.Vertex2(-Game.window.Width / 2, -Game.window.Height);

            GL.Vertex2(Game.window.Width / 2, Game.window.Height);
            GL.Vertex2(Game.window.Width / 2, -Game.window.Height);

            GL.Vertex2(-Game.window.Width, 0);
            GL.Vertex2(Game.window.Width, 0);

            // Absolute

            foreach (Wall wall in World.walls)
            {
                GL.Color3(wall.clr);
                QLine line = wall.line;

                GL.Vertex2(line.a.x - Game.window.Width / 2, line.a.y);
                GL.Vertex2(line.b.x - Game.window.Width / 2, line.b.y);
            }

            GL.End();
        }

        public const float viewLength = 30000.0f;
    }
}
