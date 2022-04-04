using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccuracyTrackerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUI;

    private int shotConnected = 0;
    private int totalShotCount = 0;

    private void Start()
    {
        if(textMeshProUI == null)
        {
            Debug.LogWarning("Accuracy text mesh pro UI element is not assigned! Accuracy display UI disabled!");
            gameObject.SetActive(false);
            return;
        }

        DisplayAccuracy(0, 0, 0f);
    }

    private void OnEnable()
    {
        AccuracyTracker.OnAccuracyUpdated += DisplayAccuracy;
    }

    private void OnDisable()
    {
        AccuracyTracker.OnAccuracyUpdated -= DisplayAccuracy;
    }

    public void DisplayAccuracy(int shotConnected, int shotCount, float accuracy)
    {
        this.shotConnected = shotConnected;
        totalShotCount = shotCount;

        textMeshProUI.text = "Shots Connected: " + this.shotConnected.ToString() + 
                             "\n Total Shots Fired: " + totalShotCount.ToString() + 
                             "\n Accuracy: " + accuracy.ToString() + " %";
    }
}
