using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AddressableAssets.HostingServices;
using UnityEngine;
using UnityEngine.UI;

public class TargetManager : MonoBehaviour
{
    public static event Action<bool> OnNotifyDoneTarget;
    //public static event Action<Candy> OnNotifyTargetCandy;

    public List<string> targetCandiesName;
    public List<Image> targetCandiesImage;
    public List<int> targetCandiesCount;
    public List<int> candiesCountPerPhase;
    public bool[] checkDone;

    public List<TextMeshProUGUI> targetLeftTexts;


    private void Awake()
    {
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
        candiesCountPerPhase.Add(0);
        candiesCountPerPhase.Add(0);
        checkDone = new bool[targetCandiesCount.Count];
        SetUI();
    }

    private void SetUI()
    {
        for (int i = 0; i < targetCandiesName.Count; i++)
        {
            //Debug.Log(targetCandiesName.Count);
            //Debug.Log(targetCandiesImage[i]);
            //targetCandiesImage[i].sprite = Resources.Load<Sprite>($"Candy Item/Sprite/{targetCandiesName[i]}");
            targetCandiesImage[i].sprite = AddressableController.Instance.LoadAssetByName<Sprite>($"Candy Item/Sprite/{targetCandiesName[i]}");
            SetTargetLeftUI(targetLeftTexts[i], targetCandiesCount[i].ToString());
        }
    }

    private void CaculateTargetCandies(List<Candy> candies)
    {
        for (int i = 0; i < targetCandiesName.Count; i++)
        {
            foreach(Candy candy in candies)
            {
                if (candy.type.ToString().Equals(targetCandiesName[i]))
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
        for (int i = 0; i < targetCandiesCount.Count; i++)
        {
            int value = targetCandiesCount[i] - candiesCountPerPhase[i] > 0 ? targetCandiesCount[i] - candiesCountPerPhase[i] : 0;
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
        for (int i = 0; i < targetCandiesCount.Count; i++)
        {
            if (candiesCountPerPhase[i] >= targetCandiesCount[i])
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