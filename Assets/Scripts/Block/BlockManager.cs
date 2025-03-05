using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private Transform candyTank;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetBlock()
    {
        return Instantiate(blockPrefab, candyTank);
    }
}