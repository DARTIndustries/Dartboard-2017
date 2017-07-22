using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DART.Dartboard.HID
{
    public class JoystickState
    {
        public Stick Left { get; set; }
        public Stick Right { get; set; }
    }

    public struct Stick
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
