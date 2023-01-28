using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


class JointsSkeleton : IDisposable
{
    Transform m_root;
    struct Joint
    {
        public Transform Transform;
        public Matrix4x4 Initial;

        public void ApplyTransform(Matrix4x4 parent, Matrix4x4 local)
        {
            var m = parent * local;
            Transform.localPosition = m.GetColumn(3);
            Transform.localRotation = m.rotation;
        }
    }
    Dictionary<int, Joint> m_joints = new Dictionary<int, Joint>();
    Dictionary<Transform, Transform> m_parentMap = new Dictionary<Transform, Transform>();

    Matrix4x4[] m_matrices = new Matrix4x4[128];
    List<Vector4> m_colors = new List<Vector4>();
    MaterialPropertyBlock m_props = new MaterialPropertyBlock();

    public JointsSkeleton(Transform root)
    {
        m_root = root;
    }

    public void Dispose()
    {
        foreach (var kv in m_joints)
        {
            GameObject.Destroy(kv.Value.Transform.gameObject);
        }
    }

    public void AddJoint(int id, Quaternion r, Vector3 t)
    {
        var joint = new GameObject($"[{id:00}]{(Ipocom.SonyMotionFormat.Bones)id}").transform;
        joint.SetParent(m_root);
        m_joints[id] = new Joint
        {
            Transform = joint,
            Initial = Matrix4x4.Rotate(r) * Matrix4x4.TRS(t, r, Vector3.one),
        };

        while (m_colors.Count <= id)
        {
            m_colors.Add(Color.white);
        }
        m_colors[id] = new Vector4(
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            1
        );
    }

    public void SetParent(int id, int parentId)
    {
        if (m_joints.TryGetValue(parentId, out Joint parent))
        {
            m_parentMap[m_joints[id].Transform] = parent.Transform;
        }
    }

    (Joint, Matrix4x4 ParentMatrix) GetJoint(int id)
    {
        var joint = m_joints[id];
        if (m_parentMap.TryGetValue(joint.Transform, out Transform parent))
        {
            return (joint, m_root.worldToLocalMatrix * parent.localToWorldMatrix);
        }
        else
        {
            return (joint, Matrix4x4.identity);
        }
    }

    public void InitPose()
    {
        foreach (var id in m_joints.Keys.OrderBy(x => x))
        {
            var (joint, matrix) = GetJoint(id);
            joint.ApplyTransform(matrix, joint.Initial);
            SetMatrix(id, joint.Transform.localToWorldMatrix);
        }
    }

    public void SetMatrix(int id, Matrix4x4 m)
    {
        var (joint, parentMatrix) = GetJoint(id);
        joint.ApplyTransform(parentMatrix, m);
        m_matrices[id] = joint.Transform.localToWorldMatrix;
    }

    public void Draw(Mesh mesh, Material material)
    {
        m_props.SetVectorArray("_Color", m_colors);
        Graphics.DrawMeshInstanced(mesh, 0, material, m_matrices, m_colors.Count, m_props);
    }
}
