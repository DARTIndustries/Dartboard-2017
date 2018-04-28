using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dartboard.Integration
{
    public abstract class AbstractRobot
    {
        public List<Motor> Motors { get; protected set; }

        public ModelInfo ModelInfo { get; protected set; }

        public List<Servo> Servos { get; protected set; }

        public Uri DeviceEndpoint { get; protected set; }

        public List<Uri> CameraAddresses { get; protected set; }

        protected AbstractRobot(Uri endpoint)
        {
            Motors = new List<Motor>();
            ModelInfo = new ModelInfo();
            Servos = new List<Servo>();
            DeviceEndpoint = endpoint;
            CameraAddresses = new List<Uri>();
        }

        public virtual Color GetColor()
        {
            return Color.CornflowerBlue;
        }
    }

    public class ModelInfo
    {
        public Model Model { get; set; }

        public Matrix BaseTransform { get; set; }
    }

    public class Servo
    {

    }

    public class Motor
    {
        public string Name { get; set; }

        public Vector3 ThrustVector { get; set; }

        public Vector3 Location { get; set; }

        public double MaximumThrust { get; set; }
    }
}
