//using DG.Tweening;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class LevelManager : MonoBehaviour
//{
//    public static LevelManager Instance;

//    public static Action<Candy.CandyType, int> OnIniCandy;
//    public static Action<int, List<int>, int[]> OnUpdateScore;

//    private float _duration = 3f;

//    private int _totalScore;
//    private int _maxScore;
//    private int[] _starArr = new int[3];
    
//    // Red, Green
//    private List<int> _targetCandyList = new List<int>();
//    internal List<Candy.CandyType> _candyTypeList = new List<Candy.CandyType>();

//    private bool _isWin;

//    [SerializeField] private GameObject _earnStarPos;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }

//        _isWin = false;
//        _totalScore = 0;
//        _maxScore = 300;
//        _starArr[0] = 60;
//        _starArr[1] = 150;
//        _starArr[2] = _maxScore;

//        _candyTypeList.Add(Candy.CandyType.Yellow);
//        _candyTypeList.Add(Candy.CandyType.Green);

//        _targetCandyList.Add(10);
//        _targetCandyList.Add(10);

//        StartCoroutine(WaitForScoreAvailable());
//    }

//    private IEnumerator WaitForScoreAvailable()
//    {
//        if (LevelUIManager.Instance == null)
//        {
//            yield return null;
//        }

//        for (int i = 0; i < _targetCandyList.Count; i++)
//        {
//            LevelUIManager.Instance.IniCandy(i, _candyTypeList[i], _targetCandyList[i]);
//        }
//    }

//    private void OnEnable()
//    {
//        BoardManager.OnDestroyCandy += OnListenerDestroyCandy;
//    }

//    private void OnListenerDestroyCandy(int count, int score, Candy.CandyType type)
//    {
//        for (int i = 0; i < _targetCandyList.Count; i++)
//        {
//            if (type == _candyTypeList[i])
//            {
//                _targetCandyList[i] = _targetCandyList[i] - count > 0 ? _targetCandyList[i] - count : 0;
//            }
//        }

//        _totalScore += score * count;

//        OnUpdateScore?.Invoke(_totalScore, _targetCandyList, _starArr);

//        _isWin = CheckWinningLevel();
//        if (_isWin)
//        {
//            StartCoroutine(WaitForPauseGame());
//        }
//    }

//    private IEnumerator WaitForPauseGame()
//    {
//        yield return new WaitForSeconds(3f);
//        Debug.Log("Win");
//        Time.timeScale = 0;
//    }

//    //private void EarnStarEffect()
//    //{
//    //    GameObject star = Resources.Load<GameObject>("Prefabs/Earn Star");
//    //    star.transform.position = _earnStarPos.transform.position;

//    //    star.transform.DOMove();
//    //}

//    private bool CheckWinningLevel()
//    {
//        foreach(int i in _targetCandyList)
//        {
//            if (i > 0) return false;
//        }
//        return true;
//    }
//}