using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private string _matrixPath, _levelPath; 
    private string _matrixFileName = "matrix.json", _levelFileName = "levels.json";

    internal Dictionary<int, LevelData>  levelDict = new Dictionary<int, LevelData>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _matrixPath = Path.Combine(Application.streamingAssetsPath, _matrixFileName);
        _levelPath = Path.Combine(Application.streamingAssetsPath, _levelFileName);
    }

    public int[][] LoadCandyMatrixData()
    {
        if (File.Exists(_matrixPath))
        {
            string json = File.ReadAllText(_matrixPath);
            MatrixData matrixData = JsonConvert.DeserializeObject<MatrixData>(json);
            return matrixData.matrix;
        }
        return null;
    }
    
    public void LoadLevelData()
    {
        if (File.Exists(_levelPath))
        {
            string json = File.ReadAllText(_levelPath);
            LevelList levelList = JsonConvert.DeserializeObject<LevelList>(json);
            foreach (LevelData levelData in levelList.levels)
            {
                levelDict[levelData.level] = levelData;
                //Debug.Log($"Level: {levelData.level} - Stars: {levelData.stars} - Targets: {levelData.targets}");
            }
        }
    }

    public void WriteLevelData(bool[] starsCheck, int levelNumber)
    {
        string json = File.ReadAllText(_levelPath);
        LevelList levelList = JsonConvert.DeserializeObject<LevelList>(json);

        levelList.levels[levelNumber - 1].stars = starsCheck;

        string updatedJson = JsonConvert.SerializeObject(levelList, Formatting.Indented);

        File.WriteAllText(_levelPath, updatedJson);
    }

    //public void WriteLevelData()
    //{

    //}

    public LevelData GetLevelData(int levelID)
    {
        return levelDict[levelID];
    }
}