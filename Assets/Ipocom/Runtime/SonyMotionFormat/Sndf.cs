using System;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public struct Sndf
    {
        public ArraySegment<byte> IpAddress;
        public UInt16 ReceivePort;
        // sndf
        //   ipad
        //   rcvp
        public static Sndf FromBox(Box sndf)
        {
            if (sndf.Type != BoxTypes.Sndf)
            {
                throw new ArgumentException("not sndf");
            }
            var it = Parser.ParseBoxes(sndf.Value).GetEnumerator();
            it.MoveNext();
            var ipad = it.Current;
            it.MoveNext();
            var rcvp = it.Current;
            return new Sndf
            {
                IpAddress = ipad.Value,
                ReceivePort = BitConverter.ToUInt16(rcvp.Value.Array, rcvp.Value.Offset),
            };
        }
    }
}
