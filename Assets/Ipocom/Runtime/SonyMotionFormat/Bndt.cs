using System;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Bndt
    {
        public Bnid BoneId;
        public Pbid ParentBoneId;
        public Tran Transformation;

        public static Bndt FromBox(Box bndt)
        {
            if (bndt.Type != BoxTypes.Bndt)
            {
                throw new ArgumentException("not bndt");
            }
            var it = Parser.ParseBoxes(bndt.Value).GetEnumerator();
            it.MoveNext();
            var bnid = it.Current;
            it.MoveNext();
            var pbid = it.Current;
            it.MoveNext();
            var tran = it.Current;
            return new Bndt
            {
                BoneId = Bnid.FromBox(bnid),
                ParentBoneId = Pbid.FromBox(pbid),
                Transformation = Tran.FromBox(tran),
            };
        }
    }
}