using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	private void Start()
	{
		MatrixProcessing matrixProcessing= new();
		matrixProcessing.Processing();
	}
}
