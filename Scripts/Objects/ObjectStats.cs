using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{

    public int totalHealth = 50;
    public int currentHealth = 50;
    public int greenBar;
    // Start is called before the first frame update

    private void Start()
    {
        float barMaxHeight = Screen.width * 0.3f;
        float tempRateHealth = (float)(currentHealth) / (float)(totalHealth);
        greenBar = (int)(tempRateHealth * barMaxHeight);
    }

    private void InstantiateExplosion ()
    {

    }

    public void AddjustHealth (int damage)
    {
        currentHealth = currentHealth + damage;

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            if (gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>() != null)
            {
                gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            }
        }

        float barMaxHeight = Screen.width * 0.3f;
        float tempRateHealth = (float)(currentHealth) / (float)(totalHealth);
        greenBar = (int)(tempRateHealth * barMaxHeight);

    }
 
}
