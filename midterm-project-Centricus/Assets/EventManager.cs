using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void RecallAction(Transform playerPosition);
    public static event RecallAction Recall;

    public delegate void RecalledAction();
    public static event RecalledAction Recalled;

    public static void InvokeRecall(Transform target)
    {
        if (Recall != null)
        {
            Recall(target);
        }
    }

    public static void InvokeRecalled() {
        Recalled();
    }
}
