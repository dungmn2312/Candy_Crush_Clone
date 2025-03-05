using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CandyController : MonoBehaviour, IGetClick
{
    public static event Action<List<Candy>> OnNotifyScore;
    public static event Action<List<Candy>> OnNotifyTargetCandies;
    public static event Action<bool> OnNotifyConfirmClick;
    public static event Action<bool> OnNotifyClick;

    private Vector3 _targetScale = new Vector3(1.2f, 1.2f, 0);
    private Vector3 _rotateZ = new Vector3(0, 0, 360);
    private float _duration = 0.5f, _slowDuration = 3f;

    internal List<Candy> destroyedCandies = new List<Candy>();

    private void OnEnable()
    {
        ClickDetector.OnClickCandy += GetClick;
    }

    private void OnDisable()
    {
        ClickDetector.OnClickCandy -= GetClick;
    }

    public void GetClick(Candy candy)
    {
        if (candy == gameObject.GetComponent<Candy>())
        {
            OnNotifyClick?.Invoke(true);
            candy.isDestroyMain = true;
            candy.BeforeDestroy();
        }
    }

    public void ClearDestroyedList()
    {
        destroyedCandies.Clear();
    }

    public bool IsSameType(Candy.CandyType type, Candy candy)
    {
        return candy.type == type;
    }

    public bool IsContainCandy(Candy candy)
    {
        return destroyedCandies.Contains(candy);
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < BoardManager.Instance._boardMatrix.GetLength(0) &&
               y >= 0 && y < BoardManager.Instance._boardMatrix.GetLength(1) / 2;
    }

    public void AddToDestroyedCandies(Candy candy)
    {
        destroyedCandies.Add(candy);
    }

    public void MoveSelfToFirst(Candy candy)
    {
        if (destroyedCandies.Contains(candy))
        {
            destroyedCandies.Remove(candy);
            destroyedCandies.Insert(0, candy);
        }
    }

    public void DOScaleGameObject(Candy candy)
    {
        transform.GetChild(0).DOScale(_targetScale, 0.1f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.GetChild(0).DOScale(Vector3.zero, 0.7f).SetEase(Ease.InCubic)
                    .OnComplete(() => { SimplePool.Despawn(candy.gameObject); });
            });
    }

    internal void RotateCandy(Candy candy)
    {
        transform.GetChild(0).rotation = Quaternion.identity;
        transform.GetChild(0).DORotate(_rotateZ, _slowDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
    }

    internal void ScaleCandy(Candy candy)
    {
        transform.GetChild(0).localScale = Vector3.one;
        transform.GetChild(0).DOScale(_targetScale, _duration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
    }

    private int CaculateScore(List<Candy> candies)
    {
        int score = 0;
        foreach (Candy candy in candies)
        {
            score += candy.score;
        }

        return score;
    }

    private void InvokeScore()
    {
        OnNotifyScore?.Invoke(destroyedCandies);
    }

    private void InvokeTargetCandies()
    {
        OnNotifyTargetCandies?.Invoke(destroyedCandies);
    }

    public void InvokeCandiesAfterDestroy()
    {
        InvokeConfirmClick();
        InvokeScore();
        InvokeTargetCandies();
    }

    private void InvokeConfirmClick()
    {
        OnNotifyConfirmClick?.Invoke(true);
    }
}