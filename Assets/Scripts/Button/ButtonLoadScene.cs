using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadScene : MonoBehaviour
{
    [SerializeField] private Button playButton;
    public string sceneName;

    private void OnEnable()
    {
        playButton.onClick.AddListener(PlayButtonClick);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(PlayButtonClick);
    }

    private void Start()
    {
        sceneName = gameObject.name;
    }

    public void PlayButtonClick()
    {
        GameManager.Instance.LoadScene(sceneName);
    }
}