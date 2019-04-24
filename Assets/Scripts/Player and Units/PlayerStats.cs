using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using riadh.Stats;
using UnityEditor;
using UnityEngine.SocialPlatforms;

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

    public enum Skills
    {
        Cure=100,
        Barrier =200,
        Invisible =300,
        DeathBlow=400,
        CritHitUp=500,
    }
    public Skills CharacterSkills;

    public CharacterStat Strength, Defense, Speed, AP, Health, Range, GunStrength, HitRate;

    public float startHealth;

    
    public bool isAlive = true;

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
                    Range = new CharacterStat(5);
                    GunStrength = new CharacterStat(5);
                    HitRate = new CharacterStat(10); //10 means perfect
                    //Rounin Mesh
                    //Rounin Weapon
                    //Rounin Gun
                    break;
                }
            case Classes.Gunner:
                {
                    Strength = new CharacterStat(7);
                    Defense = new CharacterStat(14);
                    Speed = new CharacterStat(5);
                    AP = new CharacterStat(4);
                    Health = new CharacterStat(10);
                    Range = new CharacterStat(13);
                    GunStrength = new CharacterStat(14);
                    HitRate = new CharacterStat(8);
                    //Gunner Mesh
                    //Gunner Weapon
                    //Gunner Gun
                    break;
                }
            case Classes.Ninja:
                {
                    Strength = new CharacterStat(6);
                    Defense = new CharacterStat(7);
                    Speed = new CharacterStat(10);
                    AP = new CharacterStat(14);
                    Health = new CharacterStat(6);
                    Range = new CharacterStat(10);
                    GunStrength = new CharacterStat(4);// with special ability to make enemy sleep
                    HitRate = new CharacterStat(9);
                    //Ninja Mesh
                    //Ninja Weapon
                    //Ninja Gun (Darts or whatever)
                    break;
                }
            case Classes.Underling:
                {
                    Strength = new CharacterStat(6);
                    Defense = new CharacterStat(6);
                    Speed = new CharacterStat(6);
                    AP = new CharacterStat(6);
                    Health = new CharacterStat(6);
                    Range = new CharacterStat(6);
                    GunStrength = new CharacterStat(5);
                    HitRate = new CharacterStat(9);
                    //Underling Mesh
                    //Underling Weapon
                    //Underling Gun
                    break;
                }
            case Classes.UnderlingBoss:
                {
                    Strength = new CharacterStat(10);
                    Defense = new CharacterStat(7);
                    Speed = new CharacterStat(4);
                    AP = new CharacterStat(5);
                    Health = new CharacterStat(7);
                    Range = new CharacterStat(5);
                    GunStrength = new CharacterStat(15);
                    HitRate = new CharacterStat(6);
                    //Underling Boss Mesh
                    //Underling Boss Weapon
                    //Underling Boss Gun

                    break;
                }
        }

        startHealth = this.Health.Value;

    }
}
