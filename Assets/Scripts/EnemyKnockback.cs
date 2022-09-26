using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float KnockbackMultiplier = 1000f;
    public float KnockbackIncreaseMultiplier = 1.1f;
    public GameObject KnockbackParticles;
    public GameObject HitParticles;
    public AudioSource HitSound;

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

    void CreateKnockbackParticles(float angle, Vector3 Position)
    {
        try
        {
            Position = new Vector3(Position.x, Position.y, -3);
            GameObject dust = Instantiate(KnockbackParticles, Position, Quaternion.identity);
            dust.transform.localEulerAngles = new Vector3(0, 0, angle);
            ParticleSystem PS = dust.GetComponent<ParticleSystem>();
            PS.Play();
            Destroy(dust, PS.main.duration + 1f);
        }
        catch
        {
            Debug.Log("KnockbackParticles reference missing");
        }
    }

    void CreateHitParticles(Vector3 Position)
    {
        try
        {
            Position = new Vector3(Position.x, Position.y, -3);
            GameObject dust = Instantiate(HitParticles, Position, Quaternion.identity);
            ParticleSystem PS = dust.GetComponent<ParticleSystem>();
            PS.Play();
            Destroy(dust, PS.main.duration + 1f);
        }
        catch
        {
            Debug.Log("HitParticles reference missing");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Cockroach")
        {
            Vector3 enemyPosition = collision.gameObject.transform.position;
            Vector2 KnockbackVector = (new Vector2(transform.position.x, transform.position.y) - new Vector2(enemyPosition.x, enemyPosition.y)).normalized;
            if(KnockbackVector.x<0.4 && KnockbackVector.x >= 0)
            {
                KnockbackVector = new Vector2(0.6f, KnockbackVector.y);
            }
            else if(KnockbackVector.x>-0.4 && KnockbackVector.x < 0)
            {
                KnockbackVector = new Vector2(-0.6f, KnockbackVector.y);
            }
            rb.AddForce(KnockbackVector * KnockbackMultiplier * new Vector2(1f, 1f));
            FindObjectOfType<HitStop>().Pause(0.083f);
            CreateKnockbackParticles(Vector2.SignedAngle(Vector2.up, KnockbackVector), collision.gameObject.transform.position);
            CreateHitParticles(collision.contacts[Mathf.RoundToInt(collision.contacts.Length/2)].point);
            HitSound.Play();
            KnockbackMultiplier *= KnockbackIncreaseMultiplier;

        }
    }
}
