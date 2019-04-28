﻿using System;
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



    private void Awake()
    {
        originalCommandPoints = commandPoints;
        GetChildObjectsTransforms();
    }

    private void Update()
    {
        GetChildObjectsTransforms();
    }

    public void GetChildObjectsTransforms()
    {
        //UnitsArrayTransforms = GetComponentsInChildren<Transform>();
        //for (int i = 0; i < UnitsArrayTransforms.Length; i++)
        //{
        //    if (UnitsArrayTransforms[i] == this.transform) continue;
        //    if (UnitsArrayTransforms[i].GetComponentInChildren<AI>().isAlive && !UnitsList.Contains(UnitsArrayTransforms[i]))
        //    {
        //        UnitsList.Add(UnitsArrayTransforms[i]);
        //    }
        //    else
        //        continue;
        //}
        foreach (Transform child in transform) {
            if (child.GetComponent<AI>().isAlive && !UnitsList.Contains(child))
            UnitsList.Add(child);
        }
    }
        private Vector3 RandomPointOnCircleEdge(float radius)
{
    var vector2 = Random.insideUnitCircle.normalized * radius;
    return new Vector3(vector2.x, 0, vector2.y);
}

   // private Vector3 RandomPointOnCircleEdgeWhereObjectIsStillInView(float radius, Transform obj)
   // {
   //     var randomPoint =RandomPointOnCircleEdge(radius);
   //     var direction = obj.position - randomPoint;
   //     
   //     
   // }
    //** A function that goes through the visible PlayerUnits of every EnemyUnit (the children)
    public Transform ChooseUnitTurn()
    {
        Transform chosenTransform = UnitsList[Mathf.RoundToInt(Random.Range(0, UnitsList.Count))];

        //foreach (Transform unit in UnitsList)
        //{
        //    unit.GetComponent<AI>().score = 0;
        //   // Debug.Log("FIRST SCORE" + unit.GetComponent<AI>().score);
        //    unit.GetComponent<AI>().score = FullScore(unit);
        //    //Debug.Log("END SCORE" + unit.GetComponent<AI>().score);
        //}


        for (int i = 0; i < UnitsList.Count; i++)
        {
            UnitsList[i].GetComponent<AI>().score = 0;
            UnitsList[i].GetComponent<AI>().score = FullScore(UnitsList[i]);
            Debug.Log("full score = "+FullScore(UnitsList[i]));
        }

        chosenTransform = UnitsList.MaxBy(unit => unit.GetComponent<AI>().score);
        int chosenScore = chosenTransform.GetComponent<AI>().score;
        if (chosenScore < 3)
        {
            chosenTransform.GetComponent<AI>().target = chosenTransform;
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.Cure;
        }

        if (chosenScore > 3 && chosenScore < 6)
        {
            chosenTransform.GetComponent<AI>().target = GetLowestHP(SeenPlayersTransforms);
            chosenTransform.GetComponent<AI>().offset =RandomPointOnCircleEdge(Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2); //chosenTransform.forward* Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2; 
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.RangedAttack;
        }

        if (chosenScore > 6)
        {

            chosenTransform.GetComponent<AI>().target = GetLowestHP(SeenPlayersTransforms);
            chosenTransform.GetComponent<AI>().offset =RandomPointOnCircleEdge(Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2); //chosenTransform.forward* Vector3.Distance(chosenTransform.position, chosenTransform.GetComponent<AI>().target.position) / 2; 
            chosenTransform.GetComponent<AI>().aiModes = AI.AiModes.Attack;
        }

        return chosenTransform;
    }


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
        List<Transform> sortedList =
            seenPlayerTransforms.OrderBy(o => Vector3.Distance(unit.position, o.position)).ToList();
       // for (int i = 0; i < seenPlayerTransforms.Count; i++)
       // {
       //     if (Vector3.Distance(unit.position, seenPlayerTransforms[i].position) <
       //         unit.GetComponent<PlayerStats>().AP.Value * 5)
       //     {
       //         finalreturn++;
       //     }
       //     else if (Vector3.Distance(unit.position, seenPlayerTransforms[i].position) >
       //              unit.GetComponent<PlayerStats>().AP.Value * 10)
       //     {
       //         finalreturn--;
       //     }
       // }
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
//Debug.Log("final return ISS" + finalreturn);
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
        //Debug.Log(finalreturn + " allies left");
        return finalreturn;

    }


    public int UnitsThatThisCanKill(Transform unit, List<Transform> seenPlayerTransforms)
    {
        int finalreturn = 0;
       //for (int i = 0; i < seenPlayerTransforms.Count; i++)
       //{
       //    if (CanKill(unit, seenPlayerTransforms[i]))
       //    {
       //        finalreturn++;
       //    }
       //}
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

    public int FullScore(Transform unitTransform)
    {
        int score = 0;
        score = hasPlayed(unitTransform) + IsThreatened(unitTransform) +
                     DistancefromVisibleUnits(unitTransform, PlayersInViewTransforms()) + AlliesLeft(unitTransform) +
                     UnitsThatThisCanKill(unitTransform, PlayersInViewTransforms());
        Debug.Log("is threatened is  "+IsThreatened(unitTransform));
        return score;
    }
    #endregion






    //TO START EVERYTIME WE WANT TO CHOOSE A UNIT TO USE // MEANING BEFORE ChooseUnitTurn() 

    public void GetPlayersInRange()
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
            //Debug.Log("seen player transforms is null");
            returnTransforms = null;
            return returnTransforms;
        }
        for (int j = 0; j < UnitsList.Count; j++)
        {
            //var fov = UnitsList[j].GetComponent<AI>().fov;
            for (int i = 0; i < SeenPlayersTransforms.Count; i++)
            {
                if (IsInView(UnitsList[j].gameObject, SeenPlayersTransforms[i].gameObject))
                {
                    Debug.Log("FAMA HAJA WALA LE");
                    returnTransforms.Add(SeenPlayersTransforms[i]);
                }
            }
        }
        return returnTransforms;
    }
    //Get players in view from the inRange players
    //public bool PlayerInView(Transform source, Transform target)
    //{
    //    Vector3 targetDir = target.position - source.position;
    //    float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
    //
    //    if (angleToPlayer >= -
    //}

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
            if (hit.transform.name != toCheck.name)
            {
                
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                
                Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        Debug.Log("FAMA HAJA");
        return true;
    }
}