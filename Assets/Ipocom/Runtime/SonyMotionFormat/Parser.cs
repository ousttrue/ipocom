using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ipocom.SonyMotionFormat
{
    public static class Parser
    {
        static readonly IReadOnlyDictionary<UInt32, BoxTypes> s_keyMap = InitKeyMap();

        static Dictionary<UInt32, BoxTypes> InitKeyMap()
        {
            var map = new Dictionary<UInt32, BoxTypes>();
            foreach (BoxTypes type in Enum.GetValues(typeof(BoxTypes)))
            {
                var bytes = System.Text.Encoding.ASCII.GetBytes(type.ToString().ToLower());
                map.Add(BitConverter.ToUInt32(bytes, 0), type);
            }
            return map;
        }

        public static BoxTypes GetFieldType(ArraySegment<byte> key)
        {
            if (s_keyMap.TryGetValue(BitConverter.ToUInt32(key.Array, key.Offset), out BoxTypes type))
            {
                return type;
            }
            throw new ArgumentException($"unknown key: {System.Text.Encoding.ASCII.GetString(key.Array, key.Offset, key.Count)}");
        }

        public static Box ReadBox(this BytesReader r)
        {
            var length = r.GetInt32();
            var type = r.Get(4);
            var value = r.Get(length);
            return new Box
            {
                Type = GetFieldType(type),
                Value = value,
            };
        }

        // https://github.com/seagetch/mcp-receiver/blob/main/doc/Protocol.md
        public static IEnumerable<Box> ParseBoxes(ArraySegment<byte> bytes)
        {
            var r = new BytesReader(bytes);
            while (!r.IsEnd)
            {
                yield return r.ReadBox();
            }
        }

        public static object Parse(ArraySegment<byte> bytes)
        {
            var r = new BytesReader(bytes);

            // header
            var head = Head.FromBox(r.ReadBox());
            if (!head.FileType.SequenceEqual(Head.FILE_TYPE))
            {
                throw new ArgumentException($"invalid file type: {Encoding.ASCII.GetString(head.FileType.Array, head.FileType.Offset, head.FileType.Count)}");
            }
            if (head.Version != Head.VERSION)
            {
                throw new ArgumentException($"invalid version: {head.Version}");
            }
            var sndf = Sndf.FromBox(r.ReadBox());

            var f = r.ReadBox();
            switch (f.Type)
            {
                case BoxTypes.Skdf:
                    return new SkeletonMessage
                    {
                        head = head,
                        sndf = sndf,
                        skdf = Skdf.FromBox(f),
                    };

                case BoxTypes.Fram:
                    return new FrameMessage
                    {
                        head = head,
                        sndf = sndf,
                        fram = Fram.FromBox(f),
                    };

                default:
                    throw new ArgumentException();
            }
        }
    }
}