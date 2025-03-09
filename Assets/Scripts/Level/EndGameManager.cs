using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public bool endGameState; // True: Win - False: Lose

    [SerializeField] private RectTransform levelUI;
    [SerializeField] private RectTransform failLevel;
    [SerializeField] private RectTransform winLevel;
    [SerializeField] private Transform _board;

    [Header("Win Formation")]
    [SerializeField] private Image[] stars = new Image[3];
    [SerializeField] private TextMeshProUGUI scoreWinText;

    [Header("Lose Formation")]
    [SerializeField] private List<Image> targetCandiesImage;
    [SerializeField] private List<TextMeshProUGUI> targetCandiesLeft;
    [SerializeField] private TextMeshProUGUI scoreFailText;

    [Header("Reference Formation")]
    [SerializeField] private List<Image> refTargetCandiesImage;
    [SerializeField] private List<TextMeshProUGUI> refTargetLeftTexts;
    [SerializeField] private Image[] starsCheck;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Sprite starOn;


    //private int _score = 0;

    private float _moveTime = 1f;
    private float _waitForEnd = 1f;

    private void OnEnable()
    {
        WinLoseManager.OnNotifyEndGame += SetEndGameState;
    }

    private void OnDisable()
    {
        WinLoseManager.OnNotifyEndGame -= SetEndGameState;
    }

    private void SetEndGameState(bool value)
    {
        endGameState = value;

        ProcessEndGameUI();
    }

    private async void ProcessEndGameUI()
    {
        Debug.Log("ok");

        await UniTask.WaitForSeconds(_waitForEnd);

        DOMoveOutUI(levelUI);
        _board.gameObject.SetActive(false);

        if (endGameState)
        {
            WinGame();
        }
        else
        {
            FailGame();
        }
    }

    private void WinGame()
    {
        winLevel.gameObject.SetActive(true);
        DOScaleUI(winLevel);

        bool[] starsSave = new bool[3];

        for (int i = 0; i < 3; i++)
        {
            if (starsCheck[i].sprite == starOn)
            {
                stars[i].gameObject.SetActive(true);
                starsSave[i] = true;
            }
        }

        scoreWinText.SetText(scoreText.text);

        DataManager.Instance.WriteLevelData(starsSave, GameManager.Instance.currentLevel);
    }

    private void FailGame()
    {
        failLevel.gameObject.SetActive(true);
        DOScaleUI(failLevel);

        for (int i = 0; i < targetCandiesImage.Count; i++)
        {
            targetCandiesImage[i].sprite = refTargetCandiesImage[i].sprite;
            targetCandiesLeft[i].SetText(refTargetLeftTexts[i].text);
        }

        scoreFailText.SetText(scoreText.text);
    }

    private void DOMoveOutUI(RectTransform gObj)
    {
        gObj.DOAnchorPos(new Vector2(gObj.anchoredPosition.x, 120f), _moveTime)
            .SetEase(Ease.OutCubic);
    }

    private void DOMoveOutGameObject(Transform gObj)
    {
        gObj.DOMove(new Vector3(-8.2f, gObj.position.y), _moveTime)
            .SetEase(Ease.OutCubic);
    }

    private void DOScaleUI(RectTransform gObj)
    {
        gObj.DOScale(Vector3.one, _moveTime)
            .SetEase(Ease.OutBack);
    }
}