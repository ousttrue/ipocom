using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public bool m_init;

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
            m_skeleton.AddJoint(bone.BoneId.Value.BoneId,
                bone.Transformation.Value.Rotation(Ipocom.SonyMotionFormat.Coords.LeftHandedReverseX),
                bone.Transformation.Value.Translation(Ipocom.SonyMotionFormat.Coords.LeftHandedReverseX));
        }
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.SetParent(bone.BoneId.Value.BoneId, bone.ParentBoneId.Value.ParentBoneId);
        }
    }

    public void OnFrame(Ipocom.SonyMotionFormat.FrameMessage frame)
    {
        if (m_init)
        {
            return;
        }
        if (m_skeleton == null)
        {
            return;
        }
        foreach (var bone in frame.fram.BoneTransformations)
        {
            var boneTransformation = bone.Value.Transformation.Value;
            var (joint, parentMatrix) = m_skeleton.GetJoint(bone.Value.BoneId.Value.BoneId);
            joint.ApplyTransform(parentMatrix, boneTransformation.Matrix(Ipocom.SonyMotionFormat.Coords.LeftHandedReverseX));
        }
    }

    public void Update()
    {
        if (!m_init)
        {
            return;
        }
        if (m_skeleton == null)
        {
            return;
        }
        m_skeleton.InitPose();
    }
}
