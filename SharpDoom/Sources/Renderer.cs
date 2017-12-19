using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic.RedBlack;
using System.Collections;
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
        public const float renderScale = 16000.0f;

        private static float x_scale = Game.window.Width * (float)Math.Sqrt(3);
        private static float y_scale = Game.window.Height * 60;
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

            Game.window.SwapBuffers();
        }

        private static void DrawSectors()
        {
            var renderedSectors = new RedBlackTree<int, object>() { };
            var sectorQueue = new Queue();
            int currentSector = Game.player.sector;

            var secRender = new SectorRendering(currentSector, 0.0f, Game.window.Width);

            do
            {
                var sector = World.sectors[currentSector];

                for (int i = 0; i < sector.numPoints; i++)
                {
                    int vertexIndex = sector.vertices[i];
                    int vertexIndexNext = sector.vertices[i + 1];

                    QLine temp = new QLine(World.vertices[vertexIndex], World.vertices[vertexIndexNext]);

                    temp.a -= Game.player.pos;
                    temp.b -= Game.player.pos;

                    QLine trans = QMath.RotateLine(temp, Game.player.viewAngle); // x for x, y for z

                    if (trans.a.y < 0 && trans.b.y < 0)
                    {
                        return;
                    }

                    QMath.ClipLine(ref trans);

                    QPoint[] scales = new QPoint[2]
                    {
                        new QPoint(x_scale / trans.a.y, y_scale / trans.a.y),
                        new QPoint(x_scale / trans.b.y, y_scale / trans.b.y)
                    };

                    float x1 = Game.window.Width / 2 - trans.a.x * scales[0].x;
                    float x2 = Game.window.Width / 2 - trans.b.x * scales[1].x;

                    if (x1 >= x2 || x2 < secRender.sx1 || x1 > secRender.sx2)
                    {
                        continue;
                    }

                    float yCeil = World.sectors[currentSector].ceilHeight - Game.player.height;
                    float yFloor = World.sectors[currentSector].floorHeight - Game.player.height;

                    int neighbor = World.sectors[currentSector].neighbors[i];

                } 
            } while (sectorQueue.Count > 0);
        }

        private static void DrawBackGround()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(Color.Black);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0f, Game.window.Width, Game.window.Height, 0.0f, 0.0f, 4.0f);

            GL.ClearColor(Color.DarkGray);
            GL.Color3(Color.Gold);
            GL.Begin(PrimitiveType.Quads);

            GL.Vertex2(0, Game.window.Height / 2);
            GL.Vertex2(Game.window.Width, Game.window.Height / 2);
            GL.Vertex2(Game.window.Width, Game.window.Height);
            GL.Vertex2(0, Game.window.Height);

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
                new Vector3(-Game.window.Width * 0.9f, Game.window.Height * 0.9f, 0.0f),
                QFontAlignment.Left,
                Color.White
                );

            drawing.RefreshBuffers();
            drawing.Draw();
        }
    }
}
