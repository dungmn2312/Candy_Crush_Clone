using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event Action<int> OnScoreToStarBar;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int _totalScore = 0;

    private void OnEnable()
    {
        CandyController.OnNotifyScore += CaculateScore;
    }

    private void OnDisable()
    {
        CandyController.OnNotifyScore -= CaculateScore;
    }

    private void CaculateScore(List<Candy> candies)
    {
        int score = 0;
        foreach (Candy candy in candies)
        {
            score += candy.score;
        }

        AddScore(score);
    }

    private void AddScore(int score)
    {
        _totalScore += score;
        SetScoreText();
    }

    private void SetScoreText()
    {
        scoreText.SetText(_totalScore.ToString());
        OnScoreToStarBar?.Invoke(_totalScore);
    }
}