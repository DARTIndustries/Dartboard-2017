using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Dartboard.Utils
{
    public static class Numerics
    {
        /// <summary>
        /// Performs math on the value of a joystick (when the raw value is signed, to turn into a useable value
        /// </summary>
        /// <param name="value">value to calculate</param>
        /// <param name="min">Raw input minimum; default = 0</param>
        /// <param name="max">Raw input maximum; default = 65535</param>
        /// <param name="dead">Percentage dead zone; default 0.00</param>
        /// <returns>A double between -1 and 1</returns>
        public static double SignedJoystickFunction(int value, int min = -32768, int max = 32767, double dead = 0.00d)
        {
            var v = value / ((max - min) / 2.0);
            if (v < dead && v > -1 * dead)
                return 0;
            return v;
        }

        /// <summary>
        /// Performs math on the value of a joystick, to turn into a useable value
        /// </summary>
        /// <param name="value">value to calculate</param>
        /// <param name="min">Raw input minimum; default = 0</param>
        /// <param name="max">Raw input maximum; default = 65535</param>
        /// <param name="dead">Percentage dead zone; default 0.10</param>
        /// <returns>A double between -1 and 1</returns>
        public static double JoystickFunction(int value, int min = 0, int max = 65535, double dead = 0.00d)
        {
            var half = (max - min) / 2.0;
            var v = (value - Math.Abs(half)) / half;
            if (v < dead && v > -1 * dead)
                return 0;
            return v;
        }

        /// <summary>
        /// Performs calculations on the value of a trigger
        /// </summary>
        /// <param name="value">Value of the trigger</param>
        /// <param name="min">Minimum raw value; default = 0</param>
        /// <param name="max">Maximum raw value; default = 65535</param>
        /// <returns>A double between 0 and 1</returns>
        public static double TriggerFunction(int value, int min = 0, int max = 65535)
        {
            bool inv = false;
            if (min > max)
            {
                var swap = min;
                min = max;
                max = swap;
                inv = true;
            }
            var range = max - min;
            if (inv)
                return 1 - ((double) value / range);
            return (double)value / range;
        }

        /// <summary>
        /// Performs a dead zone calculation, for joysticks. Meant to eliminate small values,
        /// while still allowing precise movements.
        /// </summary>
        /// <param name="x">X direction value</param>
        /// <param name="y">Y direction value</param>
        /// <param name="dead">Deadzone percentage</param>
        /// <returns>A tuple (x, y) with the dead zone padding</returns>
        public static (double x, double y) DeadZoneCalculation(double x, double y, double dead)
        {
            var stickInput = Vector<double>.Build.DenseOfArray(new [] {x, y});
            var magnitude = Pythagorean(stickInput[0], stickInput[1]);

            if (magnitude < dead)
                return (0, 0);

            var outVec = stickInput.Normalize(1) * ((magnitude - dead) / (1 - dead));
            return (outVec[0], outVec[1]);
        }


        /// <summary>
        /// Calculates the hypotenuse of a triangle
        /// </summary>
        /// <param name="a">Aide A</param>
        /// <param name="b">Side B</param>
        /// <returns>sqrt(A^2 + B^2)</returns>
        public static double Pythagorean(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
    }
}
