using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootToActivate : MonoBehaviour
{
    [SerializeField] private UnityEvent OnGettingShot;
    private bool hasAlreadyActivated = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasAlreadyActivated) return;

        if (collision.collider.CompareTag("Bullet"))
        {
            OnGettingShot?.Invoke();
        }
    }
}
