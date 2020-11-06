using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPurchase : MonoBehaviour
{
    bool playerInside = false;

    private void Update() {
        if (playerInside && Input.GetButtonDown("Vertical"))
        {
            EventManager.InvokeHealPlayer(1);
            EventManager.InvokeRemoveMoney(1500);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.Find("Prompt").gameObject.SetActive(true);
            playerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.Find("Prompt").gameObject.SetActive(false);
            playerInside = false;
        }
    }
}
