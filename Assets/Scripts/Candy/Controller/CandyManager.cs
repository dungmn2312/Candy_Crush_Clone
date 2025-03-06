using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CandyManager : MonoBehaviour
{
    public static CandyManager Instance;

    [SerializeField] private Transform candyTank;

    //internal GameObject[] candyPrefabs;
    //internal GameObject[] candyStripPrefabs;
    //internal GameObject[] candyBombPrefabs;
    //internal GameObject[] candyRainbowPrefabs;

    internal List<GameObject> candyPrefabs = new List<GameObject>();
    internal List<GameObject> candyStripPrefabs = new List<GameObject>();
    internal List<GameObject> candyBombPrefabs = new List<GameObject>();
    internal List<GameObject> candyRainbowPrefabs = new List<GameObject>();

    private int _candyPoolCount = 50;
    private int _specialCandyPoolCount = 5;

    internal List<Candy> rainBowCheck = new List<Candy>();

    private async void Awake()
    {
        await LoadCandyPrefabs();

        Instance = this;
    }

    private async UniTask LoadCandyPrefabs()
    {
        var normalTask = LoadCandyAsync("normalCandy", candyPrefabs);
        var stripTask = LoadCandyAsync("stripCandy", candyStripPrefabs);
        var bombTask = LoadCandyAsync("bombCandy", candyBombPrefabs);
        var rainbowTask = LoadCandyAsync("rainbowCandy", candyRainbowPrefabs);

        await UniTask.WhenAll(
            normalTask,
            stripTask,
            bombTask,
            rainbowTask
        );
        for (int i = 0; i <  candyPrefabs.Count; i++)
        {
            Debug.Log(candyPrefabs[i].name + " " + i);
        }
        //Debug.Log("All Candy Prefabs Loaded!");
        GenerateCandyPool();
    }

    private async UniTask LoadCandyAsync(string label, List<GameObject> targetList)
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(label, obj =>
        {
            targetList.Add(obj);
        });

        await handle.ToUniTask();

        //if (handle.Status == AsyncOperationStatus.Succeeded)
        //{
        //    Debug.Log($"Loaded {label}: {targetList.Count} items.");
        //}
        //else
        //{
        //    Debug.LogError($"Failed to load {label}!");
        //}
    }

    private void GenerateCandyPool()
    {
        for (int i = 0; i < candyPrefabs.Count; i++)
        {
            SimplePool.PoolPreLoad(candyPrefabs[i], _candyPoolCount, candyTank);
        }

        for (int i = 0; i < candyStripPrefabs.Count; i++)
        {
            SimplePool.PoolPreLoad(candyStripPrefabs[i], _specialCandyPoolCount, candyTank);
        }

        for (int i = 0; i < candyBombPrefabs.Count; i++)
        {
            SimplePool.PoolPreLoad(candyBombPrefabs[i], _specialCandyPoolCount, candyTank);
        }

        for (int i = 0; i < candyRainbowPrefabs.Count; i++)
        {
            SimplePool.PoolPreLoad(candyRainbowPrefabs[i], _specialCandyPoolCount, candyTank);
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

    private void GetSpecialCandyFromPool(Candy candy, List<GameObject> specialCandies, int offset)
    {
        int index = 0;
        for (int i = 0; i < specialCandies.Count; i++)
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