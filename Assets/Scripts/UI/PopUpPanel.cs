using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : BaseBox
{
    //[SerializeField] private OpenButton btnOK;
    //[SerializeField] private CloseButton btnCancel;
    //[SerializeField] private GameObject subPanel;

    protected override void OnEnable()
    {
        base.OnEnable();
        //GetComponent<CanvasGroup>().interactable = false;
    }
}