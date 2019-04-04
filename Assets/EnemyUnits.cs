using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RG;
using Panda;

public class EnemyUnits : MonoBehaviour
{
    public List<Transform> UnitsList;
    public List<Transform> SeenPlayersTransforms;


    //** A function that goes through the visible PlayerUnits of every EnemyUnit (the children)

    [Task]
    public bool GetPlayersInRange(float AP, float range, Transform currentUnit)
    {
        Collider[] seenPlayerColliders = Physics.OverlapSphere(currentUnit.position, AP + range);
        int i = 0;
        while (i < seenPlayerColliders.Length)
        {
            if(seenPlayerColliders[i].gameObject.tag =="PlayerUnit")
            SeenPlayersTransforms.Add(seenPlayerColliders[i].transform);
            i++;
        }

        return true;
    }
}
