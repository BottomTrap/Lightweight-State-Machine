using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RG;
using Panda;
using Random = UnityEngine.Random;

public class EnemyUnits : MonoBehaviour
{
    public Transform[] UnitsArrayTransforms;
    public List<Transform> UnitsList = new List<Transform>();
    public List<Transform> SeenPlayersTransforms= new List<Transform>();
    public int commandPoints;



    private void Awake()
    {
        GetChildObjectsAndPlayerTransforms();
    }

    void GetChildObjectsAndPlayerTransforms()
    {
        UnitsArrayTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < UnitsArrayTransforms.Length; i++)
        {
            if (UnitsArrayTransforms[i].GetComponentInChildren<AI>().isAlive)
            {
                UnitsList.Add(UnitsArrayTransforms[i]);
                
            }
            else
                continue;
        }
    }
   
    //** A function that goes through the visible PlayerUnits of every EnemyUnit (the children)
    Transform ChooseUnitTurn(List<Transform> unitsList)
    {
        Transform chosenTransform=unitsList[Mathf.RoundToInt(Random.Range(0,unitsList.Count))];
        int previousScore=0;
        for (int i = 0; i < unitsList.Count; i++)
        {
            
            int score = 0;
            score += hasPlayed(unitsList[i]) + IsThreatened(unitsList[i]) +
                     DistancefromVisibleUnits(unitsList[i], SeenPlayersTransforms) + AlliesLeft(unitsList[i]) +
                     UnitsThatThisCanKill(unitsList[i], SeenPlayersTransforms);
            
            if (i - 1 < 0) continue;
            if (score > previousScore)
            {
                
                chosenTransform = unitsList[i];
                if (score < 3)
                {
                    chosenTransform.GetComponent<AI>().aiModes=AI.AiModes.Cure;
                }

                if (score > 3 && score < 6)
                {
                    chosenTransform.GetComponent<AI>().target = GetLowestHP(SeenPlayersTransforms);
                    chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.RangedAttack;
                }

                if (score > 6)
                {
                    chosenTransform.GetComponent<AI>().target =
                        SeenPlayersTransforms[Mathf.RoundToInt(Random.Range(0, SeenPlayersTransforms.Count))];
                }
                previousScore = score;
            }
            else
            {
                continue;
            }
            

        }

        return chosenTransform;

    }


    #region EnemyAIChoiceFactors
    int hasPlayed(Transform unit)
    {
        if (unit.GetComponent<AI>().hasPlayed)
        {
            return -100;
        }
        else
        {
            return 1;
        }
    }

    int IsThreatened(Transform unit)
    {
        if ((unit.GetComponent<PlayerStats>().Health.Value / unit.GetComponent<PlayerStats>().startHealth) / 100.0 <
            50.0)
        {
            return -5;
        }
        else
        {
            return 1;
        }
    }

    int DistancefromVisibleUnits(Transform unit, List<Transform> seenPlayerTransforms)
    {
        int finalreturn = 0;
        List<Transform> sortedList =
            seenPlayerTransforms.OrderBy(o => Vector3.Distance(unit.position, o.position)).ToList();
        for (int i = 0; i < seenPlayerTransforms.Count; i++)
        {
            if (Vector3.Distance(unit.position, seenPlayerTransforms[i].position) <
                unit.GetComponent<PlayerStats>().AP.Value * 5)
            {
                finalreturn++;
            }else if (Vector3.Distance(unit.position, seenPlayerTransforms[i].position) >
                      unit.GetComponent<PlayerStats>().AP.Value * 10)
            {
                finalreturn--;
            }
        }

        return finalreturn;
    }

    int AlliesLeft(Transform unit)
    {
        int finalreturn = 0;
        for (int i = 0; i < UnitsList.Count; i++)
        {
            if (UnitsList[i] == unit)
            {
                continue;
            }

            if (UnitsList[i].GetComponent<PlayerStats>().isAlive)
            {
                finalreturn++;
            }
        }

        return finalreturn;

    }


    int UnitsThatThisCanKill(Transform unit, List<Transform> seenPlayerTransforms)
    {
        int finalreturn=0;
        for (int i=0; i < seenPlayerTransforms.Count; i++)
        {
            if (CanKill(unit, seenPlayerTransforms[i]))
            {
                finalreturn++;
            }
        }

        return finalreturn;
    }


    bool CanKill(Transform unit, Transform playerTransform)
    {
        if ((unit.GetComponent<PlayerStats>().Strength.Value /
             playerTransform.GetComponent<PlayerStats>().Defense.Value) >=
            playerTransform.GetComponent<PlayerStats>().Health.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
#endregion


    Transform GetLowestHP(List<Transform> transforms)
    {
        Transform chosenTransform;
        chosenTransform = transforms[0];
        for (int i = 1; i < transforms.Count; i++)
        {

            if (transforms[i].GetComponent<PlayerStats>().Health.Value <
                chosenTransform.GetComponent<PlayerStats>().Health.Value)
            {
                chosenTransform = transforms[i];
            }else 
                continue;
        }

        return chosenTransform;
    }



    //TO START EVERYTIME WE WANT TO CHOOSE A UNIT TO USE // MEANING BEFORE ChooseUnitTurn() 
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

    public List<Transform> PlayersInViewTransforms()
    {
        List<Transform> returnTransforms = new List<Transform>();
        if (SeenPlayersTransforms == null)
        {
            returnTransforms = null;
            return returnTransforms;
        }
        for (int i = 0; i < SeenPlayersTransforms.Count; i++)
        {
            Vector3 targetDir = SeenPlayersTransforms[i].transform.position - GetComponentInParent<Transform>().position;
            float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

            if (angleToPlayer >= -SeenPlayersTransforms[i].GetComponent<AI>().fov && angleToPlayer <= SeenPlayersTransforms[i].GetComponent<AI>().fov)
            {
                returnTransforms.Add(SeenPlayersTransforms[i].transform);
            }
        }

        return returnTransforms;
    }

    
}
