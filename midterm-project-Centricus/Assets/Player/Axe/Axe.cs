using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    // Inspector-Editable Variables
    [SerializeField] string facing;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] Animator animator;

    // Internal Variables
    bool recalling = false;
    bool hasStuck = false;
    bool stuck = false;
    int damage = 1;
    float speed = 60f;
    Vector3 velocity;
    float smoothing = 0.05f;
    Transform recallTarget;
    List<GameObject> hitTargets = new List<GameObject>();

    // Start is called before the first frame update
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
    
    // Check collisions (only applies while axe not stuck in a wall/platform)
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") 
            && !hitTargets.Contains(other.gameObject))
        {
            EventManager.InvokeDamageEnemy(damage, transform, other.gameObject);
        }
        if (!recalling)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall") 
                || other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                // Avoid bugs by picking up the axe if the player is touching it upon wall collision
                // if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("Player")))
                // {
                //     PickUp();
                // }
                // Change relevant values
                stuck = true;
                hasStuck = true;
                gameObject.layer = LayerMask.NameToLayer("Platform");
                animator.SetBool("Stuck", true);
                GetComponent<BoxCollider2D>().isTrigger = false;
                // Clear hitTargets
                hitTargets.Clear();
            }
        }
        if (recalling && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickUp();
        }
    }

    // This redundancy was necessary to fix a bug where the axe would not return to the player
    private void OnTriggerStay2D(Collider2D other) {
        if (recalling && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PickUp();
        }
    }

    // PickUp returns the axe to the player, destroying the axe GameObject
    void PickUp()
    {
        hitTargets.Clear();
        EventManager.InvokeRecalled();
        EventManager.Recall -= Recall;
        Destroy(gameObject);
    }

    // While the Recall event is being invoked, this function causes the axe to
    // return to the player.
    void Recall(Transform target)
    {
        if (!recalling && hasStuck)                                 
        {
            animator.SetBool("Recalling", true);
            animator.SetBool("Stuck", false);
            recalling = true;
            stuck = false;
            rb2D.constraints = RigidbodyConstraints2D.None;
            GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Decorative");
            
        }
        recallTarget = target;
    }

    // Move handles movement for the axe
    void Move() 
    {
        if (recalling && hasStuck)
        {
            Vector3 dir = recallTarget.position - transform.position;
            dir = dir.normalized;
            rb2D.velocity = 10f * dir * speed * Time.deltaTime;
            if (rb2D.velocity.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (rb2D.velocity.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
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
