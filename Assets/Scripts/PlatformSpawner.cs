using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public float randomXDeviationUpperBound = 0.6f;
    public float randomXDeviationLowerBound = -0.6f;
    public float randomYDeviationUpperBound = 0.08f;
    public float randomYDeviationLowerBound = 0.06f;
    public float platformSpawnYThreshold = 0.08f;

    public GameObject platform;
    public List<GameObject> pooledPlatforms;
    public int poolLimit = 10;

    // public GameObject movingPlatform;
    // public GameObject 



    public GameObject player;
    public int randomSeed;

    // Start is called before the first frame update
    void Start()
    {   
        pooledPlatforms = new List<GameObject>();
        for (int i = 0; i < poolLimit; i++) {
            GameObject p = (GameObject)Instantiate(platform);
            p.GetComponent<PlatformColliderToggle>().player = player;
            p.SetActive(false);
            pooledPlatforms.Add(p);
        }
        Random.InitState(randomSeed);
        StartCoroutine(spawnPlatforms());
    }

    public GameObject GetPooledPlatform() {
        for (int i = 0; i < pooledPlatforms.Count; i++) {
            if (!pooledPlatforms[i].activeInHierarchy) {
                return pooledPlatforms[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject p in pooledPlatforms) {
            Vector3 platformInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(p.transform.position);
            if (platformInCameraPosition.y < -5f) { // destroy platform if it leaves camera
                p.SetActive(false);
            }
        }
        
        // Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
        // Debug.Log(playerInCameraPosition);
    }

    IEnumerator spawnPlatforms() {
        while(true) {
            Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
            Vector3 location = UnityEngine.Camera.main.WorldToViewportPoint(Vector3.zero);
            float randomXDeviation = Random.Range(randomXDeviationUpperBound, randomXDeviationLowerBound); // to be tuned
            float randomYDeviation = Random.Range(randomYDeviationLowerBound, randomYDeviationUpperBound); // to be tuned

            location.x = Mathf.Clamp((playerInCameraPosition.x + randomXDeviation), -1, 1);
            // location.y = Mathf.Clamp((playerInCameraPosition.y + randomYDeviation), (playerInCameraPosition.y + platform.GetComponent<BoxCollider2D>().bounds.size.y), (playerInCameraPosition.y + randomYDeviationUpperBound));
            location.y = (playerInCameraPosition.y - 0.01f) + randomYDeviation;

            location = UnityEngine.Camera.main.ViewportToWorldPoint(location); // convert back to world space

            if (playerInCameraPosition.y >= platformSpawnYThreshold) { // spawn threshold 
                GameObject newPlatform = GetPooledPlatform();
                if (newPlatform != null) {
                    newPlatform.transform.position = location;
                    newPlatform.SetActive(true);
                }

                Collider2D[] platformOverlaps = Physics2D.OverlapCircleAll(newPlatform.transform.position, 3.5f, 3); // PlatformLayer is layer 3
                    if (platformOverlaps.Length > 0) {
                        newPlatform.SetActive(false);
                }
                
                // Debug.Log(spawnedPlatform.transform.position);
            }
            yield return new WaitForSeconds(1.3f - (UnityEngine.Camera.main.GetComponent<Camera>().RiseSpeed / 2));
        }
    }
}
