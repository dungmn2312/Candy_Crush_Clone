using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    public static event Action<bool> OnNotifyEndGame;

    public bool isDoneTarget;
    public bool isEmptyClick;

    [SerializeField] private TextMeshProUGUI clickLeftText;
    [SerializeField] private int clickLeftCount;
    private bool _tempClick;
    private bool _isCheckedEnd;
    private bool _isDropDone;

    private void Start()
    {
        SetClickLeftUI();
    }

    private void OnEnable()
    {
        TargetManager.OnNotifyDoneTarget += DoneTarget;
        CandyController.OnNotifyClick += SetTempClick; 
        CandyController.OnNotifyConfirmClick += CheckClickLeft;
        BoardManager.OnDoneDrop += DoneDrop;
    }

    private void OnDisable()
    {
        TargetManager.OnNotifyDoneTarget -= DoneTarget;
        CandyController.OnNotifyClick -= SetTempClick;
        CandyController.OnNotifyConfirmClick -= CheckClickLeft;
        BoardManager.OnDoneDrop -= DoneDrop;
    }

    private void DoneTarget(bool isDone)
    {
        isDoneTarget = isDone;
    }

    private void DoneDrop()
    {
        _isDropDone = true;
        CheckWinLose();
    }

    private void SetTempClick(bool value)
    {
        _tempClick = value;
    }

    private void CheckClickLeft(bool isClick)
    {
        if (isClick && _tempClick)
        {
            clickLeftCount--;
            _tempClick = false;
        }

        SetClickLeftUI();
        
        if (clickLeftCount <= 0) isEmptyClick = true;
    }

    private void CheckWinLose()
    {
        if (IsWin())
        {
            OnNotifyEndGame?.Invoke(true);
        }
        else if (IsLose())
        {
            OnNotifyEndGame?.Invoke(false);
        }
    }

    private bool IsWin()
    {
        return isDoneTarget && clickLeftCount >= 0 && _isDropDone;
    }

    private bool IsLose()
    {
        return !isDoneTarget && clickLeftCount <= 0;
    }

    private void SetClickLeftUI()
    {
        clickLeftText.SetText($"{clickLeftCount}");
    }
}