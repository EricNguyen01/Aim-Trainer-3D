using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public Transform spawnLocation;
    public List<GameObject> objList = new List<GameObject>();
    public int listSelection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT!");
        SpawnObj();
    }

    public void SpawnObj()
    {
        Instantiate(objList[listSelection], spawnLocation.position, spawnLocation.rotation);

    }
}
