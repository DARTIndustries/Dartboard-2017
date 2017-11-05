using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;
using DART.Dartboard.Models.Configuration;
using DART.Dartboard.Utils;
using HelixToolkit.Wpf;
using Ionic.Zip;
using Newtonsoft.Json;
using Simulator.Util;
using Camera = DART.Dartboard.Models.Configuration.Camera;

namespace Simulator.Control3D
{
    public class Robot
    {
        public Model3DGroup Model { get; set; }

        public string Name { get; set; }

        public MotorContoller MotorContoller { get; set; }

        public Point3D CenterOfMass { get; set; }

        public double Mass { get; set; }

        public Camera Camera { get; set; }

        public Dictionary<string, YawDefinition> MotorYawCalculation { get; set; }

        public static Robot LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Unable to find Robot", path);

            RobotConfiguration cfg;
            Model3DGroup model;
            var importer = new ModelImporter();

            if (path.EndsWith(".zip"))
            {
                string temp;
                using (var zip = ZipFile.Read(path))
                using (var reader = new StreamReader(zip["robot.json"].OpenReader()))
                {
                    cfg = JsonConvert.DeserializeObject<RobotConfiguration>(reader.ReadToEnd(), new VectorConverter(), new MatrixConverter());

                    temp = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()));
                    zip[cfg.ModelFile].Extract(temp);
                }

                model = importer.Load(Path.Combine(temp, cfg.ModelFile));

            }
            else if (path.EndsWith(".json"))
            {
                cfg = JsonConvert.DeserializeObject<RobotConfiguration>(File.ReadAllText(path), new VectorConverter(), new MatrixConverter());

                model = importer.Load(Path.Combine(Path.GetDirectoryName(path), cfg.ModelFile));
            }
            else
                throw new FormatException();

            var controller = new MotorContoller();

            foreach (var motor in cfg.Motors)
            {
                controller.Register(motor.Key, new Motor(motor.Vector.ToVector3D())
                {
                    ThrustLocation = motor.Location.ToPoint3D()
                });
            }

            return new Robot()
            {
                Model = model,
                Name = cfg.Name,
                MotorContoller = controller,
                CenterOfMass = cfg.CenterOfMass.ToPoint3D(),
                Mass = cfg.Mass,
                Camera = cfg.Camera,
                MotorYawCalculation = cfg.MotorYawCalculation
            };
        }

    }
}
