using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using riadh.Stats;

public class PlayerStats : MonoBehaviour
{
    
    public enum Classes
    {
        Rounin = 100,
        Gunner = 200,
        Ninja = 300,
        Underling = 400,
        UnderlingBoss = 500,
    }
    public Classes CharacterClass;

    
    public CharacterStat Strength, Defense, Speed,AP,Health;

   
    

    private void Start()
    {
        switch (CharacterClass)
        {
            case Classes.Rounin:
                {
                    Strength =  new CharacterStat(10);
                    Defense = new CharacterStat(7);
                    Speed = new CharacterStat(7);
                    AP = new CharacterStat(7);
                    Health = new CharacterStat(6);
                    break;
                }
            case Classes.Gunner:
                {
                    Strength = new CharacterStat(7);
                    Defense = new CharacterStat(14);
                    Speed = new CharacterStat(5);
                    AP = new CharacterStat(4);
                    Health = new CharacterStat(10);
                    break;
                }
            case Classes.Ninja:
                {
                    Strength = new CharacterStat(6);
                    Defense = new CharacterStat(7);
                    Speed = new CharacterStat(10);
                    AP = new CharacterStat(14);
                    Health = new CharacterStat(6);
                    break;
                }
            case Classes.Underling:
                {
                    Strength = new CharacterStat(6);
                    Defense = new CharacterStat(6);
                    Speed = new CharacterStat(6);
                    AP = new CharacterStat(6);
                    Health = new CharacterStat(6);
                    break;
                }
            case Classes.UnderlingBoss:
                {
                    Strength = new CharacterStat(10);
                    Defense = new CharacterStat(7);
                    Speed = new CharacterStat(4);
                    AP = new CharacterStat(5);
                    Health = new CharacterStat(7);
                    break;
                }
        }
        

    }
}
