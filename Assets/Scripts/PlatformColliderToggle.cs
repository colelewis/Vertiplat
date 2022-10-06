using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderToggle : MonoBehaviour
{
    public GameObject player;

    private Collider2D PlatformCollider;
    private Collider2D PlayerCollider;
    private float colliderXSize;
    private float colliderYSize;
    private float playerXSize;
    private float playerYSize;
    // Start is called before the first frame update
    void Start()
    {
        PlatformCollider = GetComponent<Collider2D>();
        colliderXSize = PlatformCollider.bounds.size.x / 2;
        colliderYSize = PlatformCollider.bounds.size.y / 2;

        PlayerCollider = player.GetComponent<Collider2D>();
        playerXSize = PlayerCollider.bounds.size.x;
        playerYSize = PlayerCollider.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        colliderXSize = PlatformCollider.bounds.size.x / 2;
        colliderYSize = PlatformCollider.bounds.size.y / 2;
        playerXSize = PlayerCollider.bounds.size.x;
        playerYSize = PlayerCollider.bounds.size.y;
        float playerRight = player.transform.position.x + playerXSize / 2 + PlayerCollider.offset.x;
        float playerLeft = player.transform.position.x - playerXSize / 2 + PlayerCollider.offset.x;
        float playerBottom = player.transform.position.y - playerYSize / 2 + PlayerCollider.offset.y;
        if (playerLeft < transform.position.x + colliderXSize && playerRight > transform.position.x - colliderXSize &&
            playerBottom < transform.position.y + colliderYSize)
        {
            Physics2D.IgnoreCollision(PlatformCollider, PlayerCollider, true);
            //GetComponentInChildren<SpriteRenderer>().color = Color.red;

        }
        else
        {
            Physics2D.IgnoreCollision(PlatformCollider, PlayerCollider, false);
            //GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
        //Debug.DrawLine(new Vector3(transform.position.x + colliderXSize, transform.position.y, -3), new Vector3(transform.position.x + colliderXSize, -2, 0), Color.red, -3);
        //Debug.DrawLine(new Vector3(transform.position.x - colliderXSize, transform.position.y, -3), new Vector3(transform.position.x - colliderXSize, -2, 0), Color.red, -3);
    }
}