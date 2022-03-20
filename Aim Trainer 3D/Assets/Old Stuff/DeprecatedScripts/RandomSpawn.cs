using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> food = new List<GameObject>();
    public Transform spawnLocation;
    public int spawnChoice;
    public int foodChoice;
    public float spawnTimer;
    public static bool gameStart = false;
    public GameObject button1;
    public Renderer button2;
    public GameObject button3;
    public GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStart == true)
        {
            Waves();
        }
        Rando();
    }

    public void OnTriggerEnter(Collider other)
    {
        BeginGame();
    }

    public void Rando()
    {
        spawnChoice = Random.Range(0, spawnPoints.Count);
        foodChoice = Random.Range(0, food.Count - 1);
        spawnLocation = spawnPoints[spawnChoice];
    }

    public void BeginGame()
    {
        gameStart = true;
        //disable buttons on start
        button1.SetActive(false);
        button2.enabled = false;
        button3.SetActive(false);
        canvas.SetActive(false);

    }

    public void Waves()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            Instantiate(food[foodChoice], spawnLocation.position, spawnLocation.rotation);
            spawnTimer = 2f;
        }
    }
}
