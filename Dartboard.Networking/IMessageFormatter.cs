using Dartboard.Networking.Message;

namespace Dartboard.Networking
{
    public interface IMessageFormatter
    {
        byte[] Format(DoRequestMessage msg);
        Heartbeat Format(string msg);
    }
}