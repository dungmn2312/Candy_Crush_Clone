using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenButton : MonoBehaviour
{
    [SerializeField] private Button openButton;
    [SerializeField] private PopUpPanel panel;

    private void OnEnable()
    {
        openButton.onClick.AddListener(OpenPanel);
    }

    private void OnDisable()
    {
        openButton.onClick.RemoveListener(OpenPanel);
    }

    public void OpenPanel()
    {
        panel.Show();
    }
}