using System;
using System.Collections.Generic;
using UnityEngine;

class JointsSkeleton : IDisposable
{
    Transform m_root;
    Dictionary<int, Transform> m_joints = new Dictionary<int, Transform>();

    public JointsSkeleton(Transform root)
    {
        m_root = root;
    }

    public void Dispose()
    {
        foreach (var kv in m_joints)
        {
            GameObject.Destroy(kv.Value.gameObject);
        }
    }

    public void AddJoint(int id, Quaternion r, Vector3 t)
    {
        var joint = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        joint.name = $"[{id:00}]{(Ipocom.SonyMotionFormat.Bones)id}";
        joint.SetParent(m_root);
        m_joints[id] = joint;
        joint.localPosition = t;
        joint.localRotation = r;
        joint.localScale = new Vector3(0.03f, 0.03f, 0.03f);
    }

    public Transform GetJoint(int id)
    {
        return m_joints[id];
    }
}
