using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugReceiver : MonoBehaviour
{
    public Ipocom.SkeletonMessage m_skeleton;
    public Ipocom.FrameMessage m_frame;

    public void OnSkeleton(Ipocom.SkeletonMessage skeleton)
    {
        m_skeleton = skeleton;
    }

    public void OnFrame(Ipocom.FrameMessage frame)
    {
        m_frame = frame;
    }
}
