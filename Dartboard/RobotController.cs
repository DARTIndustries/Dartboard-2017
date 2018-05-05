using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dartboard.Integration;
using Dartboard.Networking;
using Dartboard.Networking.Json;
using Dartboard.Networking.Message;
using Dartboard.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dartboard
{
    public class RobotController
    {
        private const float E = 0.000001f;

        private readonly AbstractRobot _robot;
        private readonly TimeSpan _messagePeriod;
        private readonly RobotState _state;
        private readonly DirectNetworkClient<Heartbeat, DoRequestMessage> _networkClient;

        private CancellationToken _token;

        public RobotController(AbstractRobot robot, TimeSpan period, bool sendTypes = false)
        {
            _robot = robot;
            _messagePeriod = period;
            _state = new RobotState {Throttle = 0.75f};

            var heartbeatFormatter = new JsonMessageFormatter<Heartbeat>();
            var msgFormatter = new JsonMessageFormatter<DoRequestMessage>()
            {
                IncludeTypes = sendTypes
            };

            _networkClient = new DirectNetworkClient<Heartbeat, DoRequestMessage>(robot, heartbeatFormatter, msgFormatter);

            _networkClient.ReductionFunc = MessageCompare;

        }

        public void Start(CancellationToken token)
        {
            _token = token;

            _networkClient.Start(token);

            var t = new Thread(MainControl);
            t.Start();
        }

        public void MainControl()
        {
            while (!_token.IsCancellationRequested)
            {
                EnsureTime(_messagePeriod, () =>
                {
                    var pilot = GamePad.GetState(PlayerIndex.One);
                    var captain = GamePad.GetState(PlayerIndex.Two);

                    var msg = new DoRequestMessage(TimeSpan.FromSeconds(2));

                    // Pilot Calculations

                    var z = (pilot.IsButtonDown(Buttons.LeftShoulder) ? -1 : 0) + (pilot.IsButtonDown(Buttons.RightShoulder) ? 1 : 0);
                    var movementVector = new Vector3(pilot.ThumbSticks.Left, z);
                    movementVector *= _state.Throttle;

                    /*
                     * Pitch    rX
                     * Roll     rY
                     * Yaw      rZ
                     */

                    var rz = -pilot.Triggers.Left + pilot.Triggers.Right;
                    var headingVector = new Vector3(pilot.ThumbSticks.Right.Y, -pilot.ThumbSticks.Right.X, rz);
                    headingVector *= _state.Throttle;

                    msg.Do = new IndirectDoElement()
                    {
                        MotorVector = new MotorVector()
                        {
                            AngularVelocity = headingVector,
                            Velocity = movementVector
                        },
                    };

                    if (pilot.IsButtonDown(Buttons.Y))
                        _state.Throttle = 1;

                    if (pilot.IsButtonDown(Buttons.X))
                        _state.Throttle = 0;

                    if (pilot.IsButtonDown(Buttons.B))
                    {
                        msg.Do = new DirectDoElement()
                        {
                            Motors = new sbyte[_robot.Motors.Count]
                        };
                    }

                    // Captain Calculations

                    var cameraVector = captain.ThumbSticks.Left;
                    bool sendCam = false;
                    if (cameraVector.Length() > 0)
                    {
                        sendCam = true;
                        _state.CameraX += cameraVector.X * _robot.CameraDelta;
                        _state.CameraY += cameraVector.Y * _robot.CameraDelta;
                    }

                    if (captain.IsButtonDown(Buttons.LeftStick))
                    {
                        sendCam = true;
                        _state.CameraX = (_robot.CameraHomeX - 90) / 180.0f;
                        _state.CameraY = (_robot.CameraHomeY - 90) / 180.0f;
                    }

                    if (sendCam)
                    {
                        msg.Do.Camera = new CameraElement()
                        {
                            Angles = new[] { _state.CameraX.ToInt(90) + 90, _state.CameraY.ToInt(90) + 90 }
                        };
                    }

                    if (captain.IsButtonDown(Buttons.A))
                    {
                        msg.Do.Buzzer = new BuzzerElement() { State = true };
                    }
                    if (captain.IsButtonDown(Buttons.B))
                    {
                        msg.Do.Buzzer = new BuzzerElement() { State = false };
                    }

                    var clawVelocity = -captain.Triggers.Left + captain.Triggers.Right;
                    if (Math.Abs(clawVelocity) > E)
                    {
                        _state.Claw += clawVelocity * _robot.ClawDelta;
                        msg.Do.Claw = new ClawElement()
                        {
                            Angle = _state.Claw.ToInt(90) + 90
                        };
                    }

                    // Joint Calculations

                    if (pilot.IsButtonDown(Buttons.DPadDown) || captain.IsButtonDown(Buttons.DPadDown))
                    {
                        _state.Throttle -= _robot.ThrottleDelta;
                        if (_state.Throttle < 0)
                            _state.Throttle = 0;
                    }

                    if (pilot.IsButtonDown(Buttons.DPadUp) || captain.IsButtonDown(Buttons.DPadUp))
                    {
                        _state.Throttle += _robot.ThrottleDelta;
                        if (_state.Throttle > 1)
                            _state.Throttle = 1;
                    }



                    _networkClient.Outbox.Enqueue(msg);
                });
            }
        }

        private void EnsureTime(TimeSpan minLength, Action action)
        {
            var start = DateTime.Now;
            action();
            var end = DateTime.Now;

            var length = end - start;

            if (length >= minLength)
                return;

            var wait = (int) (minLength - length).TotalMilliseconds;
            Thread.Sleep(wait);
        }

        // Returns true if left is equal to right
        private static bool MessageCompare(DoRequestMessage left, DoRequestMessage right)
        {
            if (left == null && right == null)
                return true;

            if (left == null || right == null)
                return false;

            if (left.Do.GetType() != right.Do.GetType())
                return false;

            if (left.Do is DirectDoElement)
            {
                var l = (DirectDoElement)left.Do;
                var r = (DirectDoElement)right.Do;

                if (!Utility.SequenceCompare(l.Motors, r.Motors))
                    return false;
            }
            else if (left.Do is IndirectDoElement)
            {
                var l = (IndirectDoElement)left.Do;
                var r = (IndirectDoElement)right.Do;

                if (l.MotorVector.Velocity != r.MotorVector.Velocity)
                    return false;

                if (l.MotorVector.AngularVelocity != r.MotorVector.AngularVelocity)
                    return false;
            }

            if (left.Do.Lights != right.Do.Lights)
                return false;

            if (left.Do.Camera == null ^ right.Do.Camera == null)
                return false;

            if (left.Do.Camera != null && right.Do.Camera != null && !Utility.SequenceCompare(left.Do.Camera.Angles, right.Do.Camera.Angles))
                return false;

            if (left.Do.Claw?.Angle != right.Do.Claw?.Angle)
                return false;

            return true;
        }


    }
}
