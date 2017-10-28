using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DART.Dartboard.Models.Configuration;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;

namespace DART.Dartboard.Control.GenericRobot
{
    public class JsonRobot : Robot
    {
        private RobotConfiguration _config;

        public JsonRobot(string json)
        {
            _config = JsonConvert.DeserializeObject<RobotConfiguration>(json);
        }

        public JsonRobot(RobotConfiguration config)
        {
            _config = config;
        }

        public override Vector<double>[] MotorVectors => 
            _config.Motors.Select(x => x.Vector).ToArray();

        public override int NumberOfMotors => _config.Motors.Count();

        public override Vector<double> CorrectVector(Vector<double> v)
        {
            if (_config.MotorTransformMatrix != null)
                return _config.MotorTransformMatrix.Multiply(v);
            return v;
        }
    }
}
