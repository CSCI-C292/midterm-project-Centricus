using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Inspector-Editable Variables
    [SerializeField] float runSpeed = 40f;
	[SerializeField] float jumpingSpeed = 400f;
	[SerializeField] float smoothing = .05f;
	[SerializeField] LayerMask platformMask;
	[SerializeField] Transform platformChecker;
	[SerializeField] const float platformCheckRadius = .05f;
	[SerializeField] Rigidbody2D rigidBody;
	[SerializeField] Animator animator;

	// Internal Variables
	Vector3 velocity = Vector3.zero;
	string facing = "Right";
    float horizontalMove = 0f;
    float hInput = 0f;
	bool grounded;
	bool wasGrounded;
    bool jumping = false;
	bool attacking = false;
	bool throwing = false;
	
	// Awake is called when the script instance is being loaded
	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	// Update is called every frame
    private void Update() {
		// Get Inputs
		hInput = Input.GetAxisRaw("Horizontal");
		if (Input.GetButtonDown("Jump")) 
		{
			jumping = true;
			animator.SetBool("Jumping", true);
		}
		if (Input.GetButtonDown("Fire1")) attacking = true;
		if (Input.GetButtonDown("Fire2")) throwing = true;
    }

	// FixedUpdate is a framerate independent update for physics calculations
	private void FixedUpdate()
	{
		// Check for platforms and landing
		wasGrounded = grounded;
		grounded = CheckPlatforms();
		if (grounded && !wasGrounded)
		{
			animator.SetBool("Jumping", false);
		}

        // Move and reset jump
        Move();
        jumping = false;
	}

	// CheckPlatforms returns true if there is a platform immediately below the player
	bool CheckPlatforms()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(platformChecker.position, platformCheckRadius, platformMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				return true;
			}
		}
		return false;
	}

	// Move handles all movement of the player based on inputs and speed variables
	void Move()
	{
		// Find and apply new velocity based on input and speed
		Vector3 newVelocity = new Vector3(10f * hInput * runSpeed * Time.deltaTime, rigidBody.velocity.y, 0);
		rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, newVelocity, ref velocity, smoothing);

		// Animation
		animator.SetFloat("hSpeed", Mathf.Abs(rigidBody.velocity.x));
		if (rigidBody.velocity.y < 0)
		{
			animator.SetBool("Falling", true);
		}
		else
		{
			animator.SetBool("Falling", false);
		}

		// Jump (if attempting to jump while on the ground)
		if (grounded && jumping)
		{
			grounded = false;
			rigidBody.AddForce(new Vector2(0f, jumpingSpeed));
		}

		// Facing
		if (hInput == -1 && facing == "Right")
		{
			facing = "Left";
			GetComponent<SpriteRenderer>().flipX = true;
		}
		else if (hInput == 1 && facing == "Left")
		{
			facing = "Right";
			GetComponent<SpriteRenderer>().flipX = false;
		} 
	}
}
