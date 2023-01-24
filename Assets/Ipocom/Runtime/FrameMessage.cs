using System;

namespace Ipocom
{
    [Serializable]
    public struct FrameMessage
    {
        public SonyMotionFormat.Head head;
        public SonyMotionFormat.Sndf sndf;
        public SonyMotionFormat.Fram fram;
    }
}
