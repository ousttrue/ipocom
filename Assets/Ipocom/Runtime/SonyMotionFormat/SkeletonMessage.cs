using System;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct SkeletonMessage
    {
        public Head head;
        public Sndf sndf;
        public Skdf skdf;
    }
}
