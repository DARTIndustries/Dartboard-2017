﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Dartboard.Integration
{
    public abstract class AbstractRobot
    {
        public List<Motor> Motors { get; protected set; }

        public ModelInfo ModelInfo { get; protected set; }

        public List<Servo> Servos { get; protected set; }

        public Uri DeviceEndpoint { get; protected set; }

        public List<Uri> CameraAddresses { get; protected set; }

        public ColorMode ColorMode { get; set; }

        protected AbstractRobot(Uri endpoint)
        {
            Motors = new List<Motor>();
            ModelInfo = new ModelInfo();
            Servos = new List<Servo>();
            DeviceEndpoint = endpoint;
            CameraAddresses = new List<Uri>();
        }

        public virtual Color GetColor()
        {
            switch (ColorMode)
            {
                case ColorMode.Off:
                    return Color.Black;
                case ColorMode.SignalAffirmative:
                    return Color.Green;
                case ColorMode.SignalInconclusive:
                    return Color.Yellow;
                case ColorMode.SignalNegative:
                    return Color.Red;
                case ColorMode.Display:
                    return Color.CornflowerBlue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual float ThrottleDelta => 0.05f;

        public virtual float CameraDelta => 0.05f;

        public virtual float ClawDelta => 0.05f;

        public virtual CameraConfiguration CameraConfiguration =>
            new CameraConfiguration()
            {
                X = new CameraAxis()
                {
                    Min = 0,
                    Max = 180,
                    Home = 90
                },
                Y = new CameraAxis()
                {
                    Min = 0,
                    Max = 180,
                    Home = 90
                }
            };
    }

    public enum ColorMode
    {
        Off = 0,
        SignalAffirmative,
        SignalInconclusive,
        SignalNegative,
        Display = 100
    }

    public class CameraConfiguration
    {
        public CameraAxis X { get; set; }
        public CameraAxis Y { get; set; }
    }

    public class CameraAxis
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Home { get; set; }
    }
}
