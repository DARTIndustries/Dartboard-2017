using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models.HID;
using Vector3D;

namespace DART.Dartboard.Control.GenericRobot
{
    public abstract class Robot
    {
        public virtual sbyte[] CalculateMotorValues(Vector directionVector, double yaw, GamepadState gamepad)
        {
            return new sbyte[]
            {
                ToSbyte(directionVector.X),
                ToSbyte(directionVector.X),
                ToSbyte(directionVector.Y),
                ToSbyte(directionVector.Y),
                ToSbyte(directionVector.Z),
                ToSbyte(directionVector.Z),
            };
        }

        public virtual Vector DirectionVector(Vector dirVector)
        {
            return dirVector;
        }

        public abstract Vector[] GetMotorVectors { get; }

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
