using System;
using System.Text;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]
    public struct Head
    {
        public ArraySegment<byte> FileType;
        public byte Version;

        public static readonly byte[] FILE_TYPE = Encoding.ASCII.GetBytes("sony motion format");
        public static readonly byte VERSION = 1;

        // head
        //   ftyp
        //   vrsn
        public static Head FromBox(Box head)
        {
            if (head.Type != BoxTypes.Head)
            {
                throw new ArgumentException("not head");
            }
            var it = Parser.ParseBoxes(head.Value).GetEnumerator();
            it.MoveNext();
            var ftyp = it.Current;
            it.MoveNext();
            var vrsn = it.Current;
            return new Head
            {
                FileType = ftyp.Value,
                Version = vrsn.Value.Array[vrsn.Value.Offset],
            };
        }
    }
}
