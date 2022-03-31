using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] private bool displayTimeOnStart;

    [SerializeField]
    [Tooltip("Whether this timer is counting up or down. \n Default is count up since we are looking for the player's best time.")]
    private bool isCountingDown = false;

    [Header("Countdown Setting")]
    [SerializeField] [Tooltip("If timer is counting down, when should the start time be")]
    private float countdownFrom;
    [SerializeField] private UnityEvent OnCountdownFinished;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    

    public float currentTime { get; private set; } = 0f;
    public float bestTime { get; private set; } = 0f;

    private float currentCountdownTime;

    private bool startTimer = false;

    private bool displayBlankTime = false;

    private void Start()
    {
        if (displayTimeOnStart)
        {
            if (isCountingDown) timerText.text = DisplayTimeText(currentCountdownTime);
            else timerText.text = DisplayTimeText(currentTime);
        }
    }

    private void Update()
    {
        if (!startTimer) return;

        if (!isCountingDown)
        {
            currentTime += Time.deltaTime;
            if(timerText != null) timerText.text = DisplayTimeText(currentTime);
            return;
        }

        if(currentCountdownTime > 0f)
        {
            currentCountdownTime -= Time.deltaTime;
        }
        else
        {
            currentCountdownTime = 0f;
            startTimer = false;
            OnCountdownFinished?.Invoke();
        }

        if (timerText != null) timerText.text = DisplayTimeText(currentCountdownTime);
    }

    public void StartTimer()
    {
        currentTime = 0f;
        currentCountdownTime = countdownFrom;

        if (!isCountingDown)
        {
            if (timerText != null) timerText.text = DisplayTimeText(currentTime);
        }
        else
        {
            if (timerText != null) timerText.text = DisplayTimeText(currentCountdownTime);
        }

        startTimer = true;
    }

    public void StopTimer()
    {
        startTimer = false;

        if (isCountingDown)
        {
            if (timerText != null)
            {
                timerText.text = DisplayTimeText(currentCountdownTime);
                if (displayBlankTime) timerText.text = string.Empty;
            }

            return;
        }

        if(currentTime < bestTime)
        {
            bestTime = currentTime;
            if(bestTimeText != null) bestTimeText.text = DisplayTimeText(bestTime);
        }
    }

    private string DisplayTimeText(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        string secondsText = seconds.ToString();
        if (seconds < 10) secondsText = "0" + secondsText;

        return minutes.ToString() + ":" + secondsText;
    }

    public void SetDisplayBlankTimeOnCountdownFinished(bool status)
    {
        displayBlankTime = status;
    }
}
