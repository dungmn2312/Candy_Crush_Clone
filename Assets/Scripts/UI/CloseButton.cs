using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private PopUpPanel panel;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(ClosePanel);
    }

    public void ClosePanel()
    {
        panel.Close();
    }
}
