using Dartboard.Networking.Message;

namespace Dartboard.Networking
{
    public interface IMessageFormatter<T>
    {
        byte[] Format(T msg);
        T Format(string msg);
    }
}