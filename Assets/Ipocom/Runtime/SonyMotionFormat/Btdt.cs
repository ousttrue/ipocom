using System;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Btdt
    {
        public Bnid BoneId;
        public Tran Transformation;

        public static Btdt FromBox(Box btdt)
        {
            if (btdt.Type != BoxTypes.Btdt)
            {
                throw new ArgumentException("not btdt");
            }
            var it = Parser.ParseBoxes(btdt.Value).GetEnumerator();
            it.MoveNext();
            var bnid = it.Current;
            it.MoveNext();
            var tran = it.Current;
            return new Btdt
            {
                BoneId = Bnid.FromBox(bnid),
                Transformation = Tran.FromBox(tran),
            };
        }
    }
}
