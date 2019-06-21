using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using UnityEngine;
using RG;
using Random = UnityEngine.Random;

public class EnemyUnits : MonoBehaviour
{
    public Transform[] UnitsArrayTransforms;
    public List<Transform> UnitsList = new List<Transform>();
    public List<Transform> SeenPlayersTransforms = new List<Transform>();
    public Transform currentUnit;
    public int commandPoints = 6;
    public int originalCommandPoints;

    private int originalUnitNumbers;


    private void Awake()
    {
        originalCommandPoints = commandPoints;
        GetChildObjectsTransforms();
    }

    private void Update()
    {
        GetChildObjectsTransforms();
        PlayersInViewTransforms();

        originalUnitNumbers = UnitsList.Count;
    }

    void CheckForDeath()
    {
        foreach(Transform unit in UnitsList)
        {
            if(unit.GetComponent<AI>().isAlive == false)
            {
                UnitsList.Remove(unit);
            }
        }
    }

    public void GetChildObjectsTransforms()
    {
       
        foreach (Transform child in transform) {
            if (child.GetComponent<AI>().isAlive && !UnitsList.Contains(child))
            UnitsList.Add(child);
        }
    }


    //Get a rabdom point on an endge of a circle (not inside the circle) so that the enemies will be randomly around the player
    private Vector3 RandomPointOnCircleEdge(float radius)
{
    var vector2 = Random.insideUnitCircle.normalized * radius;
    return new Vector3(vector2.x, 0, vector2.y);
}

  
    //** A function that goes through the visible PlayerUnits of every EnemyUnit (the children)
    public Transform ChooseUnitTurn()
    {
        //Transform chosenTransform = UnitsList[Mathf.RoundToInt(Random.Range(0, UnitsList.Count))];
        Transform chosenTransform;
        for (int i = 0; i < UnitsList.Count; i++)
        {
            if (UnitsList[i])
            {
                UnitsList[i].GetComponent<AI>().score = 0;
                UnitsList[i].GetComponent<AI>().score = FullScore(UnitsList[i]);
                Debug.Log("full score = " + FullScore(UnitsList[i]));
            }
        }

        chosenTransform = UnitsList.MaxBy(unit => unit.GetComponent<AI>().score); //Linq
        List<Transform> playerList= new List<Transform>();
        if (chosenTransform.GetComponent<AI>().playersInView.Count > 0)
        {
            //Get players in view by all enemies
            playerList = chosenTransform.GetComponent<AI>().playersInView;
        }
        int chosenScore = chosenTransform.GetComponent<AI>().score;
        
        if (chosenScore < 3 || playerList.Count <=0)
        {
            chosenTransform.GetComponent<AI>().target = chosenTransform;
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.Cure;
        }

        if (chosenScore > 3 && chosenScore < 6 && playerList.Count > 0)
        {
            chosenTransform.GetComponent<AI>().target = GetLowestHP(playerList);
            chosenTransform.GetComponent<AI>().offset = chosenTransform.GetComponent<AI>().RandomPointOnCircleEdge(Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2);
            if (!chosenTransform.GetComponent<AI>().PathComplete(chosenTransform.GetComponent<AI>().target.position - chosenTransform.GetComponent<AI>().offset))
            {
                chosenTransform.GetComponent<AI>().target = GetLowestHP(playerList);
                chosenTransform.GetComponent<AI>().offset = chosenTransform.GetComponent<AI>().RandomPointOnCircleEdge(Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2);
            }
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.RangedAttack;
        }

        if (chosenScore > 6 && playerList.Count >0)
        {
            chosenTransform.GetComponent<AI>().target = ClosestTarget(chosenTransform,playerList);
            chosenTransform.GetComponent<AI>().offset = chosenTransform.GetComponent<AI>().RandomPointOnCircleEdge(1.5f);
            if (!chosenTransform.GetComponent<AI>().PathComplete(chosenTransform.GetComponent<AI>().target.position - chosenTransform.GetComponent<AI>().offset))
            {
                chosenTransform.GetComponent<AI>().target = ClosestTarget(chosenTransform,playerList);
                chosenTransform.GetComponent<AI>().offset = chosenTransform.GetComponent<AI>().RandomPointOnCircleEdge(1);
            }
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.Attack;
        }

        return chosenTransform;
    }


    //Functions that determine the Enemy's choice , could easily be modified
    #region EnemyAIChoiceFactors
    public int hasPlayed(Transform unit)
    {
        if (unit.GetComponent<AI>().hasPlayed)
        {
            return -10;
        }
        else
        {
            return 1;
        }
    }

    public int IsThreatened(Transform unit)
    {
        if (unit.GetComponent<PlayerStats>().startHealth<unit.GetComponent<PlayerStats>().Health.Value/2)
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
      
        if (seenPlayerTransforms != null)
        {
            foreach (Transform seenUnit in seenPlayerTransforms)
            {
                if (Vector3.Distance(unit.position, seenUnit.position) <
                    unit.GetComponent<PlayerStats>().AP.Value * 5)
                {
                    finalreturn = 5;
                    //Debug.Log("famma visible units");
                    break;
                }
            }
        }
        else
        {
            finalreturn = -10;
        }
        return finalreturn;
    }

    public int AlliesLeft(Transform unit)
    {
        int finalreturn = 0;
        for (int i = 0; i < UnitsList.Count; i++)
        {
            if (UnitsList[i])
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
        }
        //Debug.Log(finalreturn + " allies left");
        return finalreturn;

    }


    public int UnitsThatThisCanKill(Transform unit, List<Transform> seenPlayerTransforms)
    {
        int finalreturn = 0;
         foreach (Transform seenUnit in seenPlayerTransforms)
        {
            if (CanKill(unit,seenUnit))
            {
                finalreturn = 10;
                break;
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
            }
            else
                continue;
        }

        return chosenTransform;
    }

    public Transform ClosestTarget(Transform unit,List<Transform> seenTransforms)
    {   
        Transform closestTransform;
        closestTransform = seenTransforms[0];
        for (int i =1 ; i <seenTransforms.Count;i++) 
        {
            if (Vector3.Distance(unit.position,seenTransforms[i].position)<Vector3.Distance(unit.position,closestTransform.position))
            {
                closestTransform= seenTransforms[i];
            }
        }
        return closestTransform;
    }

    public int FullScore(Transform unitTransform)
    {
        int score = 0;
        List<Transform> playersInViewList = unitTransform.GetComponent<AI>().playersInView;
        score = hasPlayed(unitTransform) + IsThreatened(unitTransform) +
                     DistancefromVisibleUnits(unitTransform, playersInViewList) + AlliesLeft(unitTransform) +
                     UnitsThatThisCanKill(unitTransform, playersInViewList);
        Debug.Log("is threatened is  "+IsThreatened(unitTransform));
        return score;
    }
    #endregion

    //TO START EVERYTIME WE WANT TO CHOOSE A UNIT TO USE // MEANING BEFORE ChooseUnitTurn() 

    public void GetPlayersInRange()
    {
        for (int j = 0; j < UnitsList.Count; j++)
        {
            if (UnitsList[j])
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
        }
    } //Get players in range, even behind

    public void PlayersInViewTransforms()
    {
       
        if (SeenPlayersTransforms == null)
        {
            //Debug.Log("seen player transforms is null");
            
            return;
        }
        for (int j = 0; j < UnitsList.Count; j++)
        {
            if (UnitsList[j])
            {
               UnitsList[j].GetComponent<AI>().playersInView = new List<Transform>(); // reinitializes the List of players in view
               var transformList = UnitsList[j].GetComponent<AI>().playersInView;  
               for (int i = 0; i < SeenPlayersTransforms.Count; i++)
                {
                    if (IsInView(UnitsList[j].gameObject, SeenPlayersTransforms[i].gameObject) && !transformList.Contains(SeenPlayersTransforms[i]))
                    {
                        transformList.Add(SeenPlayersTransforms[i]);
                    }
                }

            }
            else continue;
        }
       
    }


    //Uses the borders of the camera inside each enemy and checks between them for the player with a raycast
    private bool IsInView(GameObject origin, GameObject toCheck)
    {
        var cam = origin.GetComponentInChildren<Camera>();
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > (float)cam.pixelWidth) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > (float)cam.pixelHeight))
        {
            Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - origin.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.tag == "EnemyUnit" || hit.transform.tag == "PlayerUnit" || hit.transform.tag =="Weapon"||hit.transform.tag =="PlayerWeapon")
            {
                return true;
            }else
            if (hit.transform.name != toCheck.name)
            {
                
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError("origin"+origin.name+ toCheck.name + " occluded by " + hit.transform.name);
                
                Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        
        return true;
    }
}