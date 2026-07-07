using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class FishingWater : MonoBehaviour
{
    public List<FishingLane> lanes = new List<FishingLane>();
    private int currentLane;


    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bobber;

    [SerializeField] private GameObject startingPosition;
    [SerializeField] private GameObject endingPosition;

    [Header("Settings")]

    [SerializeField] private float playerMaxSpeed;
    [SerializeField] private float playerMinSpeed;
    [SerializeField] private float playerAcceleration;
    [SerializeField] private float playerDecceleration;

    private float currentSpeed;

    [Header("Obstacles")]

    private List<GameObject> obstacles = new List<GameObject>();

    [SerializeField] private GameObject obstaclePrefab;
    
    [SerializeField] private float obstacleDistance; //how far are the obstacles
    [SerializeField] private int obstacleRows; // how many rows of obstacles
    [SerializeField] private int obstacleMin; // minimum number of obstacles per row



    private float spawnTime; //tracks time


    // Start is called before the first frame update
    void Start()
    {
        currentLane = lanes.Count / 2;

        //spawnTime = Time.time + obstacleFrequency;

        player.transform.position = startingPosition.transform.position;

        currentSpeed = 0;

        GenerateObstacles();
    }

    // Update is called once per frame
    void Update()
    {
        CheckBobber();

        MovePlayer();
       

        // check for obstacle spawn

        //if (Time.time > spawnTime) {

        //    spawnTime = Time.time + obstacleFrequency;

        //    //SpawnObstacles();
        //}
    }

    public void CheckBobber()
    {
        //left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane = Mathf.Clamp(currentLane - 1, 0, lanes.Count - 1);
            MoveBobber();
        }
        //right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane = Mathf.Clamp(currentLane + 1, 0, lanes.Count - 1);
            MoveBobber();
        }
    }
    public void MoveBobber()
    {
        bobber.transform.position = new Vector3(lanes[currentLane].transform.position.x, bobber.transform.position.y, bobber.transform.position.z);
    }

    public void MovePlayer()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {

            currentSpeed += playerAcceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed -= playerDecceleration * Time.deltaTime;
        }

        Debug.Log(currentSpeed);

        currentSpeed = Mathf.Clamp(currentSpeed, playerMinSpeed, playerMaxSpeed);

        player.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

    }

    public void GenerateObstacles()
    {
        for (int y = 1; y <= obstacleRows; y++)
        {

            //randomly picks obstacleMin amount of lanes to spawn obstacles

            List<int> obstacleInts = new List<int>();

            Debug.Log(lanes.Count);

            for (int i = 0; i < lanes.Count; i++)
            {
                obstacleInts.Add(i);
            }

            for (int j = 0; j < obstacleMin; j++)
            {
                int index = Random.Range(0, obstacleInts.Count);

                
                GameObject newOb = Instantiate(obstaclePrefab, lanes[obstacleInts[index]].transform);

                newOb.SetActive(true);
                newOb.transform.position = new Vector3(newOb.transform.position.x, newOb.transform.position.y, newOb.transform.position.z + (obstacleDistance * y)); 

                obstacleInts.RemoveAt(index);
            }


        }
    }

        //public void SpawnObstacles()
        //{
        //    Debug.Log("Spawn: " + spawnTime + " " + Time.time);

        //    //randomly picks obstacleCount amount of lanes to spawn obstacles

        //    List<int> obstacleInts = new List<int>();

        //    for (int i = 0; i < lanes.Count; i++) {
        //        obstacleInts.Add(i);
        //    }

        //    for (int j = 0; j < obstacleCount; j++)
        //    {
        //        int index = Random.Range(0, obstacleInts.Count - 1);

        //        lanes[index].SpawnObstacle(obstacle);

        //        Debug.Log("Spawning at: " + index);


        //        obstacleInts.RemoveAt(index);
        //    }
        //}

    }
