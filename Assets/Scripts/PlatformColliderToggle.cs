using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderToggle : MonoBehaviour
{
    public GameObject player;

    private Collider2D PlatformCollider;
    private Collider2D PlayerCollider;
    private float platformXSize;
    private float platformYSize;
    [SerializeField] private float platformRight;
    [SerializeField] private float platformLeft;
    [SerializeField] private float platformTop;
    [SerializeField] private float platformBottom;

    private float playerXSize;
    private float playerYSize;
    private float playerRight;
    private float playerLeft;
    private float playerBottom;
    private float playerTop;
    // This state boolean will be used to short circuit the update conditional.
    [SerializeField] private bool solid = false;
    void Start()
    {
        PlatformCollider = GetComponent<Collider2D>();
        platformXSize = PlatformCollider.bounds.size.x / 2;
        platformYSize = PlatformCollider.bounds.size.y / 2;
        platformRight = transform.position.x + platformXSize;
        platformLeft = transform.position.x - platformXSize;
        platformTop = transform.position.y + platformYSize;
        platformBottom = transform.position.y - platformYSize;
        // Unchanging player values.
        PlayerCollider = player.GetComponent<Collider2D>();
        playerXSize = PlayerCollider.bounds.size.x;
        playerYSize = PlayerCollider.bounds.size.y;
    }
    void Update()
    {
        // Changing player values.
        playerRight = player.transform.position.x + playerXSize / 2;
        playerLeft = player.transform.position.x - playerXSize / 2;
        playerTop = player.transform.position.y + playerYSize / 2;
        playerBottom = player.transform.position.y - playerYSize / 2;
        if (solid == true) // Collide
        {
            // Solid platforms are colored white.
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        } else if (solid == false) // Passthrough
        {
            // Pass-able platforms are colored red.
            GetComponentInChildren<SpriteRenderer>().color = Color.red;
            // Turn off colliders.
            Physics2D.IgnoreCollision(PlatformCollider, PlayerCollider, true);
            // Solidify the platform.
            if ( (playerBottom > platformTop) // The player's bottom MUST BE above the platform
                  && (  (platformLeft < playerRight && playerRight < platformRight)     // The player's right side clipped within the platform or
                      ||(platformLeft < playerLeft  && playerLeft  < platformRight) ) ) // The player's left side clipped within the platform
            {
                // Turn on colliders.
                Physics2D.IgnoreCollision(PlatformCollider, PlayerCollider, false);
                // Set State.
                solid = true;
            }
        }
        // Lines for testing calculated position variables. Because Adam's insanity.
        
        Debug.DrawLine(new Vector2(platformRight, platformTop), new Vector2(platformRight, platformBottom), Color.green, 0);
        Debug.DrawLine(new Vector2(platformLeft, platformTop), new Vector2(platformLeft, platformBottom), Color.green, 0);
        Debug.DrawLine(new Vector2(playerRight, playerTop), new Vector2(playerRight, playerBottom), Color.green, 0);
        Debug.DrawLine(new Vector2(playerLeft, playerTop), new Vector2(playerLeft, playerBottom), Color.green, 0);
        
    }
}