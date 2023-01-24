using System;

namespace Ipocom
{
    [Serializable]
    public struct SkeletonMessage
    {
        public SonyMotionFormat.Head head;
        public SonyMotionFormat.Sndf sndf;
        public SonyMotionFormat.Skdf skdf;
    }
}
