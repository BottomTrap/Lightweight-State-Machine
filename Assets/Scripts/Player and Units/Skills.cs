using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;

[RequireComponent(typeof(PlayerStats))]
//[RequireComponent(typeof(Animator))]
public class Skills : MonoBehaviour
{
    private Animator animator;
    private PlayerStats playerStats;
    private AI ai;

    public GameObject bullet;

    bool isCreated = false;

    GameObject[] spawnCount;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        ai = GetComponent<AI>();
        
    }
    public IEnumerator Attack()
    {
        if (!ai.hasPlayed){
        
        Debug.Log("EnemyAttack");
        yield return StartCoroutine(ai.HasPlayed()) ;
        }
    }

    public IEnumerator RangedAttack()
    {

        
        if (!ai.hasPlayed && !isCreated) {
            var heading = ai.target.position - transform.position;
            var rotation = Quaternion.LookRotation(heading);
            var projectile =Instantiate(bullet, transform.position, rotation);
            //bullet.transform.position = transform.position;
            projectile.transform.LookAt(ai.target);
            projectile.GetComponent<Rigidbody>().AddForce(heading*5.0f, ForceMode.Impulse);
            //projectile.GetComponent<Rigidbody>().velocity = transform.TransformDirection(heading * 5.0f);
            Destroy(projectile, 3);
            isCreated = true;
            Debug.Log("RangedEnemyAttack"); 
            yield return StartCoroutine(ai.HasPlayed());
        }   
    }

    public IEnumerator Cure()
    {
        if (ai.hasPlayed){
        Debug.Log("Enemy Casted cure on itself");
        yield return StartCoroutine(ai.HasPlayed());
        }
    }

    public IEnumerator Invisible()
    {
        Debug.Log("Invisible!");
        yield return StartCoroutine(ai.HasPlayed());
    }

    public IEnumerator Barrier()
    {
        Debug.Log("Barrier Cast");
        yield return StartCoroutine(ai.HasPlayed());
    }

    public IEnumerator DeathBlow()
    {
        Debug.Log("DeathBlow Attack!!");
        yield return StartCoroutine(ai.HasPlayed());
    }

    public IEnumerator CriticalHitUp()
    {
        Debug.Log("Critical Hit Up");
        yield return StartCoroutine(ai.HasPlayed());
    }
}