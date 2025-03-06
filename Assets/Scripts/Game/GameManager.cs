using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    internal string mainMenuScene = "Assets/Scenes/MainMenu.unity";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        AddressableController.Instance.Init();
    }

    private void Start()
    {
        WaitForLoadMainMenu();
    }

    private async void WaitForLoadMainMenu()
    {
        await UniTask.WaitUntil(() => AddressableController.Instance.isInited == true);

        LoadScene(mainMenuScene);
    }

    public void LoadScene(string sceneName)
    {
        AddressableController.Instance.LoadSceneByName(sceneName, true);
    }

    public void LoadAsset(string assetName)
    {

    }
}