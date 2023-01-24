using System;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public struct Pbid
    {
        public UInt16 ParentBoneId;
        public static Pbid FromBox(Box pbid)
        {
            if (pbid.Type != BoxTypes.Pbid)
            {
                throw new ArgumentException("not pbid");
            }
            return new Pbid
            {
                ParentBoneId = BitConverter.ToUInt16(pbid.Value.Array, pbid.Value.Offset),
            };
        }
    }
}
