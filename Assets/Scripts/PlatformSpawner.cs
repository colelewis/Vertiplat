using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public float frequency = 4f;
    public float yPlayerDeviation = 2f;
    public float randomXDeviation;
    public float randomYDeviation;
    public float randomYDeviationUpperBound;
    public float randomYDeviationLowerBound;
    public float platformSpawnYThreshold = 0.5f;
    public GameObject platform;
    // public GameObject movingPlatform; 
    public GameObject player;
    public int randomSeed;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(randomSeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
        Vector3 location = Vector3.zero;
        randomXDeviation = Random.Range(-10, 10); // to be tuned
        randomYDeviation = Random.Range(randomYDeviationLowerBound, randomYDeviationUpperBound); // to be tuned

        location.x = playerInCameraPosition.x + randomXDeviation;
        location.y = playerInCameraPosition.y + yPlayerDeviation + randomYDeviation;

        if (playerInCameraPosition.y > platformSpawnYThreshold) { // spawn threshold 
            GameObject spawnedPlatform = Instantiate(platform, location, Quaternion.identity);
        }
        Debug.Log(playerInCameraPosition);
    }
}
