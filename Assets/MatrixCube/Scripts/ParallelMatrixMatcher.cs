using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ParallelMatrixMatcher : MonoBehaviour
{
	public async Task<List<Matrix4x4>> FindAllMatchingOffsetsAsync(List<Matrix4x4> model, List<Matrix4x4> space)
	{
		return await Task.Run(() =>
		{
			HashSet<Matrix4x4> spaceSet = new HashSet<Matrix4x4>(space);
			List<Matrix4x4> allMatchingOffsets = new List<Matrix4x4>();
			object lockObject = new object();

			Parallel.ForEach(space, spaceMatrix =>
			{
				List<Matrix4x4> localOffsets = new List<Matrix4x4>();

				foreach (var modelMatrix in model)
				{
					Matrix4x4 offset = spaceMatrix * modelMatrix.inverse;

					if (IsOffsetValidForAll(model, spaceSet, offset))
					{
						localOffsets.Add(offset);
					}
				}

				lock (lockObject)
				{
					foreach (var offset in localOffsets)
					{
						if (!ContainsMatrix(allMatchingOffsets, offset))
						{
							Debug.Log("Found new offset");
							allMatchingOffsets.Add(offset);
						}
					}
				}
			});

			return allMatchingOffsets;
		});
	}

	private bool IsOffsetValidForAll(List<Matrix4x4> model, HashSet<Matrix4x4> spaceSet, Matrix4x4 offset)
	{
		foreach (var modelMatrix in model)
		{
			Matrix4x4 transformedMatrix = offset * modelMatrix;
			if (!ContainsMatrix(spaceSet, transformedMatrix))
			{
				return false;
			}
		}
		return true;
	}

	private bool ContainsMatrix(IEnumerable<Matrix4x4> matrices, Matrix4x4 targetMatrix)
	{
		foreach (var matrix in matrices)
		{
			if (AreMatricesEqual(matrix, targetMatrix))
			{
				return true;
			}
		}
		return false;
	}

	private bool AreMatricesEqual(Matrix4x4 a, Matrix4x4 b, float tolerance = 0.0001f)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				if (Mathf.Abs(a[i, j] - b[i, j]) > tolerance)
				{
					return false;
				}
			}
		}
		return true;
	}
}
