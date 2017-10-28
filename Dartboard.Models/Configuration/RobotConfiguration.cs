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

        public double Mass { get; set; }
    }
}