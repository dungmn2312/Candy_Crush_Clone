using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadLevel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    public string sceneName;
    private string suffixName = ".unity";
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable()
    {
        sceneName = "Assets/Scenes/Level ";
        playButton.onClick.AddListener(PlayButtonClick);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(PlayButtonClick);
    }


    public void PlayButtonClick()
    {
        Debug.Log(sceneName + levelText.text + suffixName);
        GameManager.Instance.currentLevel = int.Parse(levelText.text);
        GameManager.Instance.LoadScene(sceneName + levelText.text + suffixName);
    }
}