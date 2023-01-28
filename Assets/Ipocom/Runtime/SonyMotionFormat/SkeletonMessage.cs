using System;
using System.Collections.Generic;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct SkeletonMessage
    {
        public Head head;
        public Sndf sndf;
        public Skdf skdf;

        public override bool Equals(object obj)
        {
            if (obj is SkeletonMessage rhs)
            {
                // if (!head.Equals(rhs.head))
                // {
                //     return false;
                // }
                // if (!sndf.Equals(rhs.sndf))
                // {
                //     return false;
                // }
                if (skdf != null)
                {
                    if (!skdf.Equals(rhs.skdf))
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return rhs.skdf == null;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = -1328486026;
            hashCode = hashCode * -1521134295 + head.GetHashCode();
            hashCode = hashCode * -1521134295 + sndf.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Skdf>.Default.GetHashCode(skdf);
            return hashCode;
        }
    }
}
