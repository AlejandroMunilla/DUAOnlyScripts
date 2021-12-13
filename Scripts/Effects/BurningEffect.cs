using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : MonoBehaviour
{

    public int maxTurns = 7;
    public int damage = 2;

    private int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        InvokeRepeating("Burning", 1, 1);
    }

    private void Burning ()
    {
     //   Debug.Log(counter);
        counter++;
        PlayerStats ps = transform.root.gameObject.GetComponent<PlayerStats>();
        ps.AddjustHealth(-damage, gameObject, true);
        ps.AddjustRegen(-damage, gameObject, false);

        if (counter >= maxTurns)
        {
            CancelInvoke("Burning");
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("Burning");
    }


}
