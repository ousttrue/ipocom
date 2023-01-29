using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


class JointsSkeleton : IDisposable
{
    Transform m_root;
    class Joint
    {
        public RigidTransform Transform;
        public Matrix4x4 Initial;
        public Matrix4x4 Shape = Matrix4x4.Scale(new Vector3(0.02f, 0.02f, 0.02f));
        public Matrix4x4 ShapeLocalMatrix => Transform.Matrix * Shape;

        public void SetTail(Joint child)
        {
            var _y = child.Initial.GetColumn(3);
            var tail = new Vector3(_y.x, _y.y, _y.z);
            var y = tail.normalized;
            var z = Vector3.forward;
            var x = Vector3.Cross(y, z).normalized;
            z = Vector3.Cross(x, y).normalized;

            var r = new Matrix4x4(
                new Vector4(x.x, x.y, x.z, 0),
                new Vector4(y.x, y.y, y.z, 0),
                new Vector4(z.x, z.y, z.z, 0),
                new Vector4(0, 0, 0, 1)
                );
            var s = Matrix4x4.Scale(new Vector3(0.02f, tail.magnitude, 0.02f));
            Shape = r * s;
        }
    }
    Dictionary<int, Joint> m_joints = new Dictionary<int, Joint>();
    Dictionary<Joint, Joint> m_parentMap = new Dictionary<Joint, Joint>();

    Matrix4x4[] m_matrices = new Matrix4x4[128];
    List<Vector4> m_colors = new List<Vector4>();
    MaterialPropertyBlock m_props = new MaterialPropertyBlock();

    public JointsSkeleton(Transform root)
    {
        m_root = root;
    }

    public void Dispose()
    {
    }

    public void AddJoint(int id, Quaternion r, Vector3 t)
    {
        m_joints[id] = new Joint
        {
            Transform = RigidTransform.Identity,
            Initial = Matrix4x4.Rotate(r) * Matrix4x4.TRS(t, r, Vector3.one),
        };

        while (m_colors.Count <= id)
        {
            m_colors.Add(Color.white);
        }
        Color color = Ipocom.SonyMotionFormat.Definition.ColorMap[(Ipocom.SonyMotionFormat.Bones)id];
        m_colors[id] = color;
    }

    public void SetParent(int id, int parentId)
    {
        if (m_joints.TryGetValue(parentId, out Joint parent))
        {
            var child = m_joints[id];
            m_parentMap[child] = parent;
        }
    }

    public void SetTail(int head, int tail)
    {
        m_joints[head].SetTail(m_joints[tail]);
    }

    (Joint, Matrix4x4 ParentMatrix) GetJoint(int id)
    {
        var joint = m_joints[id];
        if (m_parentMap.TryGetValue(joint, out Joint parent))
        {
            return (joint, parent.Transform.Matrix);
        }
        else
        {
            return (joint, Matrix4x4.identity);
        }
    }

    public void SetMatrix(int id, Matrix4x4 local)
    {
        var (joint, parentMatrix) = GetJoint(id);
        joint.Transform = RigidTransform.FromMatrix(parentMatrix * local);
        // x-mirror for right handed coordinate
        m_matrices[id] = Matrix4x4.Scale(new Vector3(-1, 1, 1)) * m_root.localToWorldMatrix * joint.ShapeLocalMatrix;
    }

    public void InitPose()
    {
        foreach (var id in m_joints.Keys.OrderBy(x => x))
        {
            var (joint, matrix) = GetJoint(id);
            SetMatrix(id, joint.Initial);
        }
    }

    public void Draw(Mesh mesh, Material material)
    {
        m_props.SetVectorArray("_Color", m_colors);
        Graphics.DrawMeshInstanced(mesh, 0, material, m_matrices, m_colors.Count, m_props);
    }
}
