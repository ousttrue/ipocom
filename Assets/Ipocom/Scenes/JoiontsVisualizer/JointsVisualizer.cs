using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public Ipocom.SonyMotionFormat.SkeletonMessage m_skeleton;
    public Ipocom.SonyMotionFormat.FrameMessage m_frame;

    public void OnSkeleton(Ipocom.SonyMotionFormat.SkeletonMessage skeleton)
    {
        m_skeleton = skeleton;
    }

    public void OnFrame(Ipocom.SonyMotionFormat.FrameMessage frame)
    {
        m_frame = frame;
    }
}
