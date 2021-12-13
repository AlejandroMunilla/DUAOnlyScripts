using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExplosive : MonoBehaviour
{

    public string target1 = "Enemy";
    public string target2 = "AllyEnemy";
    public int minDam = 2;
    public int maxDam = 12;
    public int adDam = 1;
    public int minFire = 1;
    public int maxFire = 6;
    public float areaTrap = 3;
    public float areaEffect = 6;
    private bool loaded = false;
    private int internalCounter = 0;
    public bool fire = false;
    private GameObject explosion;
    private GameController gc;
    private List<GameObject> targets = new List<GameObject>();
    
    // Start is called before the first frame update
    void OnEnable ()
    {
        if (loaded == false)
        {
            loaded = true;
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            explosion = transform.Find("Explosion").gameObject;
            explosion.transform.parent = null;
            targets.Clear();
            GetComponent<SphereCollider>().radius = areaTrap;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == target1 || other.tag == target2)
        {
            List<GameObject> target1List = new List<GameObject>();
            List<GameObject> target2List = new List<GameObject>();

            if (target1 == "Enemy")
            {
                target1List = gc.enemies;
                target2List = gc.enemyAllies;
            }
            else if (target1 == "Player")
            {
                target1List = gc.players;
                target2List = gc.allies;
            }

            foreach (GameObject go in target1List)
            {
                CheckDistance(go);
            }
            foreach (GameObject go in target2List)
            {
                CheckDistance(go);
            }

            gameObject.SetActive(false);
            explosion.transform.position = transform.position;
            explosion.SetActive(true);
        }
    }

    private void CheckDistance (GameObject go)
    {
        float distanceToTarget = Vector3.Distance(transform.position, go.transform.position);
        if (distanceToTarget <= areaEffect)
        {
            int totalDamage = Random.Range(minDam, maxDam) + adDam;
            PlayerStats ps = go.GetComponent<PlayerStats>();
            totalDamage = totalDamage - ps.armor;

            if (totalDamage < 0)
            {
                totalDamage = 0;
            }

            if (fire == true)
            {
                int fireDamage = Random.Range(minFire, maxFire);
                fireDamage = fireDamage - ps.fireRes;
                if (fireDamage > 0)
                {
                    totalDamage = totalDamage + fireDamage;

                    if (ps.currentRegen > 0 )
                    {
                        ps.currentRegen = ps.currentRegen - fireDamage;
                    }

                    targets.Add(go);
                    if (targets.Count > 0)
                    {
                        Invoke("CheckFire", 1);
                    }
                }
            }
            ps.AddjustHealth(-totalDamage, gameObject, true);

 

        }
    }

    private void CheckFire ()
    {
        internalCounter++;
        foreach (GameObject go in targets)
        {
            PlayerStats ps = go.GetComponent<PlayerStats>();

            if (ps.health > 0)
            {
                int fireDamage = Random.Range(minFire, maxFire);
                fireDamage = fireDamage - ps.fireRes;
                if (fireDamage > 0)
                {
                    bool hit = true;
                    if (ps.race == "Undead")
                    {
                        hit = false;
                    }
                    ps.AddjustHealth(-fireDamage, gameObject, hit);
                    if (ps.currentRegen > 0)
                    {
                        ps.currentRegen = ps.currentRegen - fireDamage;
                    }
                }
            }

        }

        if (internalCounter < 3)
        {
            Invoke("CheckFire", 1);
        }
    }
}
