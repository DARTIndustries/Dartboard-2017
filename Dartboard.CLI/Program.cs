using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dart.Robots.Adam;
using Dartboard.Integration;
using Dartboard.Networking;
using Dartboard.Networking.Json;
using Dartboard.Networking.Message;
using Dartboard.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NLog;

namespace Dartboard.CLI
{
    class Program
    {
        private const float ThrottleDelta = 0.05f;
        private const float ε = 0.000001f;

        static void Main(string[] args)
        {
            LogManager.EnableLogging();
            ILogger log = LogManager.GetCurrentClassLogger();
            log.Info("Starting");

            AbstractRobot robot = new Adam2018() { ColorWheel = true };
            
            var heartbeatFormatter = new JsonMessageFormatter<Heartbeat>();
            var msgFormatter = new JsonMessageFormatter<DoRequestMessage>();
            //msgFormatter.IncludeTypes = true;
            var anc = new DirectNetworkClient<Heartbeat, DoRequestMessage>(robot, heartbeatFormatter, msgFormatter);
            anc.ReductionFunc = MessageCompare;
            var tokenSource = new CancellationTokenSource();
            anc.Start(tokenSource.Token);

            float throttle = 0.75f;

            while (true)
            {
                var pilot = GamePad.GetState(PlayerIndex.One);
                var captain = GamePad.GetState(PlayerIndex.Two);

                var msg = new DoRequestMessage(TimeSpan.FromSeconds(2));

                // Pilot Calculations

                var z = (pilot.IsButtonDown(Buttons.LeftShoulder) ? -1:0) + (pilot.IsButtonDown(Buttons.RightShoulder) ? 1 : 0);
                var movementVector = new Vector3(pilot.ThumbSticks.Left, z);
                movementVector *= throttle;

                /*
                 * Pitch    rX
                 * Roll     rY
                 * Yaw      rZ
                 */

                var rz = -pilot.Triggers.Left + pilot.Triggers.Right;
                var headingVector = new Vector3(pilot.ThumbSticks.Right.Y, -pilot.ThumbSticks.Right.X, rz);
                headingVector *= throttle;

                msg.Do = new IndirectDoElement()
                {
                    MotorVector = new MotorVector()
                    {
                        AngularVelocity = headingVector,
                        Velocity = movementVector
                    },
                };

                if (pilot.IsButtonDown(Buttons.DPadDown))
                {
                    throttle -= ThrottleDelta;
                    if (throttle < 0)
                        throttle = 0;
                }

                if (pilot.IsButtonDown(Buttons.DPadUp))
                {
                    throttle += ThrottleDelta;
                    if (throttle > 1)
                        throttle = 1;
                }

                if (pilot.IsButtonDown(Buttons.Y))
                    throttle = 1;

                if (pilot.IsButtonDown(Buttons.X))
                    throttle = 0;

                if (pilot.IsButtonDown(Buttons.B))
                {
                    msg.Do = new DirectDoElement()
                    {
                        Motors = new sbyte[robot.Motors.Count]
                    };
                }

                // Captain Calculations

                var cameraVector = captain.ThumbSticks.Left;
                if (cameraVector.Length() > 0)
                {
                    msg.Do.Camera = new ServoElement()
                    {
                        Velocity = new[] { cameraVector.X.ToInt(180), cameraVector.Y.ToInt(180) }
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
                if (Math.Abs(clawVelocity) > ε)
                {
                    msg.Do.Claw = new ServoElement()
                    {
                        Velocity = new[] {clawVelocity.ToInt(180)}
                    };
                }


                anc.Outbox.Enqueue(msg);
                Thread.Sleep(50);
            }

            Console.ReadLine();

        }

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

                if (!SequenceCompare(l.Motors, r.Motors))
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

            //if (left.Do.Camera == null || right.Do.Camera == null)
            //    return false;

            //if (!SequenceCompare(left.Do.Camera.Velocity, right.Do.Camera.Velocity))
            //    return false;

            //if (!SequenceCompare(left.Do.Camera.Angles, right.Do.Camera.Angles))
            //    return false;

            //if (!SequenceCompare(left.Do.Claw.Velocity, right.Do.Claw.Velocity))
            //    return false;

            //if (!SequenceCompare(left.Do.Claw.Angles, right.Do.Claw.Angles))
            //    return false;

            return true;
        }

        private static bool SequenceCompare<T>(T[] left, T[] right) where T: IEquatable<T>
        {
            if (left.Length != right.Length)
                return false;

            for (int i = 0; i < left.Length; i++)
            {
                if (!left[i].Equals(right[i]))
                    return false;
            }

            return true;
        }

        public static bool BothNullOrBothNot(object left, object right)
        {
            return (left == null) ^ (right == null);
        }
    }
}
