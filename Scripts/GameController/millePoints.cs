using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class millePoints : MonoBehaviour {
    private bool alive = true;
    private bool started = false;
    private GameController gc;
    private AudioController audio;
    public AudioClip combatMusic = null;
    public AudioClip normalMusic;
    public List<string> enemies = new List<string>();
    public List<string> delayedEnemies = new List<string>();
    public int basicEnemies = 1;
    public bool addPerPlayer = true;
    public bool spawningDelayed = false;                        //To control max No of enemies active
    public int addEnemyDelayed = 1;                                     //this value + player No.
    private int enemyDelayedCount = 0;                          //Keep track of delayed enemies spawned already
    private Camera cam = null;

    private void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        audio = transform.root.gameObject.GetComponent<AudioController>();
        if (enemies == null || enemies.Count == 0)
        {
            if (gameObject.name == "MillePoint(1s2")
            {
                basicEnemies = 0;
                addPerPlayer = true;
                spawningDelayed = true;
                addEnemyDelayed = 1;
                enemies.Add("SkeletonArcher");
                enemies.Add("SkeletonArcher");
                enemies.Add("Skeleton");
                enemies.Add("Skeleton");
                delayedEnemies.Add("Skeleton");
                delayedEnemies.Add("Skeleton");
                delayedEnemies.Add("Skeleton");
                delayedEnemies.Add("Skeleton");
                delayedEnemies.Add("Skeleton");
            }
            else
            {
                for (int cnt = 0; cnt < 3; cnt++)
                {
                    enemies.Add("Skeleton");
                }
            }

        }
        else
        {
            
        }

    }
    private enum State
    {
        Seq01,
        Seq02,
        Seq03
    }
    private State state;
	// Use this for initialization


    private IEnumerator FSM ()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    
                    yield return new WaitForSeconds(0.5f);
                    Seq01();
                    break;
                case State.Seq02:
                    Seq02();
                    yield return new WaitForSeconds(0.5f);
                    break;
                case State.Seq03:
                    Seq03();
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
        yield return null;
    }
    

    private void OnTriggerEnter(Collider other)
    {
     //   Debug.Log(other.name);
        if (started == false)
        {
            if (other.tag == "Player")
            {
   //            Debug.Log("asas");
                state = State.Seq01;
                if (cam == null)
                {
                    foreach (Camera ca in Camera.allCameras)
                    {
                        if (ca.name == "Camera1")
                        {
                            cam = ca;
                        }
                    }
                }

                cam.GetComponent<CameraController>().move = false;
                StartCoroutine("FSM");
                started = true;
                StartCombat();
                GetComponent<BoxCollider>().enabled = false;
            }
        }

    }

    private void StartCombat ()
    {
        audio.ChangeToBattle("combat1");


    }

    private void Seq01 ()
    {
     //   Debug.Log("!");
        int enemyNo = basicEnemies;
        if (addPerPlayer == true)
        {
            enemyNo = enemyNo + gc.players.Count;
        }

        foreach (string st in enemies)
        {
  //          Debug.Log(st + "/" + enemies.Count + "/"  + enemyNo);
        }

        for (int cnt = 0; cnt < enemyNo; cnt++)
        {
            Transform taGO = null;
            string enemyName = "";
            if (transform.Find(cnt.ToString()) != null)
            {
               taGO  = transform.Find(cnt.ToString());
               if (cnt < enemies.Count)
               {
                    enemyName = enemies[cnt];
               }
               else
                {
                    enemyName = enemies[0];
                }
               
            }
            else
            {
                taGO = transform.GetChild(0);
                enemyName = enemies[0];
            }
            
    //        Debug.Log(enemyName + taGO);
            GameObject enemyGO = Instantiate(Resources.Load("Enemy/" + enemyName), taGO.position, taGO.rotation) as GameObject;
            enemyGO.name = enemyName;
            enemyGO.SetActive(true);
            
            if (enemyName == "SkeletonArcher")
            {
                enemyGO.GetComponent<EnemyAI>().attackAnim = "ArcFire1";
                enemyGO.GetComponent<PlayerAttack>().rangeEffect = "Arrow";
                Debug.Log("Archer?");
            }
        }

        if (spawningDelayed == false)
        {

            state = State.Seq03;
        }
        else
        {
            int totalDelayedEnemies = addEnemyDelayed * gc.players.Count;
            for (int cnt = 0; cnt < totalDelayedEnemies; cnt++)
            {
                if (cnt < enemies.Count)
                {
                    delayedEnemies.Add(enemies[cnt]);
                   
                }
                else
                {
                    delayedEnemies.Add(enemies[0]);
                }
            }

            state = State.Seq02;
        }


        
    }

    private void Seq02()
    {
       
        int enemyNo = basicEnemies;
        if (addPerPlayer == true)
        {
            enemyNo = enemyNo + gc.players.Count;
        }

        Debug.Log(basicEnemies + "/" + delayedEnemies);
        if (delayedEnemies.Count == 0)
        {
            state = State.Seq03;
        }
        else if (gc.enemies.Count < enemyNo)
        {
            Transform taGO = null;
            for (int cnt = 0; cnt < enemyNo; cnt++)
            {
                if (transform.Find(cnt.ToString()) != null)
                {
                    taGO = transform.Find(cnt.ToString());
                }
                else
                {
                    taGO = transform.GetChild(0);
                }
            }
            Debug.Log(delayedEnemies[0]);
            GameObject enemyGO = Instantiate(Resources.Load("Enemy/" + delayedEnemies[0]), taGO.position, taGO.rotation) as GameObject;
       
            enemyGO.name = delayedEnemies[0];
            enemyGO.SetActive(true);
            delayedEnemies.RemoveAt(0);


        }
    }


    private void Seq03()
    {

   //     Debug.Log(gc.enemies.Count);
        if (gc.enemies.Count == 0)
        {
            StopCoroutine("FSM");
            gameObject.SetActive(false);
     //       gc.GetComponent<GameController>().ChangeToPeace();
            audio.ChangeToPeace("audio1");
            cam.GetComponent<CameraController>().move = true;
        }
    }


}
