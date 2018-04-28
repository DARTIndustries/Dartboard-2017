namespace Dartboard.Networking.Message
{
    public class DoRequestMessage
    {
        public DoRequestMessage()
        {
            Do = new DoElement();

            Request = new RequestElement();

            Config = new ConfigElement();
        }

        public DoElement Do { get; set; }

        public RequestElement Request { get; set; }

        public ConfigElement Config { get; set; }
    }
}