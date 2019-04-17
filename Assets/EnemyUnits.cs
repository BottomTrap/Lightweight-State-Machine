using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RG;
using Random = UnityEngine.Random;

public class EnemyUnits : MonoBehaviour
{
    public Transform[] UnitsArrayTransforms;
    public List<Transform> UnitsList = new List<Transform>();
    public List<Transform> SeenPlayersTransforms= new List<Transform>();
    public Transform currentUnit;
    public int commandPoints=6;
    public int originalCommandPoints;



    private void Awake()
    {
        originalCommandPoints = commandPoints;
        GetChildObjectsTransforms();
    }

    public void GetChildObjectsTransforms()
    {
        UnitsArrayTransforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < UnitsArrayTransforms.Length; i++)
        {
            if (UnitsArrayTransforms[i] == this.transform) continue;
            if (UnitsArrayTransforms[i].GetComponentInChildren<AI>().isAlive && !UnitsList.Contains(UnitsArrayTransforms[i]))
            {
                UnitsList.Add(UnitsArrayTransforms[i]);
            }
            else
                continue;
        }
    }
   
    //** A function that goes through the visible PlayerUnits of every EnemyUnit (the children)
    public Transform ChooseUnitTurn()
    {
        Transform chosenTransform=UnitsList[Mathf.RoundToInt(Random.Range(0,UnitsList.Count))];
        int previousScore=0;
        foreach (Transform g in UnitsList)
        {
            
            int score = 0;
            score += hasPlayed(g) + IsThreatened(g) +
                     DistancefromVisibleUnits(g, SeenPlayersTransforms) + AlliesLeft(g) +
                     UnitsThatThisCanKill(g, SeenPlayersTransforms);
            
           
            
            if (score > previousScore && score >0)
            {
                
                chosenTransform = g;
                if (score < 3)
                {
                    chosenTransform.GetComponent<AI>().target = chosenTransform;
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
    public int hasPlayed(Transform unit)
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

    public int IsThreatened(Transform unit)
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

    public int DistancefromVisibleUnits(Transform unit, List<Transform> seenPlayerTransforms)
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

    public int AlliesLeft(Transform unit)
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


    public int UnitsThatThisCanKill(Transform unit, List<Transform> seenPlayerTransforms)
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


    public bool CanKill(Transform unit, Transform playerTransform)
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


    public Transform GetLowestHP(List<Transform> transforms)
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
    
    public void GetPlayersInRange( )
    {
        for (int j = 0; j < UnitsList.Count; j++)
        {
            Collider[] seenPlayerColliders = Physics.OverlapSphere(UnitsList[j].position, UnitsList[j].GetComponent<PlayerStats>().AP.Value + UnitsList[j].GetComponent<PlayerStats>().Range.Value);
            int i = 0;
            while (i < seenPlayerColliders.Length)
            {
                if (seenPlayerColliders[i].gameObject.tag == "PlayerUnit")
                    SeenPlayersTransforms.Add(seenPlayerColliders[i].transform);
                i++;
            }
        }


    } //Get players in range, even behind

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
    //Get players in view from the inRange players

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(2.0f);
    }

}
