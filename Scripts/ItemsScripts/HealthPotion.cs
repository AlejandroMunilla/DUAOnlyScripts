using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private int min = 1;
    private int max = 10;
    private int add = 10;
    
    // Start is called before the first frame update
    void OnEnable ()
    {
        int totalAdd = Random.Range(min, max) + add;
        GameObject target = transform.root.gameObject;
        target.GetComponent<PlayerStats>().AddjustHealth(totalAdd, target, false);
        target.transform.Find("Effects/Healing").gameObject.SetActive(true);

        gameObject.SetActive(false);
    }


}
