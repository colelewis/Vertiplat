using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    public float scrollSpeed;
    public float RiseSpeed = 1.0f; //speed at which the camera will go up automatically

    public MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureOffset += new Vector2(0f, Time.deltaTime * scrollSpeed);
        transform.position = new Vector3(UnityEngine.Camera.main.transform.position.x, UnityEngine.Camera.main.transform.position.y, 1);
    }
}
