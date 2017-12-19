using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace SharpDoom
{
    public struct QPoint
    {
        public QPoint(float a, float b)
        {
            x = a;
            y = b;
        }

        public static QPoint operator -(QPoint a, QPoint b)
        {
            return new QPoint(a.x - b.x, a.y - b.y);
        }
        public static QPoint operator +(QPoint a, QPoint b)
        {
            return new QPoint(a.x + b.x, a.y + b.y);
        }

        public float x;
        public float y;
    }

    public struct QLine
    {
        public QLine(QPoint x, QPoint y)
        {
            a = x;
            b = y;
        }

        public QPoint a, b;
    }

    public struct Wall
    {
        public Wall(QLine a, Color b)
        {
            line = a;
            clr = b;
        }

        public Color clr;
        public QLine line;
    }

    
}
