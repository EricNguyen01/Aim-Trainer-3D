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

    private void OnCollisionEnter(Collision collision)
    {
        if (hasAlreadyActivated) return;

        if (collision.collider.CompareTag("Bullet"))
        {
            OnGettingShot?.Invoke();
            if (AccuracyTracker.accuracyTrackerInstance != null) AccuracyTracker.accuracyTrackerInstance.SetShotCount(false, -1);
        }
    }

    public void ResetShootToActivate()
    {
        hasAlreadyActivated = false;
        startTextUI.text = "Start";
    }
}
