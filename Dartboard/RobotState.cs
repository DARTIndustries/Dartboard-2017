using Dartboard.Utils;

namespace Dartboard
{
    public class RobotState
    {
        private float _cameraX;
        private float _cameraY;
        private float _claw;
        private float _throttle;

        public float CameraX
        {
            get => _cameraX;
            set => _cameraX = value.Clamp();
        }

        public float CameraY
        {
            get => _cameraY;
            set => _cameraY = value.Clamp();
        }

        public float Claw
        {
            get => _claw;
            set => _claw = value.Clamp();
        }

        public float Throttle
        {
            get => _throttle;
            set => _throttle = value.Clamp();
        }
    }
}