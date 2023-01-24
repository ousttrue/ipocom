using System;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Btdt
    {
        public Box<Bnid> BoneId;
        public Box<Tran> Transformation;
    }
}
