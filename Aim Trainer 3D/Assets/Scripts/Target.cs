using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [field: SerializeField] public bool isStatic { get; private set; } = true;
    [SerializeField] private float timeAlive;
    [SerializeField] private int targetDestroyedScore;

    private TargetSpawner targetSpawnerThatSpawnedThisTarget;
    private Collider targetCollider;
    private float currentTimeAlive = 0f;

    public static event System.Action<int> OnTargetDestroyed;

    // Start is called before the first frame update
    private void Start()
    {
        targetCollider = GetComponent<Collider>();
        if (targetCollider == null) Debug.LogWarning("Target: " + name + " doesn't have a collider attached to it!");
    }

    private void Update()
    {
        currentTimeAlive += Time.deltaTime;

        if(currentTimeAlive >= timeAlive)
        {
            DestroyTarget();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            OnTargetDestroyed?.Invoke(targetDestroyedScore);
            DestroyTarget();
        }
    }

    public void SetTargetSpawnerThatSpawnedThisTarget(TargetSpawner targetSpawner)
    {
        targetSpawnerThatSpawnedThisTarget = targetSpawner;
    }

    public void DestroyTarget()
    {
        Destroy(gameObject);
    }
}
