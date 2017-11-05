using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models.HID;
using MathNet.Numerics.LinearAlgebra;

namespace DART.Dartboard.Control.GenericRobot
{
    public abstract class Robot
    {
        public virtual sbyte[] CalculateMotorValues(Vector<double> directionVector, double yaw, GamepadState gamepad)
        {
            var ret = new sbyte[MotorKeys.Count()];
            int i = 0;
            foreach (var key in MotorKeys)
            {
                var motorValueNoYaw = directionVector.DotProduct(CorrectVector(MotorVectors[key]));

                var motorValue = ApplyYaw(key, motorValueNoYaw, yaw);

                ret[i++] = ToSbyte(motorValue);
            }

            return ret;
        }

        public virtual Vector<double> DirectionVector(Vector<double> dirVector)
        {
            return dirVector;
        }

        public virtual Vector<double> CorrectVector(Vector<double> v)
        {
            return v;
        }

        public virtual double ApplyYaw(string key, double current, double yaw)
        {
            return current;
        }

        public abstract Dictionary<string, Vector<double>> MotorVectors { get; }

        public abstract IEnumerable<string> MotorKeys { get; }

        protected sbyte ToSbyte(double d)
        {
            if (d > 1)
                d = 1;

            if (d < -1)
                d = -1;

            return (sbyte)(d * sbyte.MaxValue);
        }
    }


}
