using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    private int _scorePerPhase = 0, _totalScore = 0;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void AddScore(int score)
    {
        _scorePerPhase += score;
    }

    private void AddTotalScore()
    {
        _totalScore += _scorePerPhase;
    }
}