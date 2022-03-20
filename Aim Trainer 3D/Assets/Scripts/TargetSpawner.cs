using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private List<Target> targetsToSpawn = new List<Target>();

    // Start is called before the first frame update
    private void Start()
    {
        CheckIfTargetsToSpawnListIsValid();
    }

    //Called by SpawnManager
    public void SpawnTarget()
    {
        if (targetsToSpawn == null) return;

        DestroyExistingStaticTarget();//before spawning new 

        int i = 0;
        if(targetsToSpawn.Count > 0) i = Random.Range(0, targetsToSpawn.Count);

        GameObject targetObj = Instantiate(targetsToSpawn[i].gameObject, transform.position, Quaternion.identity, transform);
        Target target = targetObj.GetComponent<Target>();
        if (target == null) target = targetObj.AddComponent<Target>();
        target.SetTargetSpawnerThatSpawnedThisTarget(this);
    }

    private void DestroyExistingStaticTarget()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Target target = transform.GetChild(i).GetComponent<Target>();
                if (target.isStatic)
                {
                    target.DestroyTarget();
                }
            }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 size = new Vector3(1f, 1f, 1f);
        Gizmos.DrawCube(transform.position, size);
    }
}
