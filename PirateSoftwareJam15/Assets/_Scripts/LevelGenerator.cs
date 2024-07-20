using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    [SerializeField] private GameObject[] floorPrefabs;
    [SerializeField] private Room[] roomPrefabs;
    [SerializeField] private Room[] hallwayPrefabs;
    [SerializeField] private GameObject[] wallPrefabs;
    [SerializeField] private int wallNoWindowIndex = 2;
    [SerializeField] private GameObject[] doorPrefabs;
    [SerializeField] private GameObject player;

    [SerializeField] private int levelWidth = 10;
    [SerializeField] private int levelHeight = 10;
    [SerializeField] private float tileSize = 5f;
    [SerializeField] private float wallSkipWidth = 0.75f;
    [SerializeField] private float wallSkipHeight = 0.5f;

    private GameObject[,] levelFloor;
    private GameObject[,] levelWall1;
    private GameObject[,] levelWall2;
    private GameObject[,] levelDoor;
    private GameObject floorParent;
    private GameObject roomParent;
    private GameObject wallParent1;
    private GameObject wallParent2;
    private GameObject doorParent;

    private void Awake() {
        levelFloor = new GameObject[levelWidth, levelHeight];
        levelWall1 = new GameObject[levelWidth, levelHeight];
        levelWall2 = new GameObject[levelWidth, levelHeight];
        levelDoor = new GameObject[levelWidth, levelHeight];
        floorParent = transform.Find("Floor").gameObject;
        roomParent = transform.Find("Room").gameObject;
        wallParent1 = transform.Find("Wall1").gameObject;
        wallParent2 = transform.Find("Wall2").gameObject;
        doorParent = transform.Find("Door").gameObject;

        GenerateLevel();
    }

    private void GenerateLevel() {

        
        if(floorParent != null) {
            GenerateFloor();
        }

        if(roomParent != null) {
            GenerateRoom();
        }
        /*
        if(wallParent1 != null && wallParent2 != null) {
            GenerateWall();
        }
        if(doorParent != null) {
            GenerateDoor();
        }*/

        SetupPlayer();
    }

    private void GenerateFloor() {
        for(int x = 0; x < levelWidth; x++) {
            for(int z = 0; z < levelHeight; z++) {

                int randomFloor = Random.Range(0, floorPrefabs.Length);
                levelFloor[x, z] = Instantiate(floorPrefabs[randomFloor], new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity, floorParent.transform);
            }
        }
    }

    private void GenerateRoom() {
        int x = levelWidth / 2;
        for(int z = 0; z < levelHeight; z++) {

            int randomFloor = Random.Range(0, floorPrefabs.Length);
            levelFloor[x, z] = Instantiate(floorPrefabs[randomFloor], new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity, floorParent.transform);
        }
    }

    private void GenerateWall() {
        
        for(int x = 0; x < levelWidth; x++) {
            int randomWallNoWindow = Random.Range(0, wallNoWindowIndex);
            levelWall1[x, 0] = Instantiate(wallPrefabs[randomWallNoWindow], new Vector3((x + 1) * tileSize, 0, 0), Quaternion.identity, wallParent1.transform);

            for(int z = 1; z < levelHeight; z++) {

                float randomSkip = Random.Range(0f, 1f);
                if(randomSkip < wallSkipWidth) {
                    continue;
                }

                int randomWall = Random.Range(0, wallPrefabs.Length);
                levelWall1[x, z] = Instantiate(wallPrefabs[randomWall], new Vector3((x + 1) * tileSize, 0, z * tileSize), Quaternion.identity, wallParent1.transform);
            }

            levelWall1[x, levelHeight - 1] = Instantiate(wallPrefabs[randomWallNoWindow], new Vector3((x + 1) * tileSize, 0, levelHeight * tileSize), Quaternion.identity, wallParent1.transform);
        }
        
        for(int z = 0; z < levelHeight; z++) {
            int randomWallNoWindow = Random.Range(0, wallNoWindowIndex);
            levelWall2[0, z] = Instantiate(wallPrefabs[randomWallNoWindow], new Vector3(0, 0, z * tileSize), Quaternion.Euler(0, 90, 0), wallParent2.transform);
            for(int x = 0; x < levelWidth - 1; x++) {

                float randomSkip = Random.Range(0f, 1f);
                if(randomSkip < wallSkipHeight) {
                    continue;
                }

                int randomWall = Random.Range(0, wallPrefabs.Length);
                levelWall2[x, z] = Instantiate(wallPrefabs[randomWall], new Vector3((x + 1) * tileSize, 0, z * tileSize), Quaternion.Euler(0, 90, 0), wallParent2.transform);
            }
            levelWall2[levelWidth - 1, z] = Instantiate(wallPrefabs[randomWallNoWindow], new Vector3(levelWidth * tileSize, 0, z * tileSize), Quaternion.Euler(0, 90, 0), wallParent2.transform);
        }
    }

    private void GenerateDoor() {
    
    }

    private void SetupPlayer() {
        List<Vector3> spawnpoints = new List<Vector3>();
        foreach(GameObject floor in levelFloor) {
            if(floor.TryGetComponent(out Spawnpoint spawn)) {
                spawnpoints.Add(floor.transform.position);
            }
        }
        Vector3 spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Count)];
        spawnpoint += new Vector3(tileSize / 2, 1, tileSize / 2);
        player.GetComponent<Rigidbody>().position = spawnpoint;
    }

    [System.Serializable]
    public class Room {
        public GameObject prefab;
        public int hallSize;
    }

}
