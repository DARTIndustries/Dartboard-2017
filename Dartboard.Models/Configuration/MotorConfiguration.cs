using MathNet.Numerics.LinearAlgebra;

namespace DART.Dartboard.Models.Configuration
{
    public struct MotorConfiguration
    {
        public string Key { get; set; }

        public Vector<double> Vector { get; set; }

        public Vector<double> Location { get; set; }
    }
}