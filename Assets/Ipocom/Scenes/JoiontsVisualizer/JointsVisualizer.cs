using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public bool m_init;
    RigidCubes.JointsSkeleton m_skeleton;

    public void OnSkeleton(Ipocom.SonyMotionFormat.SkeletonMessage skeleton)
    {
        m_skeleton = new RigidCubes.JointsSkeleton(RigidCubes.CoordinateConversion.XReverse, transform, Ipocom.SonyMotionFormat.Definition.BONE_COUNT);
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.AddJointRelative(bone.BoneId.Value.BoneId,
                bone.Transformation.Value.Rotation(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal),
                bone.Transformation.Value.Translation(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal));
        }
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.SetParentRelative(bone.BoneId.Value.BoneId, bone.ParentBoneId.Value.ParentBoneId);
        }
        foreach (var (head, tail) in Ipocom.SonyMotionFormat.Definition.HeadTailPairs)
        {
            m_skeleton.YAxisHeadTailShape((int)head, (int)tail, Vector3.forward);
        }
        m_skeleton.SetupSkinning();
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
            var id = bone.Value.BoneId.Value.BoneId;
            var (r, t) = boneTransformation.Transform(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal);
            m_skeleton.SetTransformRelative(id, new RigidCubes.RigidTransform(r, t));
        }
    }

    void Update()
    {
        if (m_skeleton != null)
        {
            if (m_init)
            {
                m_skeleton.InitPose();
            }
        }
    }
}
