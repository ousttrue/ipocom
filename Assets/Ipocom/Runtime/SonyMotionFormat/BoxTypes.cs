using System;
using System.Collections.Generic;

namespace Ipocom.SonyMotionFormat
{
    public enum BoxTypes
    {
        Head,
        Sndf,
        Skdf,
        Fram,
        Ftyp,
        Vrsn,
        Ipad,
        Rcvp,
        Fnum,
        Time,
        Btrs,
        Bons,
        Btdt,
        Bndt,
        Bnid,
        Tran,
        Pbid,
    }

    public static class BoxTypesExtensions
    {
        public static bool IsNested(this BoxTypes t)
        {
            switch (t)
            {
                case BoxTypes.Head:
                case BoxTypes.Sndf:
                case BoxTypes.Skdf:
                case BoxTypes.Fram:
                case BoxTypes.Bons:
                case BoxTypes.Btrs:
                case BoxTypes.Btdt:
                case BoxTypes.Bndt:
                    return true;
                default:
                    return false;
            }
        }
    }
}
