using System;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Bnid
    {
        public UInt16 BoneId;
        public static Bnid FromBox(Box bnid)
        {
            if (bnid.Type != BoxTypes.Bnid)
            {
                throw new ArgumentException("not bnid");
            }
            return new Bnid
            {
                BoneId = BitConverter.ToUInt16(bnid.Value.Array, bnid.Value.Offset),
            };
        }
    }
}
