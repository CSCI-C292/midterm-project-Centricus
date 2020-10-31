using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public LayerMask platformMask;
    public LayerMask enemyMask;
    public Vector3 velocity;
    public bool grounded;
    public bool wasGrounded;
    public bool jumping;
    public bool attacking;
    public float hInput;
    public float hSpeed;
    public float maxHSpeed;
    public float jumpSpeed;
    public float gravity;
    string facing = "Right";


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
        jumping = Input.GetButtonDown("Z");


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
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, platformMask))
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
        if (Physics2D.Raycast(transform.position, Vector2.up, 0.1f, platformMask))
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

        // Speed constraints
        if (velocity.x > maxHSpeed)
        {
            velocity.x = maxHSpeed;
        }
        if (velocity.x < -1 * maxHSpeed)
        {
            velocity.x = -1 * maxHSpeed;
        }

        velocity.y -= gravity;

        // Falling animation
        if (velocity.y < 0)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Falling", true);
        }
        
        transform.position += velocity;

    }

    void Attack()
    {
        // TODO: Attack
    }
}
