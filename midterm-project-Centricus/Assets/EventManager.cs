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

    public delegate void HealPlayerAction(int healing);
    public static event HealPlayerAction HealPlayer;
    
    public delegate void RemoveMoneyAction(int healing);
    public static event RemoveMoneyAction RemoveMoney;

    public static void InvokeRecall(Transform target)
    {
        if (Recall != null)
        {
            Recall(target);
        }
    }

    public static void InvokeRecalled() 
    {
        if (Recalled != null)
        {
            Recalled();
        }
    }

    public static void InvokeDamagePlayer(int damage, Transform enemyPosition)
    {
        if (DamagePlayer != null)
        {
            DamagePlayer(damage, enemyPosition);
        }
    }

    public static void InvokeDamageEnemy(int damage, Transform hitboxPosition, GameObject enemy)
    {
        if (DamageEnemy != null)
        {
            DamageEnemy(damage, hitboxPosition, enemy);
        }
    }

    public static void InvokeHealPlayer(int healing)
    {
        if (HealPlayer != null)
        {
            HealPlayer(healing);
        }
    }

    public static void InvokeRemoveMoney(int amount)
    {
        if (RemoveMoney != null)
        {
            RemoveMoney(amount);
        }
    }
}
