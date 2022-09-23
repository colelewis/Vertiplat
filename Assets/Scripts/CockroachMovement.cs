using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockroachMovement : MonoBehaviour
{
    // Movement and Collision Values
    private Rigidbody2D rigid_body;
    public int movement_speed = 1;
    private BoxCollider2D box_collider;
    // Manual Sprite Changes
    public GameObject sprite;
    private float x_sprite_scale;
    [SerializeField] private int _direction = 1; // Default to 1 or right

    // Start is called before the first frame update
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        ChangeDirection(1);
        Move();
        // Don't fall off platforms.
        if (SeesEdge())
            ChangeDirection();
    }

    private void Move()
    {
        rigid_body.velocity = new Vector2(movement_speed * _direction, rigid_body.velocity.y);
    }

    private void ChangeDirection()
    {
        ChangeDirection(_direction *= -1);
    }

    private void ChangeDirection(int new_direction)
    {
        // Stupid Check.
        if (new_direction == _direction)
            return;
        // Update Facing Direction.
        _direction = new_direction;
        x_sprite_scale = sprite.transform.localScale.x;
        if (_direction > 0)   // Moving Right
            x_sprite_scale = Mathf.Abs(x_sprite_scale);
        else if (_direction == 0f)  // Maintain State
            /* Intentionally Empty */;
        else if (_direction < 0 && x_sprite_scale > 0)  // Moving Left
            x_sprite_scale = x_sprite_scale * -1;
        sprite.transform.localScale = new Vector2(x_sprite_scale, sprite.transform.localScale.y);
    }

    public bool SeesEdge()
    {
        return false;
    }
}
