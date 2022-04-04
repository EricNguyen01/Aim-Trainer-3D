using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ShootToActivate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startTextUI;
    [SerializeField] private UnityEvent OnGettingShot;
    private bool hasAlreadyActivated = false;
    private bool temporaryDisabled = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasAlreadyActivated || temporaryDisabled) return;

        if (collision.collider.CompareTag("Bullet"))
        {
            hasAlreadyActivated = true;
            OnGettingShot?.Invoke();
            if (AccuracyTracker.accuracyTrackerInstance != null) AccuracyTracker.accuracyTrackerInstance.SetShotCount(false, -1);
        }
    }

    public void ResetShootToActivate()
    {
        hasAlreadyActivated = false;
        temporaryDisabled = false;
        startTextUI.text = "Start";
    }

    public void TemporaryDisable(bool disabled)
    {
        temporaryDisabled = disabled;
    }

    public bool HasActivated()
    {
        return hasAlreadyActivated;
    }
}
