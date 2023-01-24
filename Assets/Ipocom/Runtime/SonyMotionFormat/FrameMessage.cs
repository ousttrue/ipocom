using System;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct FrameMessage
    {
        public Head head;
        public Sndf sndf;
        public Fram fram;
    }
}
