using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartboard.Utils
{
    public static class Utility
    {
        public static sbyte ToByte(this double d)
        {
            if (d >= 1)
                return sbyte.MaxValue;

            if (d <= -1)
                return sbyte.MinValue;

            return (sbyte)(d * sbyte.MaxValue);
        }

        public static sbyte ToByte(this float d)
        {
            if (d >= 1)
                return sbyte.MaxValue;

            if (d <= -1)
                return sbyte.MinValue;

            return (sbyte)(d * sbyte.MaxValue);
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
    }
}
