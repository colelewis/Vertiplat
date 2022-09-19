using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /*
     * this script was tuned for a gravity scale of 5, and continuous collisiton detection inside rigidbody 2D
     * 
     * 
     */



    public GameObject DustParticleSystem; //dust particle system
    public GameObject sprite; //player sprite
    public float MoveSpeed = 8f; //speed of player moving left and right
    public float JumpPower = 14f; //how high the player can jump
    public float JumpHoldTime = 0.2f; //how long the player can hold jump to continue to ascend upwards
    public float FastFallRate = 2f; //how fast the player falls when holding down while falling
    public float FallShrinkFactor = 0.05f; //how much the player shrinks while fast falling
    public float ShrinkTransitionTime = 0.1f; //how long it takes for the player to shrink while fast falling
    public float JumpTimingForgiveness = 0.1f; //how soon a player can hit jump before landing that will still count when landing

    private bool FastFalling = false;
    private bool Jumping = false;
    private bool OnGround;
    private Rigidbody2D rb;
    private float HorInput;
    private float VertInput;
    private float CurrentJumpHoldTime;
    private float NormalXScale;
    private float ShrinkTransition = 0;
    private bool JumpDebounce = true;
    private Vector2 prevVel = new Vector2(0,0);
    private float LastJumpClock = 0;


    void CreateDust() //creates dust particles at players feet
    {
        GameObject dust = Instantiate(DustParticleSystem.gameObject, new Vector3(transform.position.x, transform.position.y - 0.5f, -1f), Quaternion.identity);
        ParticleSystem PS = dust.GetComponent<ParticleSystem>();
        PS.Play();
        Destroy(dust, PS.main.duration + 1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentJumpHoldTime = JumpHoldTime;
        NormalXScale = sprite.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        HorInput = Input.GetAxisRaw("Horizontal");
        //Debug.Log(HorMovement);
        VertInput = Input.GetAxisRaw("Vertical");

        if (VertInput == 1) //pressing up
        {
            if(CurrentJumpHoldTime > 0) //can still go up
            {
                Jumping = true;
                CurrentJumpHoldTime -= Time.deltaTime; //subtract from time remaining
                if(OnGround && JumpDebounce) //if this is the first frame of the jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f); //clear velocity for consistent jump
                    CreateDust();
                    OnGround = false;
                    JumpDebounce = false;
                }
            }
            else
            {   
                if (!OnGround && !Jumping && JumpDebounce) //valid jump input, but player still in air
                {
                    LastJumpClock = 0; //track jump input
                    JumpDebounce = false;
                }

                Jumping = false; //stop going up if player is out of jump time
            }
        }
        if (VertInput == 0) //not pressing up or down
        {
  
            JumpDebounce = true; //stopped pressing jump so jump debounce is true
            Jumping = false; //stop going up
            if (CurrentJumpHoldTime > 0 && !OnGround) //if there was still time remaining in the jump then stop it
            {
                CurrentJumpHoldTime = 0;
            }
            else if(CurrentJumpHoldTime!=JumpHoldTime && OnGround)
            {
                CurrentJumpHoldTime = JumpHoldTime;
            }

        }
        if (VertInput == -1 && rb.velocity.y < -2) //pressing down while falling
        {
            FastFalling = true; //start fast falling
            if(ShrinkTransition<ShrinkTransitionTime)
            {
                ShrinkTransition += Time.deltaTime; //animation time
            }
            if(ShrinkTransition>ShrinkTransitionTime)
            {
                ShrinkTransition = ShrinkTransitionTime; //cap it at max time
            }    
            sprite.transform.localScale = new Vector3(NormalXScale * 1-(FallShrinkFactor*(ShrinkTransition/ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
        }
        else
        {
            FastFalling = false; //not pressing down
            if (ShrinkTransition >= ShrinkTransitionTime) //if shrunk
            {
                ShrinkTransition -= Time.deltaTime; //move it back
            }
            if (ShrinkTransition < 0)
            {
                ShrinkTransition = 0; //cap at 0
            }
            sprite.transform.localScale = new Vector3(NormalXScale * 1 - (FallShrinkFactor * (ShrinkTransition / ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
        }
       
        if(!OnGround) //time the jump
        {
            LastJumpClock += Time.deltaTime;
        }

        if(rb.velocity.y == 0 && prevVel.y!= 0 && Mathf.Abs(rb.velocity.x-prevVel.x)>5.0f) //detect if landing reset X velocity
        {
            rb.velocity = new Vector2(prevVel.x, rb.velocity.y); //set it back
        }
        prevVel = rb.velocity;


    }

    private void FixedUpdate()
    {
        //deltaTime not needed (im pretty sure)
        rb.velocity = new Vector2(HorInput * MoveSpeed, rb.velocity.y);
        if (Jumping) rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        if (FastFalling) rb.velocity += new Vector2(0, -FastFallRate);

    }

    private void OnGroundTouch() //for both collision enter and collision stay
    {
        
        
        if(rb.velocity.y <= 0 && !OnGround) //landed
        {
            OnGround = true;
            if(LastJumpClock<=JumpTimingForgiveness)
            {
                JumpDebounce = true;
            }
            CreateDust();
        }

        if(JumpDebounce) //if allowed to jump then reset time
                {
                    CurrentJumpHoldTime = JumpHoldTime;
                }

        sprite.transform.localScale = new Vector3(NormalXScale, sprite.transform.localScale.y, sprite.transform.localScale.z); //fix scale if player was fastfalling
        ShrinkTransition = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnGroundTouch();

    }

    private void OnCollisionStay2D(Collision2D collision) //this is needed for a weird glitch where sometimes OnCollisionEnter doesnt fire
    {
       if(!OnGround || CurrentJumpHoldTime!=JumpHoldTime)
        {
            OnGroundTouch();
        }
    }
}
