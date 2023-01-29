using UnityEngine;

struct RigidTransform
{
    public Quaternion Rotation;
    public Vector3 Translation;

    public static RigidTransform Identity => new RigidTransform
    {
        Rotation = Quaternion.identity,
        Translation = Vector3.zero,
    };

    public static RigidTransform FromMatrix(Matrix4x4 m)
    {
        return new RigidTransform
        {
            Rotation = m.rotation,
            Translation = m.GetColumn(3),
        };
    }

    public Matrix4x4 Matrix
    {
        get
        {
            var r = Matrix4x4.Rotate(Rotation);
            r.SetColumn(3, new Vector4(Translation.x, Translation.y, Translation.z, 1));
            return r;
        }
    }
}
