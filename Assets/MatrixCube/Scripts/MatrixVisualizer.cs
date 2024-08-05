using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixVisualizer : MonoBehaviour
{
	[SerializeField] private GameObject spacePrefab;
	[SerializeField] private GameObject matchingPrefab;
	[SerializeField] private float lerpDuration = 2f;
	[SerializeField] private float cubeSize = 1f;

	public void Visualize(List<Matrix4x4> modelMatrices, List<Matrix4x4> spaceMatrices, List<Matrix4x4> offsets)
	{
		List<GameObject> spaceCubes = CreateCubes(spaceMatrices, spacePrefab);
		StartCoroutine(AnimateMatchings(modelMatrices, offsets));
	}

	private List<GameObject> CreateCubes(List<Matrix4x4> matrices, GameObject prefab)
	{
		List<GameObject> cubes = new List<GameObject>();

		foreach (var matrix in matrices)
		{
			GameObject cube = Instantiate(prefab);
			ApplyMatrixToCube(cube, matrix);
			cubes.Add(cube);
		}
		return cubes;
	}

	private void ApplyMatrixToCube(GameObject cube, Matrix4x4 matrix)
	{
		cube.transform.position = matrix.GetPosition();
		cube.transform.rotation = Quaternion.LookRotation(matrix.GetColumn(2).normalized, matrix.GetColumn(1).normalized);
		cube.transform.localScale = new Vector3(
			matrix.GetColumn(0).magnitude,
			matrix.GetColumn(1).magnitude,
			matrix.GetColumn(2).magnitude
		) * cubeSize;
	}

	private List<Matrix4x4> CreateMatchingList(List<Matrix4x4> modelMatrices, Matrix4x4 offset)
	{
		List<Matrix4x4> matchingMatrices = new();
		foreach (var matrix in modelMatrices)
		{
			matchingMatrices.Add(offset * matrix);
		}
		return matchingMatrices;
	}

	private IEnumerator AnimateMatchings(List<Matrix4x4> modelMatrices, List<Matrix4x4> offsets)
	{
		List<Matrix4x4> matchingMatrices = CreateMatchingList(modelMatrices, offsets[0]);
		List<GameObject> matchingCubes = CreateCubes(matchingMatrices, matchingPrefab);

		for (int i = 1; i < offsets.Count; i++)
		{
			Matrix4x4 targetOffset = offsets[i];
			var targetMatrices = CreateMatchingList(modelMatrices, targetOffset);
			float elapsedTime = 0f;
			List<TransformData> startDatas = StartDatas(matchingCubes);
			List<TransformData> targetDatas = TargetDatas(matchingCubes, targetMatrices);
			while (elapsedTime < lerpDuration)
			{
				float lerpFactor = elapsedTime / lerpDuration;
				Debug.Log(elapsedTime);
				Debug.Log(lerpFactor);
				TransformCubes(matchingCubes, startDatas, targetDatas, lerpFactor);

				elapsedTime += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(1f);
		}
	}

	private List<TransformData> TargetDatas(List<GameObject> cubes, List<Matrix4x4> targetMatrices)
	{
		List<TransformData> transformDatas = new List<TransformData>();
		for (int i = 0; i < cubes.Count; i++)
		{
			GameObject cube = cubes[i];
			Vector3 targetPosition = targetMatrices[i].GetPosition();
			Quaternion targetRotation = Quaternion.LookRotation(targetMatrices[i].GetColumn(2), targetMatrices[i].GetColumn(1));
			Vector3 targetScale = new Vector3(
				targetMatrices[i].GetColumn(0).magnitude,
				targetMatrices[i].GetColumn(1).magnitude,
				targetMatrices[i].GetColumn(2).magnitude
			) * cubeSize;
			transformDatas.Add(new TransformData(targetPosition, targetRotation, targetScale));
		}
		return transformDatas;
	}
	private List<TransformData> StartDatas(List<GameObject> cubes)
	{
		List<TransformData> transformDatas = new List<TransformData>();
		for (int i = 0; i < cubes.Count; i++)
		{
			GameObject cube = cubes[i];
			transformDatas.Add(new TransformData(cube.transform.position, cube.transform.rotation, cube.transform.localScale));
		}
		return transformDatas;
	}
	private void TransformCubes(List<GameObject> cubes, List<TransformData> startDatas , List<TransformData> targetDatas, float lerpFactor)
	{
		for (int i = 0; i < cubes.Count; i++)
		{
			GameObject cube = cubes[i];
			cube.transform.position = Vector3.Lerp(startDatas[i]._position , targetDatas[i]._position, lerpFactor);
			cube.transform.rotation = Quaternion.Slerp(startDatas[i]._rotation, targetDatas[i]._rotation, lerpFactor);
			cube.transform.localScale = Vector3.Lerp(startDatas[i]._scale, targetDatas[i]._scale, lerpFactor);
		}
	}
	public class TransformData
	{
		public Vector3 _position { get; }
		public Quaternion _rotation { get; }
		public Vector3 _scale { get; }

		public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			_position = position;
			_rotation = rotation;
			_scale = scale;
		}
	}
}
