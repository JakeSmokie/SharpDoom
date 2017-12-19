﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDoom
{
    public class Player
    {
        public Player()
        {
            pos.x = -50 * (float)Math.Sqrt(3);
            pos.y = 0.0f;

            viewAngle = 0.0f;
        }

        public QPoint pos;
        public float viewAngle;

        public const float width = 16.0f;
        public const float speed = 4.0f;

        public const float FOV = (float)Math.PI / 3;
    }
}
