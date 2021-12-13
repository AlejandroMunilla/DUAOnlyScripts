using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

//This is the class you will be storing
//in the different collections. In order to use
//a collection's Sort() method, this class needs to
//implement the IComparable interface.
public class InventoryRPG : IComparable<InventoryRPG>
{
    public string name;
    public int qt;
    public int slot;

    public InventoryRPG (string newName, int newPower, int newSlot)
    {
        name = newName;
        qt = newPower;
        slot = newSlot;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(InventoryRPG other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in qt.
        return qt - other.qt;
    }
}