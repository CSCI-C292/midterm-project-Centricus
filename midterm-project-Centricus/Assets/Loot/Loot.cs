using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] int worth;

    public int PickUp()
    {
        int value = worth;
        Destroy(gameObject);
        return value;
    }
}
