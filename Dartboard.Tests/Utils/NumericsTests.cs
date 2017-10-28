using DART.Dartboard.Utils;
using NUnit.Framework;

namespace DART.Dartboard.Tests.Utils
{
    [TestFixture]
    public class NumericsTests
    {
        [Test]
        public void TestJoystickFunction()
        {
            Assert.AreEqual(1, Numerics.JoystickFunction(65535));
            Assert.AreEqual(-1, Numerics.JoystickFunction(0));
            Assert.AreEqual(0, Numerics.JoystickFunction(32768));
            Assert.AreEqual(0, Numerics.JoystickFunction(32700));
            Assert.AreEqual(0, Numerics.JoystickFunction(32800));

            Assert.AreEqual(-1, Numerics.JoystickFunction(65535, 65535, 0));
            Assert.AreEqual(1, Numerics.JoystickFunction(0, 65535, 0));
            Assert.AreEqual(0, Numerics.JoystickFunction(32768, 65535, 0));
            Assert.AreEqual(0, Numerics.JoystickFunction(32700, 65535, 0));
            Assert.AreEqual(0, Numerics.JoystickFunction(32800, 65535, 0));
        }

        [Test]
        public void TestSignedJoystickFunction()
        {
            Assert.AreEqual(1, Numerics.SignedJoystickFunction(32767), 0.001);
            Assert.AreEqual(-1, Numerics.SignedJoystickFunction(-32768), 0.001);

            Assert.AreEqual(0, Numerics.SignedJoystickFunction(0), 0.001);
            Assert.AreEqual(0, Numerics.SignedJoystickFunction(1000), 0.001);
            Assert.AreEqual(0, Numerics.SignedJoystickFunction(-1000), 0.001);

            Assert.AreEqual(-1, Numerics.SignedJoystickFunction(32767, 32767, -32768), 0.001);
            Assert.AreEqual(1, Numerics.SignedJoystickFunction(-32768, 32767, -32768), 0.001);

            Assert.AreEqual(0, Numerics.SignedJoystickFunction(0, 32767, -32768), 0.001);
            Assert.AreEqual(0, Numerics.SignedJoystickFunction(1000, 32767, -32768), 0.001);
            Assert.AreEqual(0, Numerics.SignedJoystickFunction(-1000, 32767, -32768), 0.001);
        }

        [Test]
        public void TestTriggerFunction()
        {
            Assert.AreEqual(1, Numerics.TriggerFunction(65535));
            Assert.AreEqual(0, Numerics.TriggerFunction(0));
            Assert.AreEqual(0.5, Numerics.TriggerFunction(32768), 0.001);

            Assert.AreEqual(0, Numerics.TriggerFunction(65535, 65535, 0));
            Assert.AreEqual(1, Numerics.TriggerFunction(0, 65535, 0));
            Assert.AreEqual(0.5, Numerics.TriggerFunction(32768, 65535, 0), 0.001);
        }
    }
}
