using DART.Dartboard.Models;
using MsgPack.Serialization;

namespace DART.Dartboard.Networking
{
    public class MsgpackMessageFormatter : IMessageFormatter
    {
        public byte[] Format(Do message)
        {
            return SerializationContext.Default.GetSerializer<Do>().PackSingleObject(message);
        }
    }
}