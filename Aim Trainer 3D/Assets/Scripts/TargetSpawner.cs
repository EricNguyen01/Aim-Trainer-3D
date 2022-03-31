using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnMovingTargets = false;
    [SerializeField] private AudioClip fruitSpawnSound;
    [SerializeField] private AudioClip fruitDestroySound;
    [SerializeField] private List<Target> targetsToSpawn = new List<Target>();
    private AudioSource audioSource;

    public static event System.Action OnTargetSpawnerSpawnedNewTarget;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        CheckIfTargetsToSpawnListIsValid();
    }

    //Called by SpawnManager
    public void SpawnTarget()
    {
        if (targetsToSpawn == null) return;

        OnTargetSpawnerSpawnedNewTarget?.Invoke();

        int i = 0;
        if(targetsToSpawn.Count > 0) i = Random.Range(0, targetsToSpawn.Count);

        GameObject targetObj = Instantiate(targetsToSpawn[i].gameObject, transform.position, Quaternion.LookRotation(transform.forward), transform);
        Target targetComponent = targetObj.GetComponent<Target>();

        if(targetComponent != null)
        {
            targetComponent.SetTargetSpawner(this);

            if (spawnMovingTargets) targetComponent.isStatic = false;
            else targetComponent.isStatic = true;
        }

        if(fruitSpawnSound != null)
        {
            audioSource.clip = fruitSpawnSound;
            audioSource.Play();
        }
    }

    private void CheckIfTargetsToSpawnListIsValid()
    {
        if (targetsToSpawn != null && targetsToSpawn.Count > 0)
        {
            for (int i = 0; i < targetsToSpawn.Count; i++)
            {
                if (targetsToSpawn[i] == null || targetsToSpawn[i].Equals(null) || ReferenceEquals(targetsToSpawn[i], null))
                {
                    targetsToSpawn.RemoveAt(i);
                }
            }
        }

        if (targetsToSpawn == null || targetsToSpawn.Count == 0) Debug.LogError("Targets to spawn is not assigned for or has no element in target spawner: " + name + "!");
    }

    public void PlayFruitDestroySound()
    {
        if (fruitDestroySound != null)
        {
            audioSource.clip = fruitDestroySound;
            audioSource.Play();
        }
    }

    public bool IsSpawningMovingTargets()
    {
        return spawnMovingTargets;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(1f, 1f, 1f);
        Gizmos.DrawCube(transform.position, size);
    }
}
