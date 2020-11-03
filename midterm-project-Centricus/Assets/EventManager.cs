using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void RecallAction(Transform playerPosition);
    public static event RecallAction Recall;

    public delegate void RecalledAction();
    public static event RecalledAction Recalled;

    public delegate void DamagePlayerAction(int damage, Transform enemyPosition);
    public static event DamagePlayerAction DamagePlayer;

    public delegate void DamageEnemyAction(int damage, Transform hitboxPosition, GameObject enemy);
    public static event DamageEnemyAction DamageEnemy;

    public static void InvokeRecall(Transform target)
    {
        if (Recall != null)
        {
            Recall(target);
        }
    }

    public static void InvokeRecalled() 
    {
        Recalled();
    }

    public static void InvokeDamagePlayer(int damage, Transform enemyPosition)
    {
        DamagePlayer(damage, enemyPosition);
    }

    public static void InvokeDamageEnemy(int damage, Transform hitboxPosition, GameObject enemy)
    {
        DamageEnemy(damage, hitboxPosition, enemy);
    }
}
