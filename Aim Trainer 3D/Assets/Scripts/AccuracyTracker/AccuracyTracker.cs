using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyTracker : MonoBehaviour
{
    public static AccuracyTracker accuracyTrackerInstance;

    public bool startRegisteringShot { get; set; } = false;
    public int shotConnected { get; private set; } = 0;
    public int totalShotCount { get; private set; } = 0;

    public static event System.Action<int, int, float> OnAccuracyUpdated;

    private void Awake()
    {
        if(accuracyTrackerInstance == null)
        {
            accuracyTrackerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetShotCount(bool connected, int shot)
    {
        if (!startRegisteringShot) return;

        if (connected)
        {
            shotConnected += shot;
        }

        totalShotCount += shot;

        OnAccuracyUpdated?.Invoke(shotConnected, totalShotCount, GetAccuracy());
    }

    public void SetShotStartRegisterationStatus(bool registered)
    {
        startRegisteringShot = registered;
    }

    private float GetAccuracy()
    {
        if(totalShotCount <= 0)
        {
            return 0f;
        }

        float acc = ((float)shotConnected / (float)totalShotCount) * 100f;
        acc = (float)System.Math.Round(acc, 1);
        return acc;
    }
}
