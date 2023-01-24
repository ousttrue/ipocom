using System;
using System.Linq;
using NUnit.Framework;


namespace Ipocom.SonyMotionFormat
{
    public class NewEditModeTest
    {
        const string SkeletonPath = "Assets/Ipocom/Tests/skeleton.smf.txt";
        const string FramePath = "Assets/Ipocom/Tests/frame.smf.txt";

        public void ParseRecursive(ArraySegment<byte> bytes, string indent = "")
        {
            foreach (var (type, value) in Parser.ParseBoxes(bytes))
            {
                // Debug.Log($"{indent}{type} => {value.Count}");
                if (type.IsNested())
                {
                    ParseRecursive(value, indent + "  ");
                }
            }
        }

        [Test]
        public void Skeleton()
        {
            var bytes = System.IO.File.ReadAllBytes(SkeletonPath);
            ParseRecursive(new ArraySegment<byte>(bytes));

            var r = new BytesReader(bytes);
            var h = SonyMotionFormat.Head.FromBox(r.ReadBox());
            Assert.True(h.FileType.SequenceEqual(Head.FILE_TYPE));
            Assert.AreEqual(h.Version, Head.VERSION);
            var s = SonyMotionFormat.Sndf.FromBox(r.ReadBox());
            Assert.AreEqual(s.ReceivePort, 12351);
            var k = SonyMotionFormat.Skdf.FromBox(r.ReadBox());
        }

        [Test]
        public void Frame()
        {
            var bytes = System.IO.File.ReadAllBytes(FramePath);
            ParseRecursive(new ArraySegment<byte>(bytes));

            var r = new BytesReader(bytes);
            var h = SonyMotionFormat.Head.FromBox(r.ReadBox());
            Assert.True(h.FileType.SequenceEqual(Head.FILE_TYPE));
            Assert.AreEqual(h.Version, Head.VERSION);
            var s = SonyMotionFormat.Sndf.FromBox(r.ReadBox());
            Assert.AreEqual(s.ReceivePort, 12351);
            var f = SonyMotionFormat.Fram.FromBox(r.ReadBox());
        }
    }
}