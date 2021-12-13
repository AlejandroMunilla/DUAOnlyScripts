using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnDestroy : MonoBehaviour
{
    public bool byPassArmour = false;
    public int minDam = 0;
    public int maxDam = 6;
    public int addDam = 5;
    public float range = 7;
    public GameObject effect;
    public GameObject caster;
    private GameController gc = null;

    private void OnEnable ()
    {
        if (gc == null)
        {
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            if (caster == null)
            {
                caster = gameObject;
                
            }
            if (effect == null)
            {
                effect = Instantiate(Resources.Load("Help/MindBlast"), transform.position, transform.rotation) as GameObject;
            }
        }
    }

    private void OnDisable()
    {
        effect.transform.position = transform.position;
        effect.SetActive(true);
        if (caster.tag == "Player" || caster.tag == "Ally")
        {
            foreach (GameObject go in gc.enemies)
            {
                float distanceToGO = Vector3.Distance(transform.position, go.transform.position);
                if (distanceToGO <= range)
                {
                    int randomDam = Random.Range(minDam, maxDam) + addDam;
                    if (byPassArmour == false)
                    {
                        randomDam = randomDam - go.GetComponent<PlayerStats>().armor;
                    }

                    go.GetComponent<PlayerStats>().AddjustHealth(-randomDam, caster, true);
                }
            }
        }
    }
}
