using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreAndHighScore : MonoBehaviour
{
    private int currentScore = 0;
    private int highScore = 0;
    [SerializeField] private TextMeshProUGUI currentScoreTextDisplay;
    [SerializeField] private TextMeshProUGUI highScoreTextDisplay;

    private void OnEnable()
    {
        Target.OnTargetHitByPlayer += SetCurrentScore;
    }

    private void OnDisable()
    {
        Target.OnTargetHitByPlayer -= SetCurrentScore;
    }

    private void Start()
    {
        if (currentScoreTextDisplay != null)
        {
            currentScoreTextDisplay.text = "Score: " + currentScore.ToString();
        }

        if (highScoreTextDisplay != null)
        {
            highScoreTextDisplay.text = "HighScore: " + highScore.ToString();
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void SetCurrentScore(int score)
    {
        currentScore += score;

        if(currentScoreTextDisplay != null)
        {
            currentScoreTextDisplay.text = "Score: " + currentScore.ToString();
        }

        if(currentScore > highScore)
        {
            highScore = currentScore;

            if (highScoreTextDisplay != null)
            {
                highScoreTextDisplay.text = "HighScore: " + highScore.ToString();
            }
        }
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;

        if (currentScoreTextDisplay != null)
        {
            currentScoreTextDisplay.text = "Score: " + currentScore.ToString();
        }
    }

    public void ResetHighScore()
    {
        highScore = 0;

        if (highScoreTextDisplay != null)
        {
            highScoreTextDisplay.text = "HighScore: " + highScore.ToString();
        }
    }
}
