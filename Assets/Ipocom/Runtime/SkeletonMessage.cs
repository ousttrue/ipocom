using System;

namespace Ipocom
{
    [Serializable]
    public struct SkeletonMessage
    {
        public SonyMotionFormat.SkeletonHeader header;
        public SonyMotionFormat.SkeletonBone[] bones;
    }
}
