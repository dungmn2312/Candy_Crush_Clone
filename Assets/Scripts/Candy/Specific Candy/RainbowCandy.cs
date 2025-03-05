using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowCandy : Candy
{
    private CandyController candyController;
    private List<Candy> candyCheck = new List<Candy>();

    private void Awake()
    {
        candyController = GetComponent<CandyController>();
    }

    private void OnEnable()
    {
        transform.GetChild(0).localScale = Vector3.one;
        type = CandyType.None;
        candyController.RotateCandy(this);
    }

    private void OnDisable()
    {
        transform.GetChild(0).DOKill(true);
    }

    public override void BeforeDestroy()
    {
        if (!CheckRainbowList()) return;

        FindCandies();

        WhileDestroy();
    }

    private bool CheckRainbowList()
    {
        List<Candy> candyCheck = CandyManager.Instance.rainBowCheck;
        if (candyCheck.Count == 0)
        {
            if (type == CandyType.None)
            {
                candyCheck.Add(this);
                transform.GetChild(0).DOKill(true);
                candyController.ScaleCandy(this);
            }
            else
            {
                return true;
            }
        }
        else if (candyCheck.Count == 1)
        {
            if (candyCheck[0] == this)
            {
                candyCheck.Remove(this);
                transform.GetChild(0).DOKill(true);
                candyController.RotateCandy(this);
            }
            else
            {
                candyCheck.Add(this);
                return true;
            }
        }
        else if (candyCheck.Count == 2)
        {
            if (candyCheck[1].ability != CandyAbility.Rainbow)
            {
                type = candyCheck[1].type;
            }
            return true;
        }
        return false;
    }

    public override void WhileDestroy()
    {
        candyController.MoveSelfToFirst(this);
        foreach (Candy c in candyController.destroyedCandies)
        {
            if (c != this) c.isDestroyMain = false;
            c.DestroyCandy(this);
        }

        if (isDestroyMain) AfterDestroy();
        else
        {
            candyController.ClearDestroyedList();
            CandyManager.Instance.rainBowCheck.Clear();
        }
    }

    public override void DestroyCandy(Candy candy)
    {
        if (BoardManager.Instance.IsNullCell(Pos.x, Pos.y)) return;
        if (candyController.destroyedCandies.Count == 0 && candy.ability != CandyAbility.None)
        {
            type = candy.type;
            BeforeDestroy();
        }

        BoardManager.Instance.SetNullCell(Pos.x, Pos.y);
        transform.GetChild(transform.childCount - 1).GetComponent<ParticleSystem>().Play();
        candyController.DOScaleGameObject(this);
    }

    public override void AfterDestroy()
    {
        candyController.InvokeCandiesAfterDestroy();
        candyController.ClearDestroyedList();
        BoardManager.Instance.DropCandies();

        CandyManager.Instance.rainBowCheck.Clear();
    }

    private void FindCandies()
    {
        List<Candy> candies = CandyManager.Instance.rainBowCheck;

        for (int x = 0; x < BoardManager.Instance._boardMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < BoardManager.Instance._boardMatrix.GetLength(1) / 2; y++)
            {
                if (BoardManager.Instance.IsNullCell(x, y)) continue;

                Candy candy = BoardManager.Instance._candyMatrix[x, y];
                if (candies.Count > 1)
                {
                    if (candies[1].ability != CandyAbility.Rainbow)
                    {
                        if (candy.type != type) continue;
                    }
                }
                else
                {
                    if (candy.type != type) continue;
                }
                candyController.AddToDestroyedCandies(candy);
            }
        }
    }
}