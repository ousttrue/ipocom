using System;
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
            Debug.Log($"{indent}{type} => {value.Count}");
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
    }

    [Test]
    public void Frame()
    {
        var bytes = System.IO.File.ReadAllBytes(FramePath);
        ParseRecursive(new ArraySegment<byte>(bytes));
    }
}
