using Microsoft.Xna.Framework;

namespace Dartboard.Networking.Message
{
    public class IndirectDoElement : DoElement
    {
        public MotorVector MotorVector { get; set; }
    }

    public class MotorVector
    {
        public Vector3 Velocity { get; set; }

        public Vector3 AngularVelocity { get; set; }

        public override string ToString()
        {
            return
                $"Velocity: ({Velocity.X:F2}, {Velocity.Y:F2}, {Velocity.Z:F2}) [{Velocity.Length():F3}]; " +
                $"Angular: ({AngularVelocity.X:F2}, {AngularVelocity.Y:F2}, {AngularVelocity.Z:F2}) [{AngularVelocity.Length():F3}]";
        }
    }


    public class DirectDoElement : DoElement
    {
        public sbyte[] Motors { get; set; }
    }

    public class DoElement
    {
        public ServoElement Camera { get; set; }
        public ServoElement Claw { get; set; }
        public Color Lights { get; set; }
        public BuzzerElement Buzzer { get; set; }
    }

    public class BuzzerElement
    {
        public bool State { get; set; }
    }

    public class ServoElement
    {
        public int[] Angles { get; set; }

        public int[] Velocity { get; set; }
    }

    public enum RequestFields
    {
        TEMP,
        GYRO,
        ACEL,
        COMP,
        PRES
    }
}
