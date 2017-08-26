using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dartboard.Utils;

namespace DART.Dartboard.Models.HID
{
    public class GamepadState
    {
        public GamepadButtonFlags Buttons;
        public double LeftTrigger;
        public double RightTrigger;
        public double LeftThumbX;
        public double LeftThumbY;
        public double RightThumbX;
        public double RightThumbY;

        public void ApplyDeadZone(double dead)
        {
            var left = Numerics.DeadZoneCalculation(LeftThumbX, LeftThumbY, dead);
            var right = Numerics.DeadZoneCalculation(RightThumbX, RightThumbY, dead);

            LeftThumbX = Math.Round(left.x, 3);
            LeftThumbY = left.y;

            RightThumbX = right.x;
            RightThumbY = right.y;
        }

        public void RoundAll(int precision)
        {
            LeftTrigger = Math.Round(LeftTrigger, precision);
            RightTrigger = Math.Round(RightTrigger, precision);
            LeftThumbX = Math.Round(LeftThumbX, precision);
            LeftThumbY = Math.Round(LeftThumbY, precision);
            RightThumbX = Math.Round(RightThumbX, precision);
            RightThumbY = Math.Round(RightThumbY, precision);
        }
    }

    [Flags]
    public enum GamepadButtonFlags : short
    {
        DPadUp = 1,
        DPadDown = 2,
        DPadLeft = 4,
        DPadRight = 8,
        Start = 16,
        Back = 32,
        LeftThumb = 64,
        RightThumb = 128,
        LeftShoulder = 256,
        RightShoulder = 512,
        A = 4096,
        B = 8192,
        X = 16384,
        Y = -32768,
        None = 0,
    }
}
