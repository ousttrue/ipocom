using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public bool m_init;
    const float CUBE_SIZE = 0.025f;
    public Material m_material;
    RigidCubes.JointsSkeletonBase m_skeleton;

    public void OnSkeleton(Ipocom.SonyMotionFormat.SkeletonMessage skeleton)
    {
        if(m_skeleton!=null)
        {
            m_skeleton.Dispose();
        }
        m_skeleton = new RigidCubes.RelativeJointsSkeleton(transform);
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.AddJoint(bone.BoneId.Value.BoneId,
                bone.Transformation.Value.Rotation(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal),
                bone.Transformation.Value.Translation(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal));
        }
        for (int i = 0; i < skeleton.skdf.Bones.Length; ++i)
        {
            var bone = skeleton.skdf.Bones[i].Value;
            m_skeleton.SetParent(bone.BoneId.Value.BoneId, bone.ParentBoneId.Value.ParentBoneId);
        }
        foreach (var (head, tail) in Ipocom.SonyMotionFormat.Definition.HeadTailPairs)
        {
            m_skeleton.SetTail((int)head, (int)tail);
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
            var id = bone.Value.BoneId.Value.BoneId;
            var (r, t) = boneTransformation.Transform(Ipocom.SonyMotionFormat.Coords.RighHandledOriginal);
            m_skeleton.SetTransform(id, new RigidCubes.RigidTransform(r, t));
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
            m_skeleton.Draw(m_material);
        }
    }
}
