using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
using RG;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;


public class EnemyUnit : Conditional
{
    private int commandPoints = 6;

    private void Update()
    {
        

    }

    public override TaskStatus OnUpdate()
    {
        if (commandPoints > 0)
        {
            return TaskStatus.Success;
        }else
        return TaskStatus.Failure;

    }

    public override void OnEnd()
    {
        if (commandPoints > 0)
            commandPoints -= 1;
    }


}
