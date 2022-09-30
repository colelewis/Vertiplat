using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachMovement : MonoBehaviour
{
    // Movement and Collision Values
    private Rigidbody2D rigid_body;
    public int movement_speed = 1;
    [SerializeField] private BoxCollider2D box_collider;
    // Manual Sprite Changes
    public GameObject sprite;
    private float x_sprite_scale;
    [SerializeField] private int _direction = 1; // Default to 1 or right
    // Platform Edge Detection
    public LayerMask platform_layer;
    public float sight_box_scale = 1;
    public float sight_x_offset = 1;
    public float sight_y_offset = -0.8f;
    #pragma warning disable 0414 // Value is intentionally never used.
    [SerializeField] private bool _sees_platform = true; // ONLY for inspector testing.
    #pragma warning restore 0414
    public float cooldown_timer = Mathf.Infinity;
    public float change_direction_cooldown = 150;

    // Start is called before the first frame update, BUT is called for each unique instantiaton. :bigbrain:
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        x_sprite_scale = sprite.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        Move();
        // Don't fall off platforms.
        cooldown_timer += Time.time; // Change to Time.deltaTime later
        if (!SeesPlatform() && cooldown_timer >= change_direction_cooldown)
        {
            cooldown_timer = 0;
            ChangeDirection();
        }
    }

    private void Move()
    {
        rigid_body.velocity = new Vector2(movement_speed * _direction, rigid_body.velocity.y);
    }

    private void ChangeDirection()
    {
        ChangeDirection(_direction * -1);
    }

    private void ChangeDirection(int new_direction)
    {
        if (new_direction == _direction)
            return;
        // Update Facing Direction.
        _direction = new_direction;
        if (_direction > 0)   // Moving Right
            x_sprite_scale = Mathf.Abs(x_sprite_scale);
        else if (_direction == 0f)  // Maintain State
        {
            #pragma warning disable 0642
            /* Intentionally Empty Block */;
            #pragma warning restore 0642
        } else if (_direction < 0 && x_sprite_scale > 0)  // Moving Left
        {
            // Debug.Log("Changing Direction");
            x_sprite_scale = x_sprite_scale * -1;
        }
        sprite.transform.localScale = new Vector2(x_sprite_scale, sprite.transform.localScale.y);
    }

    // Everything is in terms of platforms.
    public bool SeesPlatform()
    {
        // Warning: Copy and paste into OnDrawGizmos() -> Gizmos.DrawWireCube
        RaycastHit2D sight = Physics2D.BoxCast(box_collider.bounds.center + new Vector3(sight_x_offset * transform.localScale.x * sprite.transform.localScale.x, sight_y_offset,0) , // Location
                                                                                                                                // What the hell, why does "x_sprite_scale" Not work but this does???
                                               new Vector2(box_collider.bounds.size.x, box_collider.bounds.size.y) * sight_box_scale, // Size
                                               0, 
                                               Vector2.left, // Side/Direction to look (not used here, smudged elsewhere)
                                               0, // Angle
                                               platform_layer); // Layer to detect
        // If sight is null, there is no platform.
        if (sight.collider != null)
        {
            // Only for inspector testing.
            _sees_platform = true;
            return true;
        } else
        {
            _sees_platform = false;
            return false;
        }
        //return sight.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box_collider.bounds.center + new Vector3(sight_x_offset* transform.localScale.x  * sprite.transform.localScale.x,sight_y_offset,0) , 
                            new Vector2(box_collider.bounds.size.x, box_collider.bounds.size.y) * sight_box_scale);
    }
}
