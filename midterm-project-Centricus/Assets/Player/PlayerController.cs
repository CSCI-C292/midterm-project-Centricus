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
    [SerializeField] const float knockbackOnHit = 300f;
	[SerializeField] Rigidbody2D rigidBody;
	[SerializeField] Animator animator;
	[SerializeField] GameObject axeThrownRightPrefab;
	[SerializeField] GameObject axeThrownLeftPrefab;
	[SerializeField] GameObject controlScreen;

	// Internal Variables
	Vector3 velocity = Vector3.zero;
	string facing = "Right";
    float hInput = 0f;
	bool grounded;
	bool wasGrounded;
    bool jumping = false;
	bool attackMelee = false;
	bool attackThrow = false;
	bool hasAxe = true;
	bool recalling = false;

	// Update is called every frame
    private void Update() {
		// Get Inputs
		hInput = Input.GetAxisRaw("Horizontal");
		if (Input.GetButtonDown("Jump")) 
		{
			jumping = true;
			animator.SetBool("Jumping", true);
		}
		if (Input.GetButtonDown("Fire1"))
		{
			attackMelee = true;
		}
		if (Input.GetButtonDown("Fire2"))
		{
			attackThrow = true;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (controlScreen.activeSelf)
			{
				controlScreen.SetActive(false);
			}
			else if (!controlScreen.activeSelf)
			{
				controlScreen.SetActive(true);
			}
		}
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

		// Attack and reset attack variables
		Attack();
		attackMelee = false;
		attackThrow = false;
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

	// Attack handles all things attack-related
	void Attack()
	{
		// Can only attack if holding axe
		if(hasAxe)
		{
			// You cannot make both a melee and thrown attack at once. Melee takes precedence
			if (attackMelee)
			{
				// TODO: Melee attack
			}
			else if (attackThrow)
			{
				if (facing == "Right")
				{
					Instantiate(axeThrownRightPrefab, transform.position, Quaternion.identity);
					EventManager.Recalled += Recalled;

				}
				else if (facing == "Left")
				{
					Instantiate(axeThrownLeftPrefab, transform.position, Quaternion.identity);
					EventManager.Recalled += Recalled;
				}
				hasAxe = false;
			}
		}
		else 
		{
			if (attackThrow)
			{
				recalling = true;
			}
			if (recalling)
			{
				EventManager.InvokeRecall(transform);
			}
		}
	}

	void Recalled()
	{
		hasAxe = true;
		recalling = false;
		EventManager.Recalled -= Recalled;
	}
}
