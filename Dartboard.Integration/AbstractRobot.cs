using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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

        /// <summary>
        /// Camera home X position, from 0 to 180
        /// </summary>
        public virtual int CameraHomeX => 90;

        /// <summary>
        /// Camera home X position, from 0 to 180
        /// </summary>
        public virtual int CameraHomeY => 90;

        public virtual float ThrottleDelta => 0.05f;

        public virtual float CameraDelta => 0.05f;

        public virtual float ClawDelta => 0.05f;
    }
}
