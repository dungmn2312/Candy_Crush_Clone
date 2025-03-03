using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StripCandy : Candy
{
    private CandyController candyController;

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
            if (c != this)  c.isDestroyMain = false;
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
        transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
        candyController.DOScaleGameObject(this);
    }

    public override void AfterDestroy()
    {
        candyController.ClearDestroyedList();
        BoardManager.Instance.DropCandies();
    }

    private void FindCandies()
    {
        if (ability == CandyAbility.StripHorizontal)
        {
            for (int i = 0; i < BoardManager.Instance._candyMatrix.GetLength(0); i++)
            {
                AddToDestroyedCandies(i, Pos.y);
            }
        }
        else
        {
            for (int i = 0; i < BoardManager.Instance._candyMatrix.GetLength(1) / 2; i++)
            {
                AddToDestroyedCandies(Pos.x, i);
            }
        }
    }

    private void AddToDestroyedCandies(int x, int y)
    {
        if (BoardManager.Instance.IsNullCell(x, y)) return;
        Candy candy = BoardManager.Instance._candyMatrix[x, y];
        candyController.AddToDestroyedCandies(candy);
    }
}