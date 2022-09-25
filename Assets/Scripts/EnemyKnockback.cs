using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float KnockbackMultiplier = 1f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Cockroach")
        {
            Vector3 enemyPosition = collision.gameObject.transform.position;
            Vector2 KnockbackVector = new Vector2(transform.position.x, transform.position.y) - new Vector2(enemyPosition.x, enemyPosition.y);
            rb.AddForce(KnockbackVector * KnockbackMultiplier);

        }
    }
}
