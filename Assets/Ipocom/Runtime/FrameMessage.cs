using System;

namespace Ipocom
{
    [Serializable]
    public struct FrameMessage
    {
        public SonyMotionFormat.FrameHeader header;
        public SonyMotionFormat.FrameBone[] bones;
    }
}
