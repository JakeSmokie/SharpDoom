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

            res.a.x = line.a.x * Sin(angle) - line.a.y * Cos(angle);
            res.b.x = line.b.x * Sin(angle) - line.b.y * Cos(angle);

            res.a.y = line.a.x * Cos(angle) + line.a.y * Sin(angle);
            res.b.y = line.b.x * Cos(angle) + line.b.y * Sin(angle);

            return res;
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
