using System;

namespace Ipocom
{
    public class BytesReader
    {
        ArraySegment<byte> data_;
        int pos_ = 0;

        public bool IsEnd => pos_ >= data_.Count;

        public BytesReader(ArraySegment<byte> data)
        {
            data_ = data;
        }
        public BytesReader(byte[] data) : this(new ArraySegment<byte>(data))
        {
        }

        public ArraySegment<byte> Get(int size)
        {
            if (pos_ + size > data_.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            var value = new ArraySegment<byte>(data_.Array, data_.Offset + pos_, size);
            pos_ += size;
            return value;
        }

        public Int32 GetInt32()
        {
            var range = Get(4);
            return BitConverter.ToInt32(range.Array, range.Offset);
        }
    }
}