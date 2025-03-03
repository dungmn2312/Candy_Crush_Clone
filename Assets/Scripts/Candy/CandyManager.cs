using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CandyManager : MonoBehaviour
{
    public static CandyManager Instance;

    internal GameObject[] candyPrefabs;
    internal GameObject[] candyStripPrefabs;
    internal GameObject[] candyBombPrefabs;
    internal GameObject[] candyRainbowPrefabs;

    private int _candyPoolCount = 30;
    private int _specialCandyPoolCount = 5;

    internal List<Candy> rainBowCheck = new List<Candy>();

    private void Awake()
    {
        LoadCandyPrefabs();

        Instance = this;
    }

    private void LoadCandyPrefabs()
    {
        candyPrefabs = Resources.LoadAll<GameObject>("Candy Item/Item");
        candyStripPrefabs = Resources.LoadAll<GameObject>("Candy Item/Special Item 4");
        candyBombPrefabs = Resources.LoadAll<GameObject>("Candy Item/Special Item 5");
        candyRainbowPrefabs = Resources.LoadAll<GameObject>("Candy Item/Special Item 6");

        GenerateCandyPool();
    }
    private void GenerateCandyPool()
    {
        for (int i = 0; i < candyPrefabs.Length; i++)
        {
            SimplePool.PoolPreLoad(candyPrefabs[i], _candyPoolCount);
        }

        for (int i = 0; i < candyStripPrefabs.Length; i++)
        {
            SimplePool.PoolPreLoad(candyStripPrefabs[i], _specialCandyPoolCount);
        }

        for (int i = 0; i < candyBombPrefabs.Length; i++)
        {
            SimplePool.PoolPreLoad(candyBombPrefabs[i], _specialCandyPoolCount);
        }

        for (int i = 0; i < candyRainbowPrefabs.Length; i++)
        {
            SimplePool.PoolPreLoad(candyRainbowPrefabs[i], _specialCandyPoolCount);
        }
    }

    public GameObject GetCandyFromPool(GameObject candyRandom, Vector3 pos)
    {
        GameObject candy = SimplePool.Spawn(candyRandom, pos, Quaternion.identity);
        candy.transform.localScale = Vector3.one;
        return candy;
    }

    public void GenerateSpecialCandy(Candy candy, int value)
    {
        if (value == 4)
        {
            GetSpecialCandyFromPool(candy, candyStripPrefabs, UnityEngine.Random.Range(0, 2));
        }
        else if (value == 5)
        {
            GetSpecialCandyFromPool(candy, candyBombPrefabs, 0);
        }
        else if (value > 5)
        {
            GetSpecialCandyFromPool(candy, candyRainbowPrefabs, 0);
        }
    }

    private void GetSpecialCandyFromPool(Candy candy, GameObject[] specialCandies, int offset)
    {
        int index = 0;
        for (int i = 0; i < specialCandies.Length; i++)
        {
            if (candy.type == specialCandies[i].GetComponent<Candy>().type)
            {
                index = i;
                break;
            }
        }
        Candy specialCandy = GetCandyFromPool(specialCandies[index + offset], new Vector3(candy.Pos.x, candy.Pos.y, 0)).GetComponent<Candy>();
        BoardManager.Instance._candyMatrix[candy.Pos.x, candy.Pos.y] = specialCandy;
        specialCandy.Pos = candy.Pos;
    }

}