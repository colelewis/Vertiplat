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


    //more feedback? squash + stretch, different particles, etc.

    public GameObject DustParticleSystem; //dust particle system
    public GameObject sprite; //player sprite
    public float MoveSpeed = 8f; //speed of player moving left and right
    public float JumpPower = 14f; //how high the player can jump
    public float JumpHoldTime = 0.2f; //how long the player can hold jump to continue to ascend upwards
    public float FastFallRate = 1.5f; //how fast the player falls when holding down while falling
    public float FallShrinkFactor = 0.05f; //how much the player shrinks while fast falling
    public float ShrinkTransitionTime = 0.1f; //how long it takes for the player to shrink while fast falling
    public float JumpBufferingTime = 0.1f; //how soon a player can hit jump before landing that will still count when landing
    //public float WallJumpPower = 6f; //overall jump power
    public float AirResistance = 0.15f; //slows the players control when in the air. MoveSpeed * AirResistance is movement calculation in the air
    //public float WallJumpTime = 0.2f;
    public float CoyoteTime = 0.1f; //time that the player still has to jump after walking off a platform
    public float WallStickTime = 0.067f; //how long the player sticks to a wall after moving away from it. this is to help pull off a more consistent wall jump. current time is 4 frames (assuming 60fps)

    private bool FastFalling = false;
    private bool Jumping = false;
    private bool OnGround;
    private Rigidbody2D rb;
    private float HorInput;
    private float VertInput;
    private float InternalJumpPower;
    private float CurrentJumpHoldTime;
    private float NormalXScale;
    private float ShrinkTransition = 0;
    private bool JumpDebounce = true;
    private Vector2 prevVel = new Vector2(0,0);
    private float LastJumpClock = 0;
    private bool RightHolding;
    private bool LeftHolding;
    private bool AwayWallJump = false;
    private float LastOnGround = 0;
    private float playerSizeY;
    private float playerSizeX;
    private float WallStickTimer;
    private bool WallStickDebounce;
    private bool CanDoubleJump = true;
    private bool hardMode;


    void CreateDust(Vector3 location) //creates dust particles at players feet
    {
        try
        {
            GameObject dust = Instantiate(DustParticleSystem.gameObject, location, Quaternion.identity);
            ParticleSystem PS = dust.GetComponent<ParticleSystem>();
            PS.Play();
            Destroy(dust, PS.main.duration + 1f);
        }
        catch
        {
            Debug.Log("Dust reference missing");
        }
    }

    void JumpAway()
    {
        CurrentJumpHoldTime = JumpHoldTime / 8;
        InternalJumpPower = JumpPower * 1.5f;
        JumpDebounce = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentJumpHoldTime = JumpHoldTime;
        NormalXScale = sprite.transform.localScale.x;
        playerSizeX = GetComponent<Collider2D>().bounds.size.x;
        playerSizeY = GetComponent<Collider2D>().bounds.size.y;

    }

    private void Awake()
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            hardMode = FindObjectOfType<GameManager>().Hardmode;
        }
        else
        {
            hardMode = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Face the direction you're moving by changing scale.
        if (HorInput > 0)   // Moving Right
            NormalXScale = Mathf.Abs(NormalXScale);
        else if (HorInput == 0f)  // Maintain State
            /* Intentionally Empty */;
        else if (HorInput < 0 && NormalXScale > 0)  // Moving Left
            NormalXScale = NormalXScale * -1;
        
        HorInput = Input.GetAxisRaw("Horizontal");
        VertInput = Input.GetAxisRaw("Vertical");
        var Jump = Input.GetAxisRaw("Jump");
        if(Jump==1)
        {
            VertInput = 1;
        }

        if (VertInput == 1) //pressing up
        {
            WallStickTimer = 0;
            //not being on the ground and not jumping clears the jump hold time, so reset it if its still within coyote time and the player tries to jump
            if(LastOnGround<=CoyoteTime && !Jumping && JumpDebounce) 
            {
                CurrentJumpHoldTime=JumpHoldTime;
            }
            if(RightHolding && JumpDebounce)
            {
                if (HorInput == -1)
                {
                    JumpAway();
                    CreateDust(new Vector3(transform.position.x + playerSizeX/2, transform.position.y, -1f));
                }
                
                
            }
            else if(LeftHolding && JumpDebounce)
            {

                if (HorInput == 1)
                {
                    JumpAway();
                    CreateDust(new Vector3(transform.position.x - playerSizeX/2, transform.position.y, -1f));
                }

                
            }
            else if(CurrentJumpHoldTime > 0) //can still go up
            {
                Jumping = true;
                CurrentJumpHoldTime -= Time.deltaTime; //subtract from time remaining
                if(OnGround && JumpDebounce || LastOnGround<=CoyoteTime && JumpDebounce) //if this is the first frame of the jump
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f); //clear velocity for consistent jump
                    CreateDust(new Vector3(transform.position.x, transform.position.y - playerSizeY/2, -1f));
                    OnGround = false;
                    JumpDebounce = false;
                }
            }
            else
            {   
                if (!OnGround && !Jumping && JumpDebounce) //valid jump input, but player still in air
                {
                    if (CanDoubleJump && !hardMode)
                    {
                        InternalJumpPower = JumpPower;
                        CurrentJumpHoldTime = JumpHoldTime;
                        rb.velocity = new Vector2(rb.velocity.x, 0f); //clear velocity for consistent jump
                        CreateDust(new Vector3(transform.position.x, transform.position.y - playerSizeY / 2, -1f));
                        CanDoubleJump = false;
                    }
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
            if (NormalXScale>0)
                sprite.transform.localScale = new Vector3(NormalXScale * 1 - (FallShrinkFactor*(ShrinkTransition/ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
            else
                sprite.transform.localScale = new Vector3(NormalXScale * 1 + (FallShrinkFactor * (ShrinkTransition / ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
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
            if (NormalXScale > 0)
                sprite.transform.localScale = new Vector3(NormalXScale * 1 - (FallShrinkFactor * (ShrinkTransition / ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
            else
                sprite.transform.localScale = new Vector3(NormalXScale * 1 + (FallShrinkFactor * (ShrinkTransition / ShrinkTransitionTime)), sprite.transform.localScale.y, sprite.transform.localScale.z); //lerp size
        }
       
        if(!OnGround) //time the jump
        {
            LastJumpClock += Time.deltaTime;
            LastOnGround += Time.deltaTime;
        }

        if(rb.velocity.y == 0 && prevVel.y!= 0 && Mathf.Abs(rb.velocity.x-prevVel.x)>5.0f) //detect if landing reset X velocity
        {
            rb.velocity = new Vector2(prevVel.x, rb.velocity.y); //set it back
        }
        prevVel = rb.velocity;

        
        if(AwayWallJump && rb.velocity.y < -4 )
        {
            AwayWallJump = false;
        }
        
        //this has to be last
        if(HorInput==-1 && RightHolding || HorInput==1 && LeftHolding)
        {
            WallStickDebounce = false;
            if(WallStickTimer>0)
            {
                HorInput = 0;
            }
        }

        if(WallStickTimer>0 && !WallStickDebounce)
        {
            WallStickTimer -= Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        //deltaTime not needed (im pretty sure)

        bool ForceImparted = false;
        if (rb.velocity.x > MoveSpeed || rb.velocity.x < -MoveSpeed) //detects if a force was imparted onto the player by an enemy so the speed limit gets turned off
            ForceImparted = true;
        if(OnGround)
        {
            if (!ForceImparted)
            {
                rb.velocity = new Vector2(HorInput * MoveSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity += new Vector2(HorInput * MoveSpeed*0.1f, rb.velocity.y);
            }
            
        }
        else
        {
            
            if(!ForceImparted)
            {
                rb.velocity += new Vector2(HorInput * MoveSpeed * AirResistance, 0);
                if (rb.velocity.x > MoveSpeed || rb.velocity.x < -MoveSpeed) rb.velocity = new Vector2(HorInput * MoveSpeed, rb.velocity.y);
                if(HorInput == 0) rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity += new Vector2((HorInput * MoveSpeed * AirResistance)/6, 0);
            }
            
        }

        if (Jumping) rb.velocity = new Vector2(rb.velocity.x, InternalJumpPower);
        if (FastFalling) rb.velocity += new Vector2(0, -FastFallRate);


    }

    private void OnCollisionTouch(Collision2D collision) //for both collision enter and collision stay
    {

        Vector3 normal = collision.contacts[0].normal;
        float angle = Vector3.Angle(normal, Vector3.up);
        if(WallStickDebounce)
            WallStickTimer = WallStickTime;

        if(!RightHolding && !LeftHolding)
            InternalJumpPower = JumpPower;
            
        if(Mathf.Approximately(angle, 0) && !OnGround && rb.velocity.y <= 0)
        {
            // Debug.Log("Ground");
            CanDoubleJump = true;
            OnGround = true;
            LastOnGround = 0f;
            if (LastJumpClock <= JumpBufferingTime)
            {
                JumpDebounce = true;
                CurrentJumpHoldTime = JumpHoldTime;
            }
            CreateDust(new Vector3(transform.position.x, transform.position.y - playerSizeY/2, -1f));
        }
        else if(Mathf.Approximately(angle, 90))
        {
            Vector3 cross = Vector3.Cross(Vector3.forward, normal);
            if(cross.y>0)
            {
                //on left wall
                LeftHolding = true;
                if(rb.velocity.y<0)
                {
                    CreateDust(new Vector3(transform.position.x - playerSizeX/2, transform.position.y - playerSizeY/2, -1f));
                }
            }
            else
            {
                //on right wall
                RightHolding = true;
                if(rb.velocity.y<0)
                {
                    CreateDust(new Vector3(transform.position.x + playerSizeX/2, transform.position.y - playerSizeY/2, -1f));
                }
                
            }
        }

        sprite.transform.localScale = new Vector3(NormalXScale, sprite.transform.localScale.y, sprite.transform.localScale.z); //fix scale if player was fastfalling
        ShrinkTransition = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionTouch(collision);

    }

    private void OnCollisionStay2D(Collision2D collision) //this is needed for a weird glitch where sometimes OnCollisionEnter doesnt fire
    {
       if(!OnGround || CurrentJumpHoldTime!=JumpHoldTime)
        {
            OnCollisionTouch(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        RightHolding = false;
        LeftHolding = false;
        OnGround = false;
        WallStickDebounce = true;
    }
}