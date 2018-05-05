using Microsoft.Xna.Framework;

namespace Dartboard.Integration
{
    public class Motor
    {
        public string Name { get; set; }

        public Vector3 ThrustVector { get; set; }

        public Vector3 Location { get; set; }

        public double MaximumThrust { get; set; }
    }
}