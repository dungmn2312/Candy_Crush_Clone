using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCandy : Candy
{
    private CandyController candyController;
    public static event Action<Vector3> OnPlayExplosionEffect;

    private void Awake()
    {
        candyController = GetComponent<CandyController>();
    }

    private void OnEnable()
    {
        candyController.ScaleCandy(this);
    }

    private void OnDisable()
    {
        transform.GetChild(0).DOKill(true);
    }

    public override void BeforeDestroy()
    {
        List<Candy> candyCheck = CandyManager.Instance.rainBowCheck;
        if (candyCheck.Count == 1 && candyCheck[0].ability == CandyAbility.Rainbow)
        {
            candyCheck.Add(this);
            candyCheck[0].isDestroyMain = true;
            candyCheck[0].BeforeDestroy();
        }
        else
        {
            FindCandies();
            WhileDestroy();
        }
    }

    public override void WhileDestroy()
    {
        candyController.MoveSelfToFirst(this);
        foreach (Candy c in candyController.destroyedCandies)
        {
            if (c != this) c.isDestroyMain = false;
            c.DestroyCandy(this);
        }

        if (isDestroyMain)  AfterDestroy();
        else candyController.ClearDestroyedList();
    }

    public override void DestroyCandy(Candy candy)
    {
        if (BoardManager.Instance.IsNullCell(Pos.x, Pos.y)) return;
        if (candyController.destroyedCandies.Count == 0 && ability != CandyAbility.None)
        {
            BeforeDestroy();
        }

        BoardManager.Instance.SetNullCell(Pos.x, Pos.y);
        if (candy == this) OnPlayExplosionEffect?.Invoke(transform.position);
        else transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
        candyController.DOScaleGameObject(this);
    }

    public override void AfterDestroy()
    {
        candyController.InvokeCandiesAfterDestroy();
        candyController.ClearDestroyedList();
        BoardManager.Instance.DropCandies();
    }

    private void FindCandies()
    {
        (int, int)[] directions = { (-1, 1), (0, 1), (1, 1), (-1, 0), (0, 0), (1, 0), (-1, -1), (0, -1), (1, -1) };
        foreach ((int dx, int dy) in directions)
        {
            int x = Pos.x + dx, y = Pos.y + dy;
            if (!candyController.IsValidPosition(x, y) || BoardManager.Instance.IsNullCell(x, y)) continue;
            Candy candy = BoardManager.Instance._candyMatrix[x, y];
            candyController.AddToDestroyedCandies(candy);
        }
    }
}