using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyKnockback : MonoBehaviour
{
    
    public float KnockbackMultiplier = 1000f;
    public float KnockbackIncreaseMultiplier = 1.1f;
    public GameObject KnockbackParticles;
    public GameObject HitParticles;
    public AudioSource HitSound;
    public Material StarMat;

    private Rigidbody2D rb;
    private CharacterController cc;
    private bool roachBoosted;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float processedKnockbackMultiplier = Mathf.Round((KnockbackMultiplier - 1000) / 10) * 100.0f * 0.001f;
        canvas.transform.Find("KnockbackOverlay").GetComponent<TextMeshProUGUI>().text = "Knockback: " + processedKnockbackMultiplier.ToString() + "%";
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
            StarMat.color = Color.yellow;
            roachBoosted = false;
            Vector3 enemyPosition = collision.gameObject.transform.position;
            Vector2 KnockbackVector = (new Vector2(transform.position.x, transform.position.y) - new Vector2(enemyPosition.x, enemyPosition.y)).normalized;
            if(!cc.FastFalling)
            {
                if(KnockbackVector.x<0.4 && KnockbackVector.x >= 0)
                {
                    KnockbackVector = new Vector2(0.6f, KnockbackVector.y);
                }
                else if(KnockbackVector.x>-0.4 && KnockbackVector.x < 0)
                {
                    KnockbackVector = new Vector2(-0.6f, KnockbackVector.y);
                }
            }
            else
            {
                if(KnockbackVector.x < 0.4 && KnockbackVector.x >= 0 || KnockbackVector.x > -0.4 && KnockbackVector.x < 0)
                {
                    //roach boost
                    KnockbackVector = new Vector2(KnockbackVector.x, KnockbackVector.y * 0.8f);
                    StarMat.color = Color.green;
                    roachBoosted = true;
                }
            }
            if(roachBoosted)
            {
                cc.FastFalling = false;
                rb.velocity = Vector2.zero;
            }
            rb.AddForce(KnockbackVector * KnockbackMultiplier);
            FindObjectOfType<HitStop>().Pause(0.083f);
            CreateKnockbackParticles(Vector2.SignedAngle(Vector2.up, KnockbackVector), collision.gameObject.transform.position);
            CreateHitParticles(collision.contacts[Mathf.RoundToInt(collision.contacts.Length/2)].point);
            HitSound.Play();
            KnockbackMultiplier *= KnockbackIncreaseMultiplier;

        }
    }
}
