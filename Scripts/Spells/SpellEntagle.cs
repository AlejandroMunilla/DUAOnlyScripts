using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public class SpellEntagle : MonoBehaviour
{

    public bool entagle = true;
    public bool damage = true;
    public bool magicalDam = false;
    public int level;
    public float timer = 5;
    public float distance = 7.5f;
    public GameObject caster;
    public GameController gc;
   
    
    private List<GameObject> targets = new List<GameObject>();
    private List<GameObject> entagleds = new List<GameObject>();
    private string tag1;
    private string tag2;
    private int minDamage = 1;
    private int maxDamage = 6;
    private int counter = 0;
    
   
    // Start is called before the first frame update

    void OnEnable ()
    {
        if (caster.tag == "Player")
        {
            tag1 = "Enemy";
            tag2 = "AllyEnemy";
        }
        else
        {
            tag1 = "Player";
            tag2 = "Ally";
        }

        InvokeRepeating ("CheckTargets", 0.01f, 2);
    }

    private void OnDisable()
    {
        CancelInvoke("CheckTargets");
    }

    private void CheckTargets ()
    {
        counter++;
        targets.Clear();
        if (caster.tag == "Player")
        {
            if (gc.enemies != null)
            {
                if (gc.enemies.Count >0)
                {
                    foreach (GameObject go in gc.enemies)
                    {
                        float distanceToGo = Vector3.Distance(go.transform.position, transform.position);

                        if (distanceToGo <= distance)
                        {
                            targets.Add(go);
                        }
                       
                    }
                }
            }
            if (gc.enemyAllies != null)
            {
                if (gc.enemyAllies.Count > 0)
                {
                    foreach (GameObject go in gc.enemyAllies)
                    {
                        float distanceToGo = Vector3.Distance(go.transform.position, transform.position);

                        if (distanceToGo <= distance)
                        {
                            targets.Add(go);
                        }

                    }
                }
            }

        }
        else if (caster.tag == "Enemy")
        {
            if (gc.players != null)
            {
                if (gc.players.Count > 0)
                {
                    foreach (GameObject go in gc.players)
                    {
                        float distanceToGo = Vector3.Distance(go.transform.position, transform.position);

                        if (distanceToGo <= distance)
                        {
                            targets.Add(go);
                        }

                    }
                }
            }
            if (gc.allies != null)
            {
                if (gc.allies.Count > 0)
                {
                    foreach (GameObject go in gc.allies)
                    {
                        float distanceToGo = Vector3.Distance(go.transform.position, transform.position);

                        if (distanceToGo <= distance)
                        {
                            targets.Add(go);
                        }

                    }
                }
            }

        }
        CheckEntagled();
    }
        




    private void CheckEntagled ()
    {   

        foreach (GameObject go in targets)
        {
            PlayerStats ps = go.GetComponent<PlayerStats>();
            int resistence = ps.strength * 5;
            int bonus = 0;
            int totalResistance = ps.strength + bonus;
            int diceRoll = Random.Range(0, 100);


            if (diceRoll > 5)
            {
                if (diceRoll > totalResistance)
                {
                    entagleds.Add(go);
                    go.GetComponent<EnemyAI>().ChangeToEntagled(1.9f);
                }


                if (timer == 5)
                {
                    timer = 0;
                    int totalDamage = Random.Range(minDamage, maxDamage) + (int)(level * 0.5f);
                    ps.AddjustHealth(-totalDamage, caster, false);

                    if (magicalDam == true)
                    {
                        int magicRes = ps.magidRes;
                        int totalMagicDamage = (int)(level * 0.5f);
                        ps.AddjustHealth(-totalDamage, caster, true);
                    }
                }
            }
        }
    }
}
