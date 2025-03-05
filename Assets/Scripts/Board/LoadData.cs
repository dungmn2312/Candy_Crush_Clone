using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LoadData : MonoBehaviour
{
    public static LoadData Instance;

    private string _path, _fileName = "matrix.json";
    private BoardManager _boardManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _path = Path.Combine(Application.streamingAssetsPath, _fileName);
        //TestData();
    }

    public int[][] LoadCandyMatrixData()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            MatrixData matrixData = JsonConvert.DeserializeObject<MatrixData>(json);
            //Debug.Log(matrixData.matrix[0].Length + " " + matrixData.matrix.Length);
            return matrixData.matrix;
        }
        return null;
    }

    private void TestData()
    {
        if (File.Exists(_path))
        {
            string json = File.ReadAllText(_path);
            Debug.Log(json);
            MatrixData data = JsonConvert.DeserializeObject<MatrixData>(json);

            Debug.Log(data.matrix[0][0]);
            Debug.Log(data.matrix[0][1]);
            Debug.Log(data.matrix[0][2]);
            Debug.Log(data.matrix[0][3]);
            Debug.Log(data.matrix[0][4]);
        }
    }
}