using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace DART.Dartboard.Models.Configuration
{
    public class RobotConfiguration
    {
        public string Name { get; set; }

        public string ModelFile { get; set; }

        public List<MotorConfiguration> Motors { get; set; }

        public Matrix<double> MotorTransformMatrix { get; set; }

        public Vector<double> CenterOfMass { get; set; }

        public Dictionary<string, YawDefinition> MotorYawCalculation { get; set; }

        public Camera Camera { get; set; }

        public double Mass { get; set; }

        public RobotConfiguration()
        {
            MotorYawCalculation = new Dictionary<string, YawDefinition>();
            Motors = new List<MotorConfiguration>();
        }
    }

    public class Camera
    {
        public Vector<double> Position { get; set; }
        public Vector<double> LookAt { get; set; }
    }

    public class YawDefinition
    {
        public bool Clockwise { get; set; }

        public double Scale { get; set; }
    }
}