using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class ResistanceDic : IComparable<ResistanceDic>
{
    public string name;
    public string type;
    public int damage;
    public int resistance;

    public ResistanceDic(string newName, string newType, int newResistance, int newDamage)
    {
        name = newName;
        type = newType;
        resistance = newResistance;
        damage = newDamage;


    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(ResistanceDic other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return resistance - other.resistance;
    }
}
