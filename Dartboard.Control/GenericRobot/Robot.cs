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
            var ret = new sbyte[NumberOfMotors];

            for (var i = 0; i < MotorVectors.Length; i++)
            {
                ret[i] = ToSbyte(directionVector.DotProduct(CorrectVector(MotorVectors[i])));
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

        public abstract Vector<double>[] MotorVectors { get; }

        public abstract int NumberOfMotors { get; }

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
