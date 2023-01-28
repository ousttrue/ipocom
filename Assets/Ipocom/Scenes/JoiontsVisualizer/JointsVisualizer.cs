using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    JointsSkeleton m_skeleton;

    public void OnSkeleton(Ipocom.SonyMotionFormat.SkeletonMessage skeleton)
    {
        if (m_skeleton != null)
        {
            m_skeleton.Dispose();
        }

        m_skeleton = new JointsSkeleton(transform);
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.AddJoint(bone.BoneId.Value.BoneId, bone.Transformation.Value.Rotation(), bone.Transformation.Value.Translation());
        }
    }

    public void OnFrame(Ipocom.SonyMotionFormat.FrameMessage frame)
    {
        if (m_skeleton == null)
        {
            return;
        }
        foreach (var bone in frame.fram.BoneTransformations)
        {
            var boneTransformation = bone.Value.Transformation.Value;
            var joint = m_skeleton.GetJoint(bone.Value.BoneId.Value.BoneId);
            joint.localPosition = boneTransformation.Translation();
            joint.localRotation = boneTransformation.Rotation();
        }
    }
}
