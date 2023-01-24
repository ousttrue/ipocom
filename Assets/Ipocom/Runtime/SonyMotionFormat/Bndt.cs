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
    }
}