using System;
using System.Runtime.InteropServices;

namespace Ipocom.SonyMotionFormat
{
    public struct Box
    {
        public BoxTypes Type;
        public ArraySegment<byte> Value;

        public void Deconstruct(out BoxTypes type, out ArraySegment<byte> value)
        {
            type = Type;
            value = Value;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Box<T> where T : struct
    {
        public UInt32 Length;
        public UInt32 BoxName;
        public T Value;
    }
}
