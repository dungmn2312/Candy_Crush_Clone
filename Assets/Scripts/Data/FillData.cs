using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class FillData : MonoBehaviour
{
    private int levelNumber;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image[] stars;
    [SerializeField] private Image[] targets;
    [SerializeField] private TextMeshProUGUI[] targetsCount;

    //private List<GameObject> spritesCandy = new List<GameObject>();

    private Dictionary<int, LevelData> _levelDict;

    private async void Awake()
    {
        await UniTask.WaitUntil(() => DataManager.Instance != null);

        DataManager.Instance.LoadLevelData();

        _levelDict = DataManager.Instance.levelDict;
    }

    //private async UniTask LoadSpritesCandy()
    //{
    //    var handle = Addressables.LoadAssetsAsync<GameObject>("spriteCandy", obj =>
    //    {
    //        spritesCandy.Add(obj);
    //    });

    //    await handle.ToUniTask();
    //}

    public async void FillDataToPanel(string levelNumberText)
    {
        await UniTask.WaitForSeconds(0.1f);

        levelNumber = int.Parse(levelNumberText);

        SetLevelText();
        SetTarget();
        SetStarOn();
    }

    private void SetLevelText()
    {
        levelText.SetText(levelNumber.ToString());
    }

    private async void SetStarOn()
    {
        for (int i = 0; i < 3; i++)
        {
            await UniTask.WaitForSeconds(0.2f);
            stars[i].gameObject.SetActive(_levelDict[levelNumber].stars[i]);
        }
    }

    private void SetTarget()
    {
        for (int i = 0; i < _levelDict[levelNumber].targets.Count; i++)
        {
            string key = _levelDict[levelNumber].targets.Keys.ElementAt(i);
            //bug.Log(AddressableController.Instance.LoadAssetByName<Sprite>($"Candy Item/Sprite/leadboard 1"));
            targets[i].sprite = AddressableController.Instance.LoadAssetByName<Sprite>($"Candy Item/Sprite/{key}");
            targets[i].color = new Color(1, 1, 1, 1);   
            targetsCount[i].SetText(_levelDict[levelNumber].targets[key].ToString());
        }
    }

    public void ResetData()
    {
        levelText.SetText("");

        for (int i = 0; i < 3; i++)
        {
            stars[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _levelDict[levelNumber].targets.Count; i++)
        {
            targets[i].color = new Color(1, 1, 1, 0);
            targetsCount[i].SetText("".ToString());
        }
    }
} 