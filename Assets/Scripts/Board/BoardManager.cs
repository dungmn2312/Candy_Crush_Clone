using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public static event Action<Candy> OnCandyCheck;
    public static event Action<bool> OnClickAble;

    private Board _board;
    internal int[,] _boardMatrix;
    internal Candy[,] _candyMatrix;

    private GameObject[] _candyObjects;

    private Vector3 tempVector3 = Vector3.zero;
    private Vector2Int tempVector2 = Vector2Int.zero;

    //private bool _isFromBegin = true;
    private int _startX = 0, _startY = 0;
    private float _maxTime = -1f;

    private void Awake()
    {
        Instance = this;

        _board = GetComponent<Board>();

        _boardMatrix = _board.GetMatrix();
        _candyMatrix = new Candy[_board.Width, _board.Height];

        StartCoroutine(WaitForPrefabsReady());
    }

    private void OnEnable()
    {
        NormalCandy.OnDestroyable += CheckStartXY;
    }

    private void OnDisable()
    {
        NormalCandy.OnDestroyable -= CheckStartXY;
    }

    private IEnumerator WaitForPrefabsReady()
    {
        while (CandyManager.Instance == null || BlockManager.Instance == null)
        {
            yield return null;
        }
        _candyObjects = CandyManager.Instance.candyPrefabs;

        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int x = 0; x < _boardMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < _boardMatrix.GetLength(1); y++)
            {
                tempVector3.x = x; tempVector3.y = y;

                if (y < _boardMatrix.GetLength(1) / 2)
                {
                    GameObject block = BlockManager.Instance.GetBlock();
                    block.transform.position = tempVector3;
                }

                Candy candy = CandyManager.Instance.GetCandyFromPool(_candyObjects[UnityEngine.Random.Range(0, 5)], tempVector3).GetComponent<Candy>();
                _candyMatrix[x, y] = candy;
                tempVector2.x = x; tempVector2.y = y;
                candy.Pos = tempVector2;
            }
        }
    }

    private int FindHighestY()
    {
        for (int x = 0; x < _boardMatrix.GetLength(0); x++)
        {
            for (int y = _boardMatrix.GetLength(1)/2 - 1; y > 0; y--)
            {
                if (_candyMatrix[x, y] == null)
                    return y;
            }
        }
        return 0;
    }
      
    public async void DropCandies()
    {
        float dropTime;
        await UniTask.WaitForSeconds(0.5f);
        OnClickAble?.Invoke(false);
        for (int x = 0; x < _boardMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < _boardMatrix.GetLength(1); y++)
            {
                if (_candyMatrix[x, y] == null)
                {
                    for (int k = y + 1; k < _boardMatrix.GetLength(1); k++)
                    {
                        if (_candyMatrix[x, k] != null)
                        {
                            _candyMatrix[x, y] = _candyMatrix[x, k];
                            _candyMatrix[x, k] = null;
                            tempVector2.x = x; tempVector2.y = y;
                            _candyMatrix[x, y].Pos = tempVector2;
                            tempVector3.x = x; tempVector3.y = y;

                            dropTime = (k - y) / 6f;
                            if (_maxTime < dropTime) _maxTime = dropTime;
                            _candyMatrix[x, y].transform.DOMove(tempVector3, dropTime)
                                .SetEase(Ease.InQuad);
                           
                            break;
                        }
                    }
                }
            }
        }

        RespawnCandy();
    }

    private async void RespawnCandy()
    {
        for (int x = 0; x < _boardMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < _boardMatrix.GetLength(1); y++)
            {
                if (_candyMatrix[x, y] == null)
                {
                    tempVector3.x = x; tempVector3.y = y;
                    Candy candy = CandyManager.Instance.GetCandyFromPool(_candyObjects[UnityEngine.Random.Range(0, 5)], tempVector3).GetComponent<Candy>();
                    _candyMatrix[x, y] = candy;
                    tempVector2.x = x; tempVector2.y = y;
                    candy.Pos = tempVector2;
                }
            }
        }

        await UniTask.WaitForSeconds(_maxTime + 0.1f);
        _maxTime = -1f;
        ScanAfterDrop(0, 0);
    }

    public bool IsNullCell(int x, int y)
    {
        return _candyMatrix[x, y] == null;
    }

    public void SetNullCell(int x, int y)
    {
        _candyMatrix[x, y] = null;
    }

    public void ScanAfterDrop(int startX, int startY)
    {
        Candy candy = _candyMatrix[startX, startY];
        if (candy.ability == Candy.CandyAbility.None)
            OnCandyCheck?.Invoke(candy);
        else
        {
            SetStartXY();
        }

    }

    private void CheckStartXY(bool isFromBegin)
    {
        if (!isFromBegin)
        {
            SetStartXY();
        }
        else
        {
            _startX = 0; _startY = 0;
        }
    }

    private void SetStartXY()
    {
        if (_startX == _candyMatrix.GetLength(0) - 1 && _startY == _candyMatrix.GetLength(1) / 2 - 1)
        {
            _startX = 0; _startY = 0;
            OnClickAble?.Invoke(true);
            return;
        }
        if (_startY == _candyMatrix.GetLength(1) / 2 - 1)
        {
            _startY = 0; ++_startX;
        }
        else
        {
            ++_startY;
        }
        ScanAfterDrop(_startX, _startY);
    }
}