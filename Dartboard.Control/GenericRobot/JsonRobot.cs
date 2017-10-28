using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Simulator.Serialization;
using Vector3D;

namespace DART.Dartboard.Control.GenericRobot
{
    public class JsonRobot : Robot
    {
        private RobotConfig _config;

        public JsonRobot(string json)
        {
            _config = JsonConvert.DeserializeObject<RobotConfig>(json);
        }

        public override Vector[] GetMotorVectors => _config.Motors.Select(x => new Vector(x.Vector.X, x.Vector.Y, x.Vector.Z)).ToArray();
        public override int NumberOfMotors => _config.Motors.Count();
    }
}
