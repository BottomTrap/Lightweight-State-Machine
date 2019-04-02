using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
using Panda;

public class EnemyUnit : MonoBehaviour
{
    private PlayerStats playerStats;
   
    private void Update()
    {
        playerStats = (PlayerStats) GetComponent(typeof(PlayerStats));

    }

    private void FixedUpdate()
    {
        

    }

    [Task]
    void IdleShoot()
    {

    }

    [Task]
    void IsHealthLessThan(float health)
    {

    }

    [Task]
    void MoveTo()
    {

    }


}
