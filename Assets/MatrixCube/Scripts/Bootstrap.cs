using UnityEngine;
using System.Collections.Generic;
using System.IO;

// public class Bootstrap : MonoBehaviour
// {
// 	private void Start()
// 	{
// 	    MatrixLoader loader = new MatrixLoader();
// 	    MatrixVisualizer visualizer = GetComponent<MatrixVisualizer>();

// 	    string modelFilePath = Path.Combine(Application.dataPath, "model.json");
// 	    string spaceFilePath = Path.Combine(Application.dataPath, "space.json");
// 	    string offsetFilePath = Path.Combine(Application.dataPath, "offsets.json");

// 	    List<Matrix4x4> modelMatrices = loader.LoadMatricesFromJson(modelFilePath);
// 	    List<Matrix4x4> spaceMatrices = loader.LoadMatricesFromJson(spaceFilePath);
// 	    List<Matrix4x4> offsets = loader.LoadMatricesFromJson(offsetFilePath);

// 	    visualizer.Visualize(modelMatrices, spaceMatrices, offsets);
// 	}
// }
public class Bootstrap : MonoBehaviour
{
	private async void Start()
	{
		MatrixLoader loader = new MatrixLoader();
		ParallelMatrixMatcher matcher = new ParallelMatrixMatcher();
		MatrixVisualizer visualizer = GetComponent<MatrixVisualizer>();

		string modelFilePath = Path.Combine(Application.dataPath, "model.json");
		string spaceFilePath = Path.Combine(Application.dataPath, "space.json");
		string outputFilePath = Path.Combine(Application.dataPath, "offsets.json");

		List<Matrix4x4> modelMatrices = loader.LoadMatricesFromJson(modelFilePath);
		List<Matrix4x4> spaceMatrices = loader.LoadMatricesFromJson(spaceFilePath);
	 	List<Matrix4x4> offsets = await matcher.FindAllMatchingOffsetsAsync(modelMatrices, spaceMatrices);
		await loader.ExportOffsetsToJson(offsets, outputFilePath);

		visualizer.Visualize(modelMatrices, spaceMatrices, offsets);
	}
}


