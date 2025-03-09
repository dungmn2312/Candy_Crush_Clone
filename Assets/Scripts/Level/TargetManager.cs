using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.AddressableAssets.HostingServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public static event Action<bool> OnNotifyDoneTarget;
    //public static event Action<Candy> OnNotifyTargetCandy;

    public List<Image> targetCandiesImage;

    public List<int> candiesCountPerPhase;
    public bool[] checkDone;

    public List<TextMeshProUGUI> targetLeftTexts;

    internal Dictionary<string, int> targetDict;


    private void Awake()
    {
        int levelNumber = GameManager.Instance.currentLevel;
        targetDict = DataManager.Instance.levelDict[levelNumber].targets;
        Setup();
    }

    private void OnEnable()
    {
        CandyController.OnNotifyTargetCandies += CaculateTargetCandies;
    }

    private void OnDisable()
    {
        CandyController.OnNotifyTargetCandies -= CaculateTargetCandies;
    }

    private void Setup()
    {
        for (int i = 0; i < targetDict.Count; i++)
        {
            candiesCountPerPhase.Add(0);
        }

        checkDone = new bool[targetDict.Count];
        SetUI();
    }

    private void SetUI()
    {
        for (int i = 0; i < targetDict.Count; i++)
        {
            string key = targetDict.Keys.ElementAt(i);
            targetCandiesImage[i].sprite = AddressableController.Instance.LoadAssetByName<Sprite>($"Candy Item/Sprite/{key}");
            SetTargetLeftUI(targetLeftTexts[i], targetDict[key].ToString());
        }
    }

    private void CaculateTargetCandies(List<Candy> candies)
    {
        for (int i = 0; i < targetDict.Count; i++)
        {
            foreach(Candy candy in candies)
            {
                if (candy.type.ToString().Equals(targetDict.Keys.ElementAt(i)))
                {
                    candiesCountPerPhase[i]++;
                    //OnNotifyTargetCandy?.Invoke(candy);
                }
            }
        }

        UpdateTargetLeftUI();
        CheckDoneTarget();
    }

    private void UpdateTargetLeftUI()
    {
        for (int i = 0; i < targetDict.Count; i++)
        {
            string key = targetDict.Keys.ElementAt(i);
            int value = targetDict[key] - candiesCountPerPhase[i] > 0 ? targetDict[key] - candiesCountPerPhase[i] : 0;
            targetLeftTexts[i].SetText(value.ToString());
        }
    }

    private void SetTargetLeftUI(TextMeshProUGUI text, string value)
    {
        text.SetText(value);
    }

    private void CheckDoneTarget()
    {
        //Debug.Log(targetCandiesCount.Count);
        for (int i = 0; i < targetDict.Count; i++)
        {
            string key = targetDict.Keys.ElementAt(i);
            if (candiesCountPerPhase[i] >= targetDict[key])
            {
                if (checkDone[i] == false) checkDone[i] = true;
            }
        }

        for (int i = 0; i < checkDone.Length; i++)
        {
            if (checkDone[i] == false)
                return;
        }

        OnNotifyDoneTarget?.Invoke(true);
    }
}