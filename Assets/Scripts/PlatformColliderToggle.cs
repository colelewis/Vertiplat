using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderToggle : MonoBehaviour
{
    public GameObject player;

    private Collider2D PlatformCollider;
    private float colliderXSize;
    private float colliderYSize;
    private float playerXSize;
    private float playerYSize;
    // Start is called before the first frame update
    void Start()
    {
        PlatformCollider = GetComponent<Collider2D>();
        colliderXSize = PlatformCollider.bounds.size.x /2;
        colliderYSize = PlatformCollider.bounds.size.y /2;

        var playerCollider = player.GetComponent<Collider2D>();
        playerXSize = playerCollider.bounds.size.x;
        playerYSize = playerCollider.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float playerRight = player.transform.position.x + playerXSize / 2;
        float playerLeft = player.transform.position.x - playerXSize / 2;
        float playerBottom = player.transform.position.y - playerYSize / 2;
        if (playerLeft < transform.position.x + colliderXSize && playerRight > transform.position.x - colliderXSize && 
            playerBottom < transform.position.y+colliderYSize)
        {
            PlatformCollider.enabled = false;
            
        }
        else
        {
            PlatformCollider.enabled = true;
        }
    }
}
