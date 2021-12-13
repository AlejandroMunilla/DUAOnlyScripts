using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public int health = 8;
    public int armour = 0;

    // Start is called before the first frame update


    public void AddjustHealth (int dam, GameObject attacker, bool animate)
    {
        Debug.Log(dam);
    }
}
