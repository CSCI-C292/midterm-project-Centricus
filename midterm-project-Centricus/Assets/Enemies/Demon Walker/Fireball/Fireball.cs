using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb2d;

    Vector3 direction = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            EventManager.InvokeDamagePlayer(1, transform);
        }
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        Destroy(gameObject);
    }

    public void setDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void FixedUpdate() {
        // Movement
        rb2d.velocity = (direction * speed * Time.deltaTime * 10f);
        if (rb2d.velocity.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
