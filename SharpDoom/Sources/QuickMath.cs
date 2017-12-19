using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDoom
{
    public static class QMath
    {
        public static Func<double, float> Cos = angleR => (float)System.Math.Cos(angleR);
        public static Func<double, float> Sin = angleR => (float)System.Math.Sin(angleR);

        public static QPoint Intersect(QLine m, QLine n)
        {
            QPoint point, temp;

            point.x = Matrix.Det(m.a, m.b);
            point.y = Matrix.Det(n.a, n.b);

            float det = Matrix.Det(m.a - m.b, n.a - n.b);

            temp.x = Matrix.Det(new QPoint(point.x, (m.a - m.b).x), new QPoint(point.y, (n.a - n.b).x)) / det;
            temp.y = Matrix.Det(new QPoint(point.x, (m.a - m.b).y), new QPoint(point.y, (n.a - n.b).y)) / det;

            return temp;
        }

        public static QLine RotateLine(QLine line, float angle)
        {
            QLine res;

            float psin = Sin(angle);
            float pcos = Cos(angle);

            res.a.x = line.a.x * psin - line.a.y * pcos;
            res.b.x = line.b.x * psin - line.b.y * pcos;

            res.a.y = line.a.x * pcos + line.a.y * psin;
            res.b.y = line.b.x * pcos + line.b.y * psin;

            return res;
        }

        public static void ClipLine(ref QLine line)
        {
            QPoint vec = line.a - line.b;
            const float y0 = 1e-4f;

            if (line.a.y < 0)
            {
                line.a.y = y0;
                line.a.x = line.b.x + vec.x * (y0 - line.b.y) / vec.y;

                return;
            }

            if (line.b.y < 0)
            {
                line.b.y = y0;
                line.b.x = line.a.x + vec.x * (y0 - line.a.y) / vec.y;
            }
        }

        public static class Matrix
        {
            public static float Det(QPoint p1, QPoint p2)
            {
                return p1.x * p2.y + p2.x * p1.y;
            }
        }
    }
}
