using System;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Bndt
    {
        public Box<Bnid> BoneId;
        public Box<Pbid> ParentBoneId;
        public Box<Tran> Transformation;

        public override bool Equals(object obj)
        {
            if (obj is Bndt rhs)
            {
                if (BoneId.Value.BoneId != rhs.BoneId.Value.BoneId)
                {
                    return false;
                }
                if (ParentBoneId.Value.ParentBoneId != rhs.ParentBoneId.Value.ParentBoneId)
                {
                    return false;
                }
                if (!Transformation.Value.Equals(rhs.Transformation.Value))
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hashCode = -1223921908;
            hashCode = hashCode * -1521134295 + BoneId.GetHashCode();
            hashCode = hashCode * -1521134295 + ParentBoneId.GetHashCode();
            hashCode = hashCode * -1521134295 + Transformation.GetHashCode();
            return hashCode;
        }
    }
}