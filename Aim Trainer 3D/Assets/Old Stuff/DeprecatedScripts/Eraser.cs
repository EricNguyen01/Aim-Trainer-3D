using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyGameObject()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Prefab");
        foreach (GameObject shape in obj)
            GameObject.Destroy(shape);

        GameObject[] obj2 = GameObject.FindGameObjectsWithTag("Split");
        foreach (GameObject shape in obj2)
            GameObject.Destroy(shape);
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyGameObject();
    }

}
