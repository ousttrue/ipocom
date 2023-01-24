using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public class Fram
    {
        public UInt32 FrameNumber;
        public UInt32 Time;
        public Box<Btdt>[] BoneTransformations = new Box<Btdt>[27];

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
            if (btrs.Type != BoxTypes.Btrs)
            {
                throw new ArgumentException("not btrs");
            }
#if DEBUG
            if (Marshal.SizeOf<Box<Btdt>>() * Definition.BONE_COUNT != btrs.Value.Count)
            {
                throw new ArgumentException("invalid size");
            }
#endif                
            var frames = new Fram
            {
                FrameNumber = BitConverter.ToUInt32(fnum.Value.Array, fnum.Value.Offset),
                Time = BitConverter.ToUInt32(time.Value.Array, time.Value.Offset),
            };
            using (var pin = new ArrayPin(frames.BoneTransformations))
            {
                Marshal.Copy(btrs.Value.Array, btrs.Value.Offset, pin.Ptr, btrs.Value.Count);
            }
            return frames;
        }
    }
}