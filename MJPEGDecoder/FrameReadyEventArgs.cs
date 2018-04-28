using System;

namespace MjpegDecoder
{
    public class FrameReadyEventArgs : EventArgs
    {
        public byte[] FrameBuffer;
    }
}