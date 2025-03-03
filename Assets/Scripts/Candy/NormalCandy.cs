using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NormalCandy : Candy
{
    public static event Action<bool> OnDestroyable;
    private CandyController candyController;
    bool isDestroyByDrop = false;

    private void Awake()
    {
        candyController = GetComponent<CandyController>();
    }

    private void OnEnable()
    {
        transform.GetChild(0).localScale = Vector3.one;

        BoardManager.OnCandyCheck += StartForDestroyByDrop;
    }

    private void OnDisable()
    {
        BoardManager.OnCandyCheck -= StartForDestroyByDrop;
    }

    private void StartForDestroyByDrop(Candy candy)
    {
        if (candy == this)
        {
            isDestroyByDrop = true;
            FindRelativeCandies(Pos.x, Pos.y);
            WhileDestroy();
        }
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
            FindRelativeCandies(Pos.x, Pos.y);
            WhileDestroy();
        }

    }

    public override void WhileDestroy()
    {
        if (candyController.destroyedCandies.Count < 4 && isDestroyByDrop)
        {
            candyController.ClearDestroyedList();
            isDestroyByDrop = false;
            OnDestroyable?.Invoke(false);
            return;
        }
        if (candyController.destroyedCandies.Count < 2)
        {
            candyController.ClearDestroyedList();
            return;
        }

        candyController.MoveSelfToFirst(this);
        foreach (Candy c in candyController.destroyedCandies)
        {
            if (c != this) c.isDestroyMain = false;
            c.DestroyCandy(this);
        }

        AfterDestroy();
    }

    public override void DestroyCandy(Candy candy)
    {
        if (BoardManager.Instance.IsNullCell(Pos.x, Pos.y)) return;

        BoardManager.Instance.SetNullCell(Pos.x, Pos.y);
        
        if (candy.ability != CandyAbility.None)
            transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
        candyController.DOScaleGameObject(this);
    }

    public override void AfterDestroy()
    {
        if (isDestroyByDrop)
        {
            isDestroyByDrop = false;
            OnDestroyable?.Invoke(true);
        }

        CandyManager.Instance.GenerateSpecialCandy(this, candyController.destroyedCandies.Count);
       
        candyController.ClearDestroyedList();
        BoardManager.Instance.DropCandies();
    }

    private void FindRelativeCandies(int x, int y)
    {
        if (!candyController.IsValidPosition(x, y)) return;
        if (BoardManager.Instance.IsNullCell(x, y)) return;

        Candy candy = BoardManager.Instance._candyMatrix[x, y];
        if (candyController.IsContainCandy(candy) || !candyController.IsSameType(candy.type, this) || candy.ability == CandyAbility.Rainbow) return;

        candyController.AddToDestroyedCandies(candy);

        FindRelativeCandies(x - 1, y);
        FindRelativeCandies(x + 1, y);
        FindRelativeCandies(x, y - 1);
        FindRelativeCandies(x, y + 1);
    }
}