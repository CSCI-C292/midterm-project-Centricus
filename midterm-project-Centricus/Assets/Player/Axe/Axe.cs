﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] string facing;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;

    bool recalling = false;
    bool stuck = false;
    float speed = 60f;
    Vector3 velocity;
    float smoothing = 0.05f;
    Transform recallTarget;

    // Start is called on the frame when a script is enabled
    private void Start() {
        EventManager.Recall += Recall;
    }
    
	// FixedUpdate is a framerate independent update for physics calculations
    private void FixedUpdate() {
        if (!stuck)
        {
            Move();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (!recalling)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                stuck = true;
                gameObject.layer = LayerMask.NameToLayer("Platform");
                animator.SetBool("Stuck", true);
                GetComponent<BoxCollider2D>().isTrigger = false;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // DAMAGE ENEMY
            }
        }
        if (recalling && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.InvokeRecalled();
            EventManager.Recall -= Recall;
            Destroy(gameObject);

        }
    }

    void Recall(Transform target)
    {
        if (!recalling)                                 
        {
            animator.SetBool("Recalling", true);
            rb2D.constraints = RigidbodyConstraints2D.None;
            recalling = true;
            stuck = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Decorative");
            
        }
        recallTarget = target;
    }

    // Move handles movement for the axe
    void Move() 
    {
        if (recalling)
        {
            Vector3 dir = recallTarget.position - transform.position;
            dir = dir.normalized;
            rb2D.velocity = 10f * dir * speed * Time.deltaTime;
            // Facing depends on velocity.x
        }
        else {
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
            Vector3 newVelocity = new Vector3(10f * hDir * speed * Time.deltaTime, rb2D.velocity.y, 0);
            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, newVelocity, ref velocity, smoothing);
        }
    }
}
