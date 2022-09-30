using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachClean : MonoBehaviour
{
    //make this into an object pool instead platform spawner maybe? but only- 
    //really a problem if this effects performance

    // Update is called once per frame
    void Update()
    {
        Vector3 roachCameraPos = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
        if (roachCameraPos.y < -0.2f)
        {
            //Debug.Log("destroy roach");
            Destroy(gameObject, 0.0f);
        }
    }
}
