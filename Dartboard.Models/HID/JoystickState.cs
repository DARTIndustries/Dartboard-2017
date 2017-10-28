using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Utils;

namespace DART.Dartboard.Models.HID
{
    public class JoystickState
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double RotationZ { get; set; }

        public double Slider { get; set; }

        public HatPosition HatSwitch { get; set; }

        public bool[] Buttons { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            return sb.ToString();
        }

        public void RoundAll(int precision)
        {
            X = Math.Round(X, precision);
            Y = Math.Round(Y, precision);
            RotationZ = Math.Round(RotationZ, precision);
            Slider = Math.Round(Slider, precision);
        }

        public void ApplyDeadZone(double dead)
        {
            var res = Numerics.DeadZoneCalculation(X, Y, dead);
            X = res.x;
            Y = res.y;

            res = Numerics.DeadZoneCalculation(RotationZ, 0, dead);
            RotationZ = res.x;
        }
    }

    public enum HatPosition
    {
        NoState = -1,
        North = 0,
        NorthEast = 4500,
        East = 9000,
        SouthEast = 13500,
        South = 18000,
        SouthWest = 22500,
        West = 27000,
        NorthWest = 31500
    }
}
