using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RG;

public class PlayerUnits : MonoBehaviour //This short script updates the number of player ALIVE that are inside the PlayerUnits Gameobject
{
    private Transform[] playerUnitsArrayTransform;
    public List<Transform> playerUnitsTransformList;

    public void GetChildObjectsTransforms()
    {
        playerUnitsArrayTransform = GetComponentsInChildren<Transform>();

        foreach (Transform child in this.transform)
        {
            if (!playerUnitsTransformList.Contains(child))
            {
                playerUnitsTransformList.Add(child);
            }
                
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        GetChildObjectsTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        GetChildObjectsTransforms();
    }
}
