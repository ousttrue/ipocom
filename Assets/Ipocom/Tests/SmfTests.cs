using System;
using System.Linq;
using Ipocom;
using NUnit.Framework;
using UnityEngine;

public class NewEditModeTest
{
    const string SkeletonPath = "Assets/Ipocom/Tests/skeleton.smf.txt";
    const string FramePath = "Assets/Ipocom/Tests/frame.smf.txt";

    public void ParseRecursive(ArraySegment<byte> bytes, string indent = "")
    {
        foreach (var (type, value) in SonyMotionFormat.ParseFields(bytes))
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
        var h = SonyMotionFormat.Head.FromField(r.ReadField());
        Assert.True(h.FileType.SequenceEqual(SonyMotionFormat.FileType));
        Assert.AreEqual(h.Version, SonyMotionFormat.Version);
        var s = SonyMotionFormat.Sndf.FromField(r.ReadField());
        Assert.AreEqual(s.ReceivePort, 12351);
        var k = SonyMotionFormat.Skdf.FromField(r.ReadField());
    }

    [Test]
    public void Frame()
    {
        var bytes = System.IO.File.ReadAllBytes(FramePath);
        ParseRecursive(new ArraySegment<byte>(bytes));

        var r = new BytesReader(bytes);
        var h = SonyMotionFormat.Head.FromField(r.ReadField());
        Assert.True(h.FileType.SequenceEqual(SonyMotionFormat.FileType));
        Assert.AreEqual(h.Version, SonyMotionFormat.Version);
        var s = SonyMotionFormat.Sndf.FromField(r.ReadField());
        Assert.AreEqual(s.ReceivePort, 12351);
        var f = SonyMotionFormat.Fram.FromField(r.ReadField());
    }
}
