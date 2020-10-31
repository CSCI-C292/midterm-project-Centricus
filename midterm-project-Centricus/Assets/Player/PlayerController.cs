using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Inspector-Editable Values
    public Animator animator;
    public LayerMask platformMask;
    public LayerMask enemyMask;
    public float platformCheckDistance;
    public float hSpeed;
    public float maxHSpeed;
    public float jumpSpeed;
    public float gravity;

    // Internal Values
    string facing = "Right";
    Vector3 velocity;
    bool grounded;
    bool wasGrounded;
    bool jumping;
    bool attacking;
    float hInput;


    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        // Update whether the player is airborne
        wasGrounded = grounded;
        grounded = CheckPlatformBelow();

        // Getting inputs
        hInput = Input.GetAxis("Horizontal");
        jumping = Input.GetButtonDown("Jump");


        // End Jump
        if (!wasGrounded && grounded)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", false);
        }
    }

    void FixedUpdate() 
    {
        Move();
        if (attacking)
        {
            Attack();
        }
    }

    // Returns true if there is a platform immediately underneath the player
    bool CheckPlatformBelow()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, platformCheckDistance, platformMask))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    // Returns true if there is a platform immediately above the player
    bool CheckPlatformAbove()
    {
        if (Physics2D.Raycast(transform.position, Vector2.up, platformCheckDistance, platformMask))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    void Move()
    {
        // TODO: Make all edits to velocity
    
        // Jumping
        if (grounded && jumping)
        {
            velocity.y += jumpSpeed;
            animator.SetBool("Jumping", true);
        }

        // Ceiling
        if (velocity.y > 0 && CheckPlatformAbove())
        {
            velocity.y = 0;
        }

        // Facing
        if (facing == "Right" && hInput == -1)
        {
            facing = "Left";
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (facing == "Left" && hInput == 1)
        {
            facing = "Right";
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        // Horizontal movement
        velocity.x = hInput * hSpeed;

        // Speed constraints
        if (velocity.x > maxHSpeed)
        {
            velocity.x = maxHSpeed;
        }
        if (velocity.x < -1 * maxHSpeed)
        {
            velocity.x = -1 * maxHSpeed;
        }

        // Gravity
        velocity.y -= gravity;

        // Stop on the ground
        if (grounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        // Animation
        if (velocity.y < 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        animator.SetFloat("hSpeed", Mathf.Abs(velocity.x));
        
        velocity.z = 0;
        transform.position += velocity;

    }

    void Attack()
    {
        // TODO: Attack
    }
}
