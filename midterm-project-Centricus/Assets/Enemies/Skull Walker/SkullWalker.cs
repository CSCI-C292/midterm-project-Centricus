using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullWalker : MonoBehaviour
{
    // Inspector-Editable Variables
    [SerializeField] float runSpeed = 20f;
	[SerializeField] float smoothing = .05f;
	[SerializeField] LayerMask playerMask;
	[SerializeField] LayerMask enemyMask;
	[SerializeField] LayerMask platformMask;
	[SerializeField] Transform platformCheckerLeft;
	[SerializeField] Transform platformCheckerRight;
	[SerializeField] const float platformCheckRadius = .05f;
    [SerializeField] const float knockbackOnHit = 300f;
	[SerializeField] Rigidbody2D rigidBody;
	[SerializeField] Animator animator;

	// Internal Variables
	Vector3 velocity = Vector3.zero;
	string facing = "Right";
    bool active = false;

    // FixedUpdate is a framerate independent update for physics calculations
	private void FixedUpdate()
	{
        if (!active)
        {
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, ~enemyMask);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                active = true;
                facing = "Left";
            }
            hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, ~enemyMask);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                active = true;
                facing = "Right";
            }
        }
        if (active)
        {
            Move();
        }
    }

    // CheckPlatformsLeft return true if there is a platform to enemy's left
    bool CheckPlatformsLeft()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(platformCheckerLeft.position, platformCheckRadius, platformMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				return true;
			}
		}
		return false;
    }

    // CheckPlatformsRight return true if there is a platform to enemy's right
    bool CheckPlatformsRight()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(platformCheckerRight.position, platformCheckRadius, platformMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				return true;
			}
		}
		return false;
    }

    // OnCollisionEnter runs when something touches or enters the collider
    private void OnCollisionEnter2D(Collision2D other) {
        // Bounce off of walls
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall")){
            Flip();
        }
        // Bounce off of enemies
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            Flip();
        }
    }

    // Move handles movement of the enemy
	void Move()
	{
        // Bounce off edges of platform
		if (facing == "Right" && !CheckPlatformsRight())
		{
			Flip();
		}
		else if (facing == "Left" && !CheckPlatformsLeft())
		{
			Flip();
		} 

        // Find and apply new velocity based on facing and speed
        float hDir;
        if (facing == "Right")
        {
            hDir = 1;
        }
        else
        {
            hDir = -1;
        }
		Vector3 newVelocity = new Vector3(10f * hDir * runSpeed * Time.deltaTime, rigidBody.velocity.y, 0);
		rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, newVelocity, ref velocity, smoothing);

    }

    // Flip reverses the direction of the enemy
    void Flip()
    {
        if (facing == "Right")
        {
            facing = "Left";
			GetComponent<SpriteRenderer>().flipX = true;
        }
        else 
        {
            facing = "Right";
			GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
