using System;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public class Fram
    {
        public UInt32 FrameNumber;
        public UInt32 Time;
        public Btdt[] BoneTransformations = new Btdt[27];

        // fram
        //   fnum
        //   time
        //   btrs
        //     btdt x 27
        public static Fram FromBox(Box fram)
        {
            if (fram.Type != BoxTypes.Fram)
            {
                throw new ArgumentException("not fram");
            }
            var it = Parser.ParseBoxes(fram.Value).GetEnumerator();
            it.MoveNext();
            var fnum = it.Current;
            it.MoveNext();
            var time = it.Current;
            it.MoveNext();
            var btrs = it.Current;
            var frames = new Fram
            {
                FrameNumber = BitConverter.ToUInt32(fnum.Value.Array, fnum.Value.Offset),
                Time = BitConverter.ToUInt32(time.Value.Array, time.Value.Offset),
            };
            var i = 0;
            foreach (var btdt in Parser.ParseBoxes(btrs.Value))
            {
                frames.BoneTransformations[i] = Btdt.FromBox(btdt);
                ++i;
            }
            if (i != Definition.BONE_COUNT)
            {
                throw new ArgumentException($"{i}!={Definition.BONE_COUNT}");
            }
            return frames;
        }
    }
}