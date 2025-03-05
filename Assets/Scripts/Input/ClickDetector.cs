using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    public static Action<Candy> OnClickCandy;
    private bool _clickAble = true;

    private void OnEnable()
    {
        BoardManager.OnClickAble += SetClick;
        WinLoseManager.OnNotifyEndGame += DisableClick;
    }

    private void OnDisable()
    {
        BoardManager.OnClickAble -= SetClick;
        WinLoseManager.OnNotifyEndGame -= DisableClick;
    }

    private void Update()
    {
        if (_clickAble)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetectClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DetectClick(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            }
        }
    }

    private void DetectClick(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null)
        {
            OnClickCandy?.Invoke(hit.collider.GetComponent<Candy>());
        }
    }

    private void SetClick(bool clickAble)
    {
        _clickAble = clickAble;
    }

    private void DisableClick(bool value)
    {
        _clickAble = false;
    }
}