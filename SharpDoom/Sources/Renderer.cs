using System;
using System.Collections.Generic.RedBlack;
using System.Collections;
using System.Drawing;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using QuickFont;
using static SharpDoom.QMath;
using static SharpDoom.Game;


namespace SharpDoom
{
    static class Renderer
    {
        public const float renderScale = 16000.0f;

        private static float x_scale = window.Height * (float)Math.Sqrt(3);
        private static float y_scale = window.Height * 0.1f;
        private static float z_scale = 0.000001f;

        private static QPoint near = new QPoint(1e-5f, 1e-4f);
        private static QPoint far = new QPoint(2000, 500);

        public static QFont font;
        public static QFontDrawing drawing;

        public static Matrix4 projectionMatrix;

        private const bool debug = false;

        public static void RenderFrame()
        {
            DrawBackGround();
            DrawSectors();
            Automap.Draw();

            //DrawHUD();

            window.SwapBuffers();
        }

        private static void DrawSectors()
        {
            var renderedSectors = new RedBlackTree<int, object>() { };
            var sectorQueue = new Queue();
            int currentSector = player.sector;

            var secRender = new SectorRendering(currentSector, 0, window.Width - 1);

            int[] yTop = new int[window.Width];
            int[] yBottom = Enumerable.Repeat(window.Height - 1, window.Width).ToArray();

            GL.Begin(PrimitiveType.Lines);

            do
            {
                var sector = World.sectors[currentSector];

                for (int i = 0; i < sector.numPoints; i++)
                {
                    int vertexIndex = sector.vertices[i];
                    int vertexIndexNext = sector.vertices[i + 1];

                    QLine temp = new QLine(World.vertices[vertexIndex], World.vertices[vertexIndexNext]);

                    temp.a -= player.pos;
                    temp.b -= player.pos;

                    QLine trans = RotateLine(temp, player.viewAngle); // x for x, y for z

                    if (trans.a.y < 0 && trans.b.y < 0)
                    {
                        return;
                    }

                    ClipLine(ref trans);

                    QPoint[] scales = new QPoint[2]
                    {
                        new QPoint(x_scale / trans.a.y, y_scale / trans.a.y),
                        new QPoint(x_scale / trans.b.y, y_scale / trans.b.y)
                    };

                    int x1 = window.Width / 2 - (int)(trans.a.x * scales[0].x);
                    int x2 = window.Width / 2 - (int)(trans.b.x * scales[1].x);

                    if (x1 >= x2 || x2 < secRender.sx1 || x1 > secRender.sx2)
                    {
                        continue;
                    }

                    float yCeil = World.sectors[currentSector].ceilHeight - player.height;
                    float yFloor = World.sectors[currentSector].floorHeight - player.height;

                    int neighbor = World.sectors[currentSector].neighbors[i];

                    float nyCeil = 0.0f, nyFloor = 0.0f;
                    if (neighbor >= 0)
                    {
                        nyCeil = World.sectors[currentSector].ceilHeight - player.height;
                        nyFloor = World.sectors[currentSector].floorHeight - player.height;
                    }

                    // TODO: ADD YAW
                    int y1a = window.Height / 2 - (int)(yCeil * scales[0].y);
                    int y2a = window.Height / 2 - (int)(yCeil * scales[1].y);
                    int y1b = window.Height / 2 - (int)(yFloor * scales[0].y);
                    int y2b = window.Height / 2 - (int)(yFloor * scales[1].y);

                    int ny1a = window.Height / 2 - (int)(nyCeil * scales[0].y);
                    int ny2a = window.Height / 2 - (int)(nyCeil * scales[1].y);
                    int ny1b = window.Height / 2 - (int)(nyFloor * scales[0].y);
                    int ny2b = window.Height / 2 - (int)(nyFloor * scales[1].y);

                    int beginX = Math.Max(x1, secRender.sx1);
                    int endX = Math.Min(x2, secRender.sx2);

                    for (int x = beginX; x <= endX; x++)
                    {
                        int cya = (x - x1) * (y2a - y1a) / (x2 - x1) + y1a;
                        Clamp(ref cya, yTop[x], yBottom[x]);
                        int cyb = (x - x1) * (y2b - y1b) / (x2 - x1) + y1b;
                        Clamp(ref cyb, yTop[x], yBottom[x]);

                        GL.Color3(Color.Gray);
                        GL.Vertex2(x, yTop[x]);
                        GL.Vertex2(x, cya - 1);

                        GL.Color3(Color.CadetBlue);
                        GL.Vertex2(x, yBottom[x]);
                        GL.Vertex2(x, cyb + 1);

                        if (neighbor >= 0)
                        {
                            int cnya = (x - x1) * (ny2a - ny1a) / (x2 - x1) + ny1a;
                            Clamp(ref cnya, yTop[x], yBottom[x]);
                            int cnyb = (x - x1) * (ny2b - ny1b) / (x2 - x1) + ny1b;
                            Clamp(ref cnyb, yTop[x], yBottom[x]);

                            if (x == x1 || x == x2)
                            {
                                GL.Color3(Color.Black);
                            }
                            else
                            {
                                GL.Color3(Color.White);
                            }

                            GL.Vertex2(x, cya);
                            GL.Vertex2(x, cnya);

                            int max = Math.Max(cya, cnya);
                            Clamp(ref max, yTop[x], window.Height - 1);
                            yTop[x] = max;   // Shrink the remaining window below these ceilings
                                             /* If our floor is lower than their floor, render bottom wall */

                            if (x == x1 || x == x2)
                            {
                                GL.Color3(Color.Black);
                            }
                            else
                            {
                                GL.Color3(Color.White);
                            }

                            GL.Vertex2(x, cnyb);
                            GL.Vertex2(x, cyb);

                            int min = Math.Max(cyb, cnyb);
                            Clamp(ref min, 0, yBottom[x]);
                            yBottom[x] = min;
                        }
                        else
                        {

                        }
                    }


                }
            } while (sectorQueue.Count > 0);

            GL.End();
        }

        private static void DrawBackGround()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, window.Width, window.Height, 0.0f, 0.0f, 4.0f);

            return;

            GL.ClearColor(Color.DarkGray);
            GL.Color3(Color.Gold);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(0, window.Height / 2);
            GL.Vertex2(window.Width, window.Height / 2);
            GL.Vertex2(window.Width, window.Height);
            GL.Vertex2(0, window.Height);

            GL.End();
        }

        private static void DrawWalls()
        {
            /*foreach (Wall curWall in World.walls)
            {
                DrawWall(curWall);
            }*/

            //DrawWall(new Wall(new QLine(new QPoint(100, 100), new QPoint(100, -100)), Color.Lime));
        }

        private static void DrawWall(Wall wall)
        {
            
        }
        private static void DrawHUD()
        {
            drawing.DrawingPrimitives.Clear();
            drawing.ProjectionMatrix = projectionMatrix;

            drawing.Print(
                font,
                "FPS: " + Tools.CalculateFrameRate(),
                new Vector3(-window.Width * 0.9f, window.Height * 0.9f, 0.0f),
                QFontAlignment.Left,
                Color.White
                );

            drawing.RefreshBuffers();
            drawing.Draw();
        }
    }
}
