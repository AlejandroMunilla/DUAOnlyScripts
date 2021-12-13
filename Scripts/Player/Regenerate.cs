using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regenerate : MonoBehaviour
{
    public int regenHealth = 1;
    public int perSecond = 5;
    private PlayerStats ps;
    // Start is called before the first frame update
    
    void Start()
    {
        ps = GetComponent<PlayerStats>();
        InvokeRepeating("RegenerateHealth", 1, perSecond);
    }

    private void RegenerateHealth ()
    {
     //   Debug.Log(ps.currentRegen + "/" + ps.health);
        if (gameObject.tag == "Enemy")
        {
            if (ps.currentRegen > ps.health)
            {
                ps.AddjustHealth(regenHealth, gameObject, false);


            }
        }

    }
}
