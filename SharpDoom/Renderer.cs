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

using QuickFont;
using QuickFont.Configuration;

namespace SharpDoom
{
    static class Renderer
    {
        public static void RenderFrame()
        {
            DrawBackGround();
            DrawWalls();
            Automap.Draw();

            //DrawHUD();

            Game.window.SwapBuffers();
        }

        private static void DrawBackGround()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //GL.ClearColor(Color.Black);
            GL.ClearColor(Color.DarkGray);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-renderScale / 9.0f, renderScale / 9.0f, -renderScale / 16.0f, renderScale / 16.0f, 0.0f, 4.0f);

            GL.Color3(Color.Gold);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(-Game.window.Width, Game.window.Height);
            GL.Vertex2(Game.window.Width, Game.window.Height);
            GL.Vertex2(Game.window.Width, 0);
            GL.Vertex2(-Game.window.Width, 0);

            GL.End();
        }

        private static void DrawWalls()
        {
            foreach (Wall curWall in World.walls)
            {
                DrawWall(curWall);
            }
        }

        private static void DrawWall(Wall wall)
        {
            QPoint[] img = new QPoint[2];
            QLine temp = wall.line;

            temp.a -= Game.player.pos;
            temp.b -= Game.player.pos;

            QLine trans = QMath.RotateLine(temp, Game.player.viewAngle); // x for x, y for z

            if (debug)
            {
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex2(trans.a.x + Game.window.Width / 2, trans.a.y);
                GL.Vertex2(trans.b.x + Game.window.Width / 2, trans.b.y);
                #region drawFOV
                GL.Color3(Color.Gray);

                GL.Vertex2(Game.window.Width / 2,
                            0);
                GL.Vertex2(-QMath.Sin(Player.FOV / 2) * Automap.viewLength + Game.window.Width / 2,
                           QMath.Cos(Player.FOV / 2) * Automap.viewLength);

                GL.Vertex2(Game.window.Width / 2,
                           0);
                GL.Vertex2(-QMath.Sin(-Player.FOV / 2) * Automap.viewLength + Game.window.Width / 2,
                           QMath.Cos(-Player.FOV / 2) * Automap.viewLength);
                #endregion
                GL.End();
                #region drawPosition
                GL.Begin(PrimitiveType.Quads);
                GL.Color3(Color.Thistle);

                int[] a = { -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    GL.Vertex2(a[0] * Player.width + Game.window.Width / 2,
                               a[1] * Player.width);

                    a[i % 2] *= -1;
                }

                GL.End();
                #endregion
            }
            if (trans.a.y < 0 && trans.b.y < 0) // Depth of two vertices less than zero so we don't need to draw wall
            {
                return;
            }

            #region shit
            /*
            if (trans.a.y <= 0 || trans.b.y <= 0)
            {
                img[0] = QMath.Intersect(trans, new QLine(new QPoint(-near.x, near.y), new QPoint(-far.x, far.y)));
                img[1] = QMath.Intersect(trans, new QLine(near, far));

                if (trans.a.y <= 0)
                {
                    if (img[0].y > 0)
                    {
                        trans.a = img[0];
                    }
                    else
                    {
                        trans.a = img[1];
                    }
                }
                else
                {
                    if (img[0].y > 0)
                    {
                        trans.b = img[0];
                    }
                    else
                    {
                        trans.b = img[1];
                    }
                }
            }
            */
            #endregion

            QPoint vec = trans.a - trans.b;
            float y0 = 1e-4f;

            if (trans.a.y < 0)
            {
                trans.a.y = y0;
                trans.a.x = trans.b.x + vec.x * (y0 - trans.b.y) / vec.y;
            }

            if (trans.b.y < 0)
            {
                trans.b.y = y0;
                trans.b.x = trans.a.x + vec.x * (y0 - trans.a.y) / vec.y;
            }

            if (debug)
            {
                GL.LineWidth(3.0f);
                GL.Begin(PrimitiveType.Lines);

                GL.Vertex2(trans.a.x + Game.window.Width / 2, trans.a.y);
                GL.Vertex2(trans.b.x + Game.window.Width / 2, trans.b.y);

                GL.End();
            }

            float x1 = trans.a.x * x_scale / trans.a.y;
            float x2 = trans.b.x * x_scale / trans.b.y;

            QLine[] walls = new QLine[2];

            for (int i = 0; i < 2; i++)
            {
                walls[i].a.y = (i == 0 ? -y_scale : y_scale) / trans.a.y;
                walls[i].b.y = (i == 0 ? -y_scale : y_scale) / trans.b.y;
            }

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(wall.clr);

            GL.Vertex2(x1, walls[0].a.y);
            GL.Vertex2(x2, walls[0].b.y);

            GL.Vertex2(x1, walls[1].a.y);
            GL.Vertex2(x2, walls[1].b.y);

            GL.Vertex2(x1, walls[0].a.y);
            GL.Vertex2(x1, walls[1].a.y);

            GL.Vertex2(x2, walls[0].b.y);
            GL.Vertex2(x2, walls[1].b.y);

            GL.End();

            GL.LineWidth(2.0f);
            GL.Begin(PrimitiveType.LineLoop);

            GL.Color3(Color.Black);

            GL.Vertex2(x1, walls[0].a.y);
            GL.Vertex2(x2, walls[0].b.y);
            GL.Vertex2(x2, walls[1].b.y);
            GL.Vertex2(x1, walls[1].a.y);

            GL.End();
        }
        private static void DrawHUD()
        {
            drawing.DrawingPrimitives.Clear();
            drawing.ProjectionMatrix = projectionMatrix;

            drawing.Print(
                font,
                "FPS: " + Tools.CalculateFrameRate(),
                new Vector3(-Game.window.Width * 0.9f, Game.window.Height * 0.9f, 0.0f),
                QFontAlignment.Left,
                Color.White
                );

            drawing.RefreshBuffers();
            drawing.Draw();
        }

        public static float renderScale = 16000.0f;

        private const float x_scale = 3000.0f;    
        private const float y_scale = 128000.0f;
        private const float z_scale = 0.0001f;

        private static QPoint near = new QPoint(1e-5f, 1e-4f);
        private static QPoint far = new QPoint(2000, 500);

        public static QFont font;
        public static QFontDrawing drawing;

        public static Matrix4 projectionMatrix;

        private const bool debug = false;
    }
}
