using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    //public GameObject obj1;
    //public GameObject obj2;
    //public GameObject obj3;
    public Transform spawnLocation;
    public List<GameObject> objList = new List<GameObject>();
    public GameObject spawnButton;
    public GameObject changeObjButton;
    public Text text;
    public int listSelection;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeShape(string text)
    {
        Text txt = GetComponent<Text>();
        txt.text = text;
    }

    public void ListSelection()
    {


        if (listSelection == objList.Count - 1)
        {
            listSelection = 0;
        }

        else listSelection++;


    }

    public void ResetListSelection(string text)
    {
        if(listSelection == 0)
        {
            Text txt = GetComponent<Text>();
            txt.text = text;
        }
    }

    public void SpawnObj()
    {
        Instantiate(objList[listSelection], spawnLocation.position, spawnLocation.rotation);

    }

    public void DestroyGameObject()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Prefab");
        foreach(GameObject shape in obj)
        GameObject.Destroy(shape);
    }

    public void OnTriggerEnter(Collider other)
    {
        SpawnObj();
    }

}
