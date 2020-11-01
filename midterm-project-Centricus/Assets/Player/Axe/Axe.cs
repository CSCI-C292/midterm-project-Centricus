using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    bool recalling = false;
    bool stuck = false;
    
	// FixedUpdate is a framerate independent update for physics calculations
    private void FixedUpdate() {
        if (stuck)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
        else 
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
            Move();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            stuck = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // DAMAGE ENEMY
        }
        if (recalling && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Send recalled event
            Destroy(gameObject);
        }
    }

    // TODO: Recall event. Stuck = false, recalling = true

    // Move handles movement for the axe
    void Move() 
    {
        if (recalling)
        {
            // Move in direction of player
        }
        else {

        }
    }
}
