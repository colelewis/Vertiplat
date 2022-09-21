using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Player; //player object
    public float RiseSpeed = 1.0f; //speed at which the camera will go up automatically
    public float CatchUpLeeway = 1.0f; //how far up relative to the camera the player is allowed to get before it catches up to them
    public int ZAxis = -10; //sets z axis of camera

    // Update is called once per frame
    void Update()
    {
        if(Player.transform.position.y > transform.position.y+CatchUpLeeway)
        {
            transform.position = new Vector3(transform.position.x, Player.transform.position.y-CatchUpLeeway, ZAxis);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+RiseSpeed*Time.deltaTime, ZAxis);
        }
    }
}
