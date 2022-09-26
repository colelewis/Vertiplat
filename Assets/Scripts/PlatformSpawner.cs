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

    public LayerMask platformMask;
    public LayerMask playerLayer;
    public float platformMinDistance = 2f;
    public float spawnWaitTime = 1.3f;

    // public GameObject movingPlatform;
    // public GameObject 



    public GameObject player;
    public GameObject cockroach;
    public int randomSeed;

    // Start is called before the first frame update
    void Start()
    {
        pooledPlatforms = new List<GameObject>();
        for (int i = 0; i < poolLimit; i++)
        {
            GameObject p = (GameObject)Instantiate(platform);
            p.GetComponent<PlatformColliderToggle>().player = player;
            p.SetActive(false);
            pooledPlatforms.Add(p);
        }
        if (randomSeed != 0)
        {
            Random.InitState(randomSeed);
        }

        StartCoroutine(spawnPlatforms());
    }

    public GameObject GetPooledPlatform()
    {
        for (int i = 0; i < pooledPlatforms.Count; i++)
        {
            if (!pooledPlatforms[i].activeInHierarchy)
            {
                return pooledPlatforms[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject p in pooledPlatforms)
        {
            Vector3 platformInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(p.transform.position);
            if (platformInCameraPosition.y < -0.2f)
            { // destroy platform if it leaves camera
                p.SetActive(false);
            }
        }

        // Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
        // Debug.Log(playerInCameraPosition);
    }

    IEnumerator spawnPlatforms()
    {
        while (true)
        {
            bool spawnCockroach = Random.Range(1, 100) <= 50;
            float waitTime = spawnWaitTime - (UnityEngine.Camera.main.GetComponent<Camera>().RiseSpeed / 2);
            Vector3 playerInCameraPosition = UnityEngine.Camera.main.WorldToViewportPoint(player.transform.position);
            Vector3 location = UnityEngine.Camera.main.WorldToViewportPoint(Vector3.zero);
            float randomXDeviation = Random.Range(randomXDeviationUpperBound, randomXDeviationLowerBound); // to be tuned
            float randomYDeviation = Random.Range(randomYDeviationLowerBound, randomYDeviationUpperBound); // to be tuned

            location.x = Mathf.Clamp((playerInCameraPosition.x + randomXDeviation), -1, 1);
            //location.x = Mathf.Clamp((location.x + randomXDeviation), -1, 1);
            // location.y = Mathf.Clamp((playerInCameraPosition.y + randomYDeviation), (playerInCameraPosition.y + platform.GetComponent<BoxCollider2D>().bounds.size.y), (playerInCameraPosition.y + randomYDeviationUpperBound));
            location.y = (playerInCameraPosition.y - 0.01f) + randomYDeviation;

            location = UnityEngine.Camera.main.ViewportToWorldPoint(location); // convert back to world space
            if (location.x < -11)
            {
                float newX = -11 + Mathf.Abs(location.x + 11);
                location = new Vector3(newX, location.y, location.z);
            }
            else if (location.x > 11)
            {
                float newX = 11 - (location.x - 11);
                location = new Vector3(newX, location.y, location.z);
            }
            if (playerInCameraPosition.y >= platformSpawnYThreshold)
            { // spawn threshold 
                try
                {
                    GameObject newPlatform = GetPooledPlatform();
                    if (newPlatform != null)
                    {
                        newPlatform.transform.position = location;
                        newPlatform.SetActive(true);


                        Collider2D[] platformOverlaps = Physics2D.OverlapCircleAll(newPlatform.transform.position, platformMinDistance, platformMask); // PlatformLayer is layer 3
                        Collider2D[] playerCheck = Physics2D.OverlapCircleAll(newPlatform.transform.position, platformMinDistance, playerLayer);
                        if (platformOverlaps.Length > 0)
                        {
                            for (int i = 0; i < platformOverlaps.Length; i++)
                            {
                                if (platformOverlaps[i].gameObject.activeSelf && platformOverlaps[i] != newPlatform.GetComponent<Collider2D>())
                                {
                                    newPlatform.SetActive(false);
                                    waitTime = 0;
                                }
                            }

                        }
                        if (playerCheck.Length > 0 && spawnCockroach)
                        {
                            newPlatform.SetActive(false);
                            Debug.Log("overlap");
                            waitTime = 0;
                        }
                    }

                    if (spawnCockroach && newPlatform.activeSelf)
                    {
                        Instantiate(cockroach, new Vector2(newPlatform.transform.position.x, newPlatform.transform.position.y + 0.2f), Quaternion.identity);
                        newPlatform.transform.localScale = new Vector3(2f, 2f, 2f);
                    }
                }
                catch
                {
                    Debug.Log("No available platform");
                }
                
                // Debug.Log(spawnedPlatform.transform.position
            }


            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (GameObject p in pooledPlatforms)
        {
            Gizmos.DrawWireSphere(p.transform.position, platformMinDistance);
        }
    }
}
