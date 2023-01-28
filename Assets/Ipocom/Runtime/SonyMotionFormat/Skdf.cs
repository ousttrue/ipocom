using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public class Skdf
    {
        // public Bndt[] Bones = new Bndt[Definition.BONE_COUNT];
        public Box<Bndt>[] Bones = new Box<Bndt>[Definition.BONE_COUNT];

        public override bool Equals(object obj)
        {
            if (obj is Skdf rhs)
            {
                if (Bones != null)
                {
                    return Bones.SequenceEqual(rhs.Bones);
                }
                else
                {
                    return rhs.Bones == null;
                }
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return 984241268 + EqualityComparer<Box<Bndt>[]>.Default.GetHashCode(Bones);
        }

        // skdf
        //   bons
        //     bndt x 27
        public static Skdf FromBox(Box skdf)
        {
            if (skdf.Type != BoxTypes.Skdf)
            {
                throw new ArgumentException("not skdf");
            }
            var bons = Parser.ReadBox(new BytesReader(skdf.Value));
            if (bons.Type != BoxTypes.Bons)
            {
                throw new ArgumentException("not bons");
            }
#if DEBUG
            if (Marshal.SizeOf<Box<Bndt>>() * Definition.BONE_COUNT != bons.Value.Count)
            {
                throw new ArgumentException("invalid size");
            }
#endif                
            var skeleton = new Skdf();
            using (var pin = new ArrayPin(skeleton.Bones))
            {
                Marshal.Copy(bons.Value.Array, bons.Value.Offset, pin.Ptr, bons.Value.Count);
            }
            return skeleton;
        }
    }
}
