using System;

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
}
