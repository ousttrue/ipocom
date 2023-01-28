using System;
using UnityEngine;

namespace Ipocom.SonyMotionFormat
{
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

        public Quaternion Rotation()
        {
            return new Quaternion(
                rx,
                ry,
                rz,
                rw
            );
        }

        public Vector3 Translation()
        {
            return new Vector3(tx, ty, tz);
        }

        public Matrix4x4 Matrix()
        {
            return Matrix4x4.TRS(Translation(), Rotation(), Vector3.one);
        }
    }
}