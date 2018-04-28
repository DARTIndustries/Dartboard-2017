using Microsoft.Xna.Framework;

namespace Dartboard.Networking.Message
{
    public class IndirectDoElement : DoElement
    {
        public MotorVector MotorVector { get; set; }

        public Vector3 Heading { get; set; }
    }

    public class MotorVector
    {
        public Vector3 Velocity { get; set; }

        public Vector3 AngularVelocity { get; set; }
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

    }

    public class ServoElement
    {
        public sbyte[] Angles { get; set; }

        public sbyte[] Velocity { get; set; }
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
