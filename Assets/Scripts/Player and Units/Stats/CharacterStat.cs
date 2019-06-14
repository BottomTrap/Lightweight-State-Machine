using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace riadh.Stats
{
    [Serializable]
    public class CharacterStat
    {
        #region PublicVariables
        public float BaseValue;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        #endregion


        public virtual float Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;

                }
                return _value;
            }
        }

        #region protectedVariables
        protected float lastBaseValue = float.MinValue;
        protected bool isDirty = true; // to avoid calling CalculateFinalValue() every single time and call it only when a change has occured
        protected float _value;
        protected readonly List<StatModifier> statModifiers;
        #endregion

        #region Constructor
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }
        #endregion

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0; // This will hold the sum of our "PercentAdd" modifiers
            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModifier.StatModType.Flat)
                {
                    finalValue += statModifiers[i].Value;
                }
                else if (mod.Type == StatModifier.StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value; //add all percentAdd modifiers we meet

                    // if we reached end of list or the modifier li 3anna isn't percentAdd
                    if (i+ 1 >=statModifiers.Count || statModifiers[i+1].Type != StatModifier.StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd; // Multiply the sum with the final value , kima percentMult
                        sumPercentAdd = 0; // reset, rajja3 kima kenet, for next uses
                    }
                }
                else if (mod.Type == StatModifier.StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                    //This is the equivalent of "finalValue += finalValue * mod.Value" 
                }
            }
            //Rounding to 4 digits after the Zero 
            return (float)Math.Round(finalValue, 4);
        }

        protected virtual int CompareModifierOrder (StatModifier a, StatModifier b) // if returns 1 , it will go the standard order in the enum, else it will go opposite order and sort it in that order
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if both are equal, so same priority
        }
         
        public virtual bool RemoveAllModifiersFromSource (object source) //loop is in reverse to not edit the order of the items in the list and go through all of them and not reach null reference
        {
            bool didRemove = false; //still isn't removed

            for (int i = statModifiers.Count -1; i>=0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

    }
}
