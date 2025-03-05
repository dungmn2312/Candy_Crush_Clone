using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : MonoBehaviour
{
    const int ISAUTO_NORMALIZE_VALUE = 10000;
    const int NORMALIZE_VALUE = 1000;

    [SerializeField] private Sprite starSpriteOn;
    [SerializeField] private Image starBar;

    [SerializeField] private Image[] starsOff = new Image[3];
    public int[] targetStars;

    private bool _isAuto;

    private void Awake()
    {
        targetStars = new int[3];
        WaitForInstance();
    }

    private async void WaitForInstance()
    {
        await UniTask.WaitUntil(() => BoardManager.Instance != null);

        _isAuto = BoardManager.Instance.isAuto;
        int value = _isAuto ? ISAUTO_NORMALIZE_VALUE : NORMALIZE_VALUE;

        targetStars[0] = 2 * value / 10;
        targetStars[1] = 5 * value / 10;
        targetStars[2] = 10 * value / 10;
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreToStarBar += UpdateStarBar;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreToStarBar -= UpdateStarBar;
    } 

    private void UpdateStarBar(int score)
    {
        if (starBar.fillAmount < 1)
        {
            starBar.fillAmount = _isAuto ? ((float)score / ISAUTO_NORMALIZE_VALUE) : ((float)score / NORMALIZE_VALUE);
            //Debug.Log(_starBar.fillAmount);
            for (int i = 0; i < starsOff.Length; i++)
            {
                if (score >= targetStars[i] && starsOff[i] != starSpriteOn)
                {
                    starsOff[i].sprite = starSpriteOn;
                }
            }
        }
    }
}
