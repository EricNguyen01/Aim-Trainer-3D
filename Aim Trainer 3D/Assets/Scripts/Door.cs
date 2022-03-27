using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openPos;
    [SerializeField] private float openSpeed;
    private bool hasOpened = false;

    public void Open()
    {
        if (!hasOpened)
        {
            hasOpened = true;
            StartCoroutine(DoorOpenSequence());
        }
    }

    private IEnumerator DoorOpenSequence()
    {
        float dist = Vector3.Distance(transform.position, openPos);

        while(dist > 0.13f)
        {
            if (openPos.x - transform.position.x <= 0f) transform.position -= transform.forward * openSpeed * Time.fixedDeltaTime;
            else transform.position += transform.forward * openSpeed * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }
    }
}
