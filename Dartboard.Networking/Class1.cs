using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dartboard.Integration;
using Dartboard.Utils.Validation;
using Microsoft.Xna.Framework;

namespace Dartboard.Networking
{
    public class DoRequestMessage : BaseValidator
    {
        public DoRequestMessage()
        {
            Do = new DoElement();

            Request = new RequestElement();

            Config = new ConfigElement();
        }

        public DoElement Do { get; set; }

        public RequestElement Request { get; set; }

        public ConfigElement Config { get; set; }
    }

    public class IndirectDoElement : DoElement
    {
        public Vector3 MovementVector { get; set; }

        public Vector3 Heading { get; set; }

        public override bool Validate(AbstractRobot robot)
        {
            // Heading must not be zero, but movement vector can
            if (Heading == Vector3.Zero)
                return false;

            return base.Validate(robot);
        }
    }

    public class DirectDoElement : DoElement
    {
        public sbyte[] Motors { get; set; }

        public override bool Validate(AbstractRobot robot)
        {
            if (Motors.Length != robot.Motors.Count)
                return false;

            return base.Validate(robot);
        }
    }

    public class DoElement : IValidatable
    {
        public virtual bool Validate(AbstractRobot robot)
        {
            return true;
        }
    }

    public class RequestElement
    {
        
    }

    public class ConfigElement
    {
        
    }
}
