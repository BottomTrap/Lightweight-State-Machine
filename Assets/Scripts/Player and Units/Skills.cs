using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
//[RequireComponent(typeof(Animator))]
public class Skills : MonoBehaviour
{
    private Animator animator;
    private PlayerStats playerStats;


    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }
    public void Attack()
    {
        Debug.Log("EnemyAttacked");
    }

    public void RangedAttack()
    {
        Debug.Log("RangedEnemyAttack");
    }

    public void Cure()
    {
        Debug.Log("Enemy Casted cure on itself");
    }

    public void Invisible()
    {
        Debug.Log("Invisible!");
    }

    public void Barrier()
    {
        Debug.Log("Barrier Cast");
    }

    public void DeathBlow()
    {
        Debug.Log("DeathBlow Attack!!");
    }

    public void CriticalHitUp()
    {
        Debug.Log("Critical Hit Up");
    }
}
