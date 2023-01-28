using System.Collections.Generic;
using UnityEngine;

public class JointsVisualizer : MonoBehaviour
{
    public bool m_init;
    const float CUBE_SIZE = 0.025f;
    public Mesh m_mesh;
    public Material m_material;
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
            var id = bone.Value.BoneId.Value.BoneId;
            m_skeleton.SetMatrix(id, boneTransformation.Matrix(Ipocom.SonyMotionFormat.Coords.LeftHandedReverseX));
        }
    }

    void Start()
    {
        if (m_mesh == null)
        {
            var builder = new MeshBuilder();

            //    7 6
            //    +-+
            //   / /|
            // 4+-+5+2
            //  | |/
            //  +-+
            //  0 1
            //  ------> x
            var s = CUBE_SIZE;
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
