using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MatrixLoader
{
	public List<Matrix4x4> LoadMatricesFromJson(string filePath)
	{
		List<Matrix4x4> matrices = new List<Matrix4x4>();
		string json = File.ReadAllText(Path.Combine(Application.dataPath, filePath));
		MatrixData[] matrixDataArray = JsonUtility.FromJson<MatrixDataArrayWrapper>("{\"matrices\":" + json + "}").matrices;

		foreach (var matrixData in matrixDataArray)
		{
			matrices.Add(matrixData.ToMatrix4x4());
		}

		return matrices;
	}

	public void ExportOffsetsToJson(List<Matrix4x4> offsets, string filePath)
    {
        if (offsets == null || offsets.Count == 0)
        {
            Debug.LogError("Offsets list is empty or null.");
            return;
        }

        MatrixData[] offsetData = new MatrixData[offsets.Count];

        for (int i = 0; i < offsets.Count; i++)
        {
            Matrix4x4 matrix = offsets[i];
            offsetData[i] = new MatrixData
            {
                m00 = matrix.m00, m01 = matrix.m01, m02 = matrix.m02, m03 = matrix.m03,
                m10 = matrix.m10, m11 = matrix.m11, m12 = matrix.m12, m13 = matrix.m13,
                m20 = matrix.m20, m21 = matrix.m21, m22 = matrix.m22, m23 = matrix.m23,
                m30 = matrix.m30, m31 = matrix.m31, m32 = matrix.m32, m33 = matrix.m33
            };
        }

        string json = JsonUtility.ToJson(new SerializationWrapper<MatrixData>(offsetData), true);
        json = json.Substring(json.IndexOf('['), json.LastIndexOf(']') - json.IndexOf('[') + 1);
        File.WriteAllText(filePath, json);
        Debug.Log($"Offsets successfully written to {filePath}");
    }

}

[Serializable]
public class MatrixDataArrayWrapper
{
	public MatrixData[] matrices;
}
[Serializable]
public class SerializationWrapper<T>
{
    public T[] items;
    public SerializationWrapper(T[] items)
    {
        this.items = items;
    }
}

