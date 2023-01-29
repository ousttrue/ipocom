using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public bool m_init;
    const float CUBE_SIZE = 0.025f;
    public Mesh m_mesh;
    public Material m_material;
    RigidCubes.JointsSkeletonBase m_skeleton;

    public void OnSkeleton(Ipocom.SonyMotionFormat.SkeletonMessage skeleton)
    {
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

    void Start()
    {
        if (m_mesh == null)
        {
            var builder = new RigidCubes.MeshBuilder();

            // reauire Y-Up 1.0f size Shape.
            //
            //    7 6
            //    +-+
            //   / /|
            // 4+-+5+2
            //  | |/
            //  +-+
            //  0 1
            //  ------> x
            var s = 0.5f;
            var v0 = new Vector3(-s, 0, -s);
            var v1 = new Vector3(+s, 0, -s);
            var v2 = new Vector3(+s, 0, +s);
            var v3 = new Vector3(-s, 0, +s);
            var v4 = new Vector3(-s, 2 * s, -s);
            var v5 = new Vector3(+s, 2 * s, -s);
            var v6 = new Vector3(+s, 2 * s, +s);
            var v7 = new Vector3(-s, 2 * s, +s);

            builder.PushQuadrangle(v0, v1, v2, v3);
            builder.PushQuadrangle(v5, v4, v7, v6);
            builder.PushQuadrangle(v1, v0, v4, v5);
            builder.PushQuadrangle(v2, v1, v5, v6);
            builder.PushQuadrangle(v3, v2, v6, v7);
            builder.PushQuadrangle(v0, v3, v7, v4);

            m_mesh = builder.ToMesh();
            m_mesh.name = nameof(JointsVisualizer);
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
            m_skeleton.Draw(m_mesh, m_material);
        }
    }
}
