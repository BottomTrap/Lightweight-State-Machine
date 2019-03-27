namespace riadh.Stats
{
    public class StatModifier
    {
        public enum StatModType
        {
            Flat =100,
            PercentAdd=200,
            PercentMult=300,
        }
        public readonly float Value; //value of how much you want to change the stat
        public readonly StatModType Type; // value of HOW you want to change the stat ( * percent or + add flat value) 
        public readonly int Order;
        public readonly object Source; // the source of each modifier, meaning from which "object" or "item" or "powerup" or "buff" or "debuff" it came from


        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        //A Constructor that add a default Order value (similar in Unity where you chose the way you use a contructor) in case the user doesn't want to manually define it for some reason
        //calling a constructor from another constructor , like extending the constructor, so it takes the things we did in the other constructor and adds to it
        // (int) type is getting the index of the type in the enum StatModType , which decides for us the order

        // First : requires to add value and StatModType , calls the main contructor above and sets order and source as default (int)type and null respectively 
        public StatModifier (float value, StatModType type) : this(value, type, (int)type, null) { }

        //Second : requires to add value, StatModType, and Order . Sets the source as default, which is null
        public StatModifier (float value, StatModType type, int order) : this (value, type, order, null) { }

        //Third : requires to add value, StatModType, and Source . Sets the type as defaults, which is (int)type, which is the Id of the type from the enum
        public StatModifier (float value, StatModType type, object source) : this (value, type, (int)type, source) { }
    }
}