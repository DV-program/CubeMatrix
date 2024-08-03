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
        float[][] offsetData = new float[offsets.Count][];

        for (int i = 0; i < offsets.Count; i++)
        {
            Matrix4x4 matrix = offsets[i];
            offsetData[i] = new float[]
            {
                matrix[0, 0], matrix[0, 1], matrix[0, 2], matrix[0, 3],
                matrix[1, 0], matrix[1, 1], matrix[1, 2], matrix[1, 3],
                matrix[2, 0], matrix[2, 1], matrix[2, 2], matrix[2, 3],
                matrix[3, 0], matrix[3, 1], matrix[3, 2], matrix[3, 3]
            };
        }

        string json = JsonUtility.ToJson(new MatrixArrayWrapper() { matrices = offsetData });
        File.WriteAllText(Path.Combine(Application.dataPath, filePath), json);
    }
}

[Serializable]
public class MatrixDataArrayWrapper
{
    public MatrixData[] matrices;
}

[Serializable]
public class MatrixArrayWrapper
{
    public float[][] matrices;
}
