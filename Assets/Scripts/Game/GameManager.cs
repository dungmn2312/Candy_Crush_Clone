using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    internal string mainMenuScene = "MainMenu";
    internal string levelScene_1 = "Level 1";
    internal string mapScene = "Map";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Test load wait
        WaitForLoadMainMenu();
    }

    private async void WaitForLoadMainMenu()
    {
        await UniTask.WaitForSeconds(3f);
        LoadScene(mainMenuScene);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}