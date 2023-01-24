using System;

namespace Ipocom.SonyMotionFormat
{
    [Serializable]

    public class Skdf
    {
        public Bndt[] Bones = new Bndt[Definition.BONE_COUNT];

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
            var skeleton = new Skdf();
            int i = 0;
            foreach (var bndt in Parser.ParseBoxes(bons.Value))
            {
                skeleton.Bones[i] = Bndt.FromBox(bndt);
                ++i;
            }
            if (i != Definition.BONE_COUNT)
            {
                throw new ArgumentException($"{i}!={Definition.BONE_COUNT}");
            }
            return skeleton;
        }
    }
}
