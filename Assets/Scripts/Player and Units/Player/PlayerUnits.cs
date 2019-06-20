using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RG;

public class PlayerUnits : MonoBehaviour
{
    private Transform[] playerUnitsArrayTransform;
    public List<Transform> playerUnitsTransformList;

    public void GetChildObjectsTransforms()
    {
        playerUnitsArrayTransform = GetComponentsInChildren<Transform>();

       //for (int i = 0; i < playerUnitsArrayTransform.Length; i++)
       //{
       //   
       //    //if (playerUnitsArrayTransform[i] == this.transform) continue;
       //    
       //    if (playerUnitsArrayTransform[i].GetComponent<PlayerMovement>().isAlive && !playerUnitsTransformList.Contains(playerUnitsArrayTransform[i]))
       //    {
       //        playerUnitsTransformList.Add(playerUnitsArrayTransform[i]);
       //    }
       //    else
       //        continue;
       //}

        foreach (Transform child in transform)
        {
            if(child.GetComponent<PlayerMovement>().isAlive && !playerUnitsTransformList.Contains(child))
            {
                playerUnitsTransformList.Add(child);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GetChildObjectsTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
