using System;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
    public enum Coords
    {
        RighHandledOriginal,
        LeftHandedReverseX,
    }

    [Serializable]
    public struct Tran
    {
        public float rx;
        public float ry;
        public float rz;
        public float rw;
        public float tx;
        public float ty;
        public float tz;
        public static Tran FromBox(Box tran)
        {
            if (tran.Type != BoxTypes.Tran)
            {
                throw new ArgumentException("not tran");
            }
            return new Tran
            {
                rx = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset),
                ry = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 4),
                rz = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 8),
                rw = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 12),
                tx = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 16),
                ty = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 20),
                tz = BitConverter.ToSingle(tran.Value.Array, tran.Value.Offset + 24),
            };
        }

        public Quaternion Rotation(Coords coords)
        {
            switch (coords)
            {
                case Coords.RighHandledOriginal:
                    return new Quaternion(
                        rx,
                        ry,
                        rz,
                        rw
                    );

                case Coords.LeftHandedReverseX:
                    return new Quaternion(
                        -rx,
                        ry,
                        rz,
                        -rw
                    );

                default:
                    throw new NotImplementedException();
            }
        }

        public Vector3 Translation(Coords coords)
        {
            switch (coords)
            {
                case Coords.RighHandledOriginal:
                    return new Vector3(tx, ty, tz);

                case Coords.LeftHandedReverseX:
                    return new Vector3(-tx, ty, tz);

                default:
                    throw new NotImplementedException();
            }
        }

        public Matrix4x4 Matrix(Coords coords)
        {
            return Matrix4x4.TRS(Translation(coords), Rotation(coords), Vector3.one);
        }
    }
}