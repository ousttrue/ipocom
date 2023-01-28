using System;
using System.Collections.Generic;
using UnityEngine;

struct Joint
{
    public Transform Transform;
    public Matrix4x4 Initial;

    public void ApplyTransform(Matrix4x4 parent, Matrix4x4 local)
    {
        var m = parent * Initial * local;
        Transform.localPosition = m.GetColumn(3);
        Transform.localRotation = m.rotation;
    }
}

class JointsSkeleton : IDisposable
{
    Transform m_root;
    Dictionary<int, Joint> m_joints = new Dictionary<int, Joint>();
    Dictionary<Transform, Transform> m_parentMap = new Dictionary<Transform, Transform>();

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

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        var cubeSize = 0.08f;
        cube.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        cube.SetParent(joint);
    }

    public void SetParent(int id, int parentId)
    {
        if (m_joints.TryGetValue(parentId, out Joint parent))
        {
            m_parentMap[m_joints[id].Transform] = parent.Transform;
        }
    }

    public (Joint, Matrix4x4 ParentMatrix) GetJoint(int id)
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
}
