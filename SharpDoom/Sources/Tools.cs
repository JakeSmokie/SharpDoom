﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace SharpDoom
{
    public class Tools
    {
        #region Basic Frame Counter
        private static int lastTick;
        private static int lastFrameRate;
        private static int frameRate;

        public static int CalculateFrameRate()
        {
            if (System.Environment.TickCount - lastTick >= 1000)
            {
                lastFrameRate = frameRate;
                frameRate = 0;
                lastTick = System.Environment.TickCount;
            }
            frameRate++;
            return lastFrameRate;
        }
        #endregion

        public static float StrToFloat(string str)
        {
            return Convert.ToSingle(str, numberFormat);
        }

        public static NumberFormatInfo numberFormat = new NumberFormatInfo()
        {
            NumberDecimalSeparator = "."
        };
    }
}
