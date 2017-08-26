using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartboard.Utils
{
    public static class Extensions
    {
        public static bool In<T>(this T t, IEnumerable<T> choices)
        {
            foreach (var choice in choices)
            {
                if (EqualityComparer<T>.Default.Equals(t, choice))
                    return true;
            }
            return false;
        }
    }
}
