using System;
using UnityEngine;
[Serializable]
public class MatrixData
{
    public float m00, m10, m20, m30;
    public float m01, m11, m21, m31;
    public float m02, m12, m22, m32;
    public float m03, m13, m23, m33;

    public Matrix4x4 ToMatrix4x4()
    {
        return new Matrix4x4(
            new Vector4(m00, m10, m20, m30),
            new Vector4(m01, m11, m21, m31),
            new Vector4(m02, m12, m22, m32),
            new Vector4(m03, m13, m23, m33)
        );
    }
}