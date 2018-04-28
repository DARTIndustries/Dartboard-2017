﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartboard.Utils
{
    public static class Utility
    {
        public static sbyte ToSByte(this double d)
        {
            if (d >= 1)
                return sbyte.MaxValue;

            if (d <= -1)
                return sbyte.MinValue;

            return (sbyte)(d * sbyte.MaxValue);
        }

        public static sbyte ToSByte(this float d)
        {
            if (d >= 1)
                return sbyte.MaxValue;

            if (d <= -1)
                return sbyte.MinValue;

            return (sbyte)(d * sbyte.MaxValue);
        }

        public static byte ToByte(this float d, byte zero, byte one)
        {
            if (d >= 1)
                return one;

            if (d <= -1)
                return zero;

            return 0;
            //return (sbyte)()
        }

        public static double ToDouble(this sbyte b)
        {
            if (b == -1)
                return sbyte.MinValue;

            return b / 127.0;
        }

        public static float ToFloat(this sbyte b)
        {
            if (b == -1)
                return sbyte.MinValue;

            return b / 127.0f;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}