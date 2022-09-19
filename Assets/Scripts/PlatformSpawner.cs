using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public int frequency;
    public double yPlayerDeviation;
    public float randomXDeviationUpperBound;
    public float randomXDeviationLowerBound;
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
        StartCoroutine(spawnPlatforms());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
        // Vector3 location = Vector3.zero;
        // float randomXDeviation = Random.Range(randomXDeviationUpperBound, randomXDeviationLowerBound); // to be tuned
        // float randomYDeviation = Random.Range(randomYDeviationLowerBound, randomYDeviationUpperBound); // to be tuned

        // location.x = playerInCameraPosition.x + randomXDeviation;
        // location.y = playerInCameraPosition.y + yPlayerDeviation + randomYDeviation;

        // if (playerInCameraPosition.y > platformSpawnYThreshold) { // spawn threshold 
        //     for (int i = 0; i < frequency; i++) {
        //         GameObject spawnedPlatform = Instantiate(platform, location, Quaternion.identity);
        //     }
        // }
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject p in platforms) {
            Vector3 platformInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(p.transform.position);

            if (platformInCameraPosition.y < 0f) { // destroy platform if it leaves camera
                Destroy(p);
            }
        }
        Debug.Log(playerInCameraPosition);
    }

    IEnumerator spawnPlatforms() {
        while(true) {
            Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
            Vector3 location = Vector3.zero;
            float randomXDeviation = Random.Range(randomXDeviationUpperBound, randomXDeviationLowerBound); // to be tuned
            float randomYDeviation = Random.Range(randomYDeviationLowerBound, randomYDeviationUpperBound); // to be tuned

            location.x = UnityEngine.Camera.main.transform.position.x + playerInCameraPosition.x + randomXDeviation;
            location.y = UnityEngine.Camera.main.transform.position.y + playerInCameraPosition.y + randomYDeviation;
            if (playerInCameraPosition.y >= platformSpawnYThreshold) { // spawn threshold 
                GameObject spawnedPlatform = Instantiate(platform, location, Quaternion.identity);
            }
            yield return new WaitForSeconds(1 - playerInCameraPosition.y);
        }
        
    }
}
