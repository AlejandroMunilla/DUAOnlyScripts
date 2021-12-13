using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    private int min = 5;
    private int max = 15;
    private int add = 5;

    // Start is called before the first frame update
    void OnEnable()
    {
        int totalAdd = Random.Range(min, max) + add;
        GameObject target = transform.root.gameObject;
        target.GetComponent<PlayerStats>().AddjustMana(totalAdd, target);

        

        if (target.transform.Find("Effects/Maning") != null)
        {
            target.transform.Find("Effects/Maning").gameObject.SetActive(true);
        }
        else
        {
            Vector3 tempPos = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
            GameObject itemTemp = Instantiate(Resources.Load("Effects/Maning"), tempPos, target.transform.rotation) as GameObject;
            itemTemp.name = "Maning";
            Transform traTemp = target.transform.Find("Effects");
            itemTemp.transform.parent = traTemp;
            itemTemp.SetActive(true);

        }
        gameObject.SetActive(false);

    }
}
