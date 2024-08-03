using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MatrixProcessing 
{
	public string modelFileName = "model.json";
	public string spaceFileName = "space.json";
	public string outputFileName = "offsets.json";

	private List<Matrix4x4> modelMatrices;
	private List<Matrix4x4> spaceMatrices;

	public async void Processing()
	{
		Debug.Log("StartWork");
		MatrixLoader loader = new MatrixLoader();

		string modelFilePath = Path.Combine(Application.dataPath, modelFileName);
		string spaceFilePath = Path.Combine(Application.dataPath, spaceFileName);
		string outputFilePath = Path.Combine(Application.dataPath, outputFileName);

		modelMatrices = loader.LoadMatricesFromJson(modelFilePath);
		spaceMatrices = loader.LoadMatricesFromJson(spaceFilePath);

		var matcher = new ParallelMatrixMatcher();
		
		List<Matrix4x4> allMatchingOffsets = await matcher.FindAllMatchingOffsetsAsync(modelMatrices, spaceMatrices);

		Debug.Log("All Matching Offsets Found: " + allMatchingOffsets.Count);

		loader.ExportOffsetsToJson(allMatchingOffsets, outputFilePath);
	}
}
