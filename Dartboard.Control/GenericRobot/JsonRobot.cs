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

        public override Dictionary<string, Vector<double>> MotorVectors => 
            _config.Motors.ToDictionary(x => x.Key, x => x.Vector);

        public override IEnumerable<string> MotorKeys => _config.Motors.Select(x => x.Key);

        public override Vector<double> CorrectVector(Vector<double> v)
        {
            if (_config.MotorTransformMatrix != null)
                return _config.MotorTransformMatrix.Multiply(v);
            return v;
        }

        public override double ApplyYaw(string key, double current, double yaw)
        {
            if (!_config.MotorYawCalculation.ContainsKey(key))
                return current;

            var calc = _config.MotorYawCalculation[key];

            if (calc.Clockwise)
                return current + (yaw * calc.Scale);
            else
                return current - (yaw * calc.Scale);
        }
    }
}
