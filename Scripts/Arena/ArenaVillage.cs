using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ArenaVillage : MonoBehaviour
{
    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public GameObject spawnPoint4;
    public GameObject spawnPoint5;
    public GameObject spawnPoint6;

   
    private bool alive = true;
    private bool listCompleted = false;
    private bool enemiesLoaded = false;
    private bool missionSuccess = false;
    private string enemies;
    private Transform spawnPoints;
    private GameController gc;
    private MySteamManager steamManager;
    private List<string> reservoir;
    private List<string> currentWave = new List<string>();
    private List<Transform> spawnPointsList = new List<Transform>();
    private float timer = 0;
    private int waveNo = 1;

    private enum State 
    {
        Seq01,
        Seq01a,
        Seq01aa,
        Seq01b,
        Seq01c,
        Seq02,
        Seq02a,
        Seq02b,
        Seq02c,
        Seq02d,
        Seq02e,
        Seq03,
        Seq03a,
        Seq03b,
        Seq03c,
        Seq03d,
        Seq04,
        Seq04a,
        Seq04b,
        Seq04c,
        Seq04d,
        Seq05,
        Seq05a,
        Seq05b,
        Seq05c,
        Seq05d,
        Seq06,
        Seq07
    }
    private State state;

    private string [] wave1 = { "Zombie", "Zombie", "Skeleton", "Skeleton"};
    private string [] reservoir1 = { "Zombie", "Zombie", "Zombie", "Skeleton"};


    private string [] wave2 = { "Zombie", "Zombie", "Skeleton", "SkeletonArcher", "Ghoul"};
    private string [] reservoir2 = { "Zombie", "Zombie", "Skeleton"};


    private string [] wave3 = { "Zombie", "Zombie", "SkeletonArcher", "Skeleton", "Ghoul"};
    private string[] reservoir3 = { "Zombie", "SkeletonArcher", "Vampire" };

    private string [] wave4 = { "Zombie", "SkeletonArcher", "Skeleton", "Ghoul", "Vampire"};
    private string[] reservoir4 = { "Zombie", "Banshee" };

    private string [] wave5 = { "Zombie", "Skeleton", "SkeletonArcher", "Ghoul", "Banshee"};
    private string[] reservoir5 = { "Zombie", "Skeleton", "Polidori" };

    //UNDEAD
    private string[] undead1 = { "Zombie", "Zombie", "Skeleton", "Skeleton" };
    private string[] undeadRes1 = { "Zombie", "Zombie", "Zombie", "Skeleton" };


    private string[] undead2 = { "Zombie", "Zombie", "Skeleton", "SkeletonArcher", "Ghoul" };
    private string[] undeadRes2 = { "Zombie", "Zombie", "Skeleton" };


    private string[] undead3 = { "Zombie", "Zombie", "SkeletonArcher", "Skeleton", "Ghoul" };
    private string[] undead3Res = { "Zombie", "SkeletonArcher", "Vampire" };

    private string[] undead4 = { "Zombie", "SkeletonArcher", "Skeleton", "Ghoul", "Vampire" };
    private string[] undead4Res = { "Zombie", "Banshee" };

    private string[] undead5 = { "Zombie", "Skeleton", "SkeletonArcher", "Ghoul", "Banshee" };
    private string[] undead5Res = { "Zombie", "Skeleton", "Polidori" };



    // Start is called before the first frame update
    void OnEnable ()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        steamManager = GameObject.FindGameObjectWithTag("SteamManager").GetComponent<MySteamManager>();
        spawnPoints = transform.Find("SpawnPoints");
        spawnPoint1 = transform.Find("SpawnPoints/SpawnPoint1").gameObject;
        spawnPoint2 = transform.Find("SpawnPoints/SpawnPoint2").gameObject;
        spawnPoint3 = transform.Find("SpawnPoints/SpawnPoint3").gameObject;
        spawnPoint4 = transform.Find("SpawnPoints/SpawnPoint4").gameObject;    


        reservoir = new List<string>();
        reservoir.Clear();

        if (gc.multiplayer == false || gc.server == true)
        {
            foreach (string st in wave1)
            {
                currentWave.Add(st);
            }

            Invoke("PreLoad", 2);
        }

    }

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(2);
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;  

                case State.Seq01a:
                    yield return new WaitForSeconds(2);
                    Seq01a();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq01aa:
                    yield return new WaitForSeconds(3);
                    Seq01a();
                    yield return new WaitForSeconds(2);
                    break;

                case State.Seq01b:
                    yield return new WaitForSeconds(1);
                    Seq01b();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq01c:
                    yield return new WaitForSeconds(1);
                    Seq01c();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(1);
                    Seq02();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02a:
                    yield return new WaitForSeconds(1);
                    Seq02a();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02b:
                    yield return new WaitForSeconds(1);
                    Seq02b();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02c:
                    yield return new WaitForSeconds(1);
                    Seq02c();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02d:
                    yield return new WaitForSeconds(1);
                    Seq02d();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02e:
                    yield return new WaitForSeconds(1);
                    Seq02e();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq03:
                    yield return new WaitForSeconds(1);
                    Seq03();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq03a:
                    yield return new WaitForSeconds(1);
                    Seq03a();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq03b:
                    yield return new WaitForSeconds(1);
                    Seq03b();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq03c:
                    yield return new WaitForSeconds(1);
                    Seq03c();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq03d:
                    yield return new WaitForSeconds(1);
                    Seq03d();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq04:
                    yield return new WaitForSeconds(1);
                    Seq04();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq04a:
                    yield return new WaitForSeconds(1);
                    Seq04a();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq04b:
                    yield return new WaitForSeconds(1);
                    Seq04b();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq04c:
                    yield return new WaitForSeconds(1);
                    Seq04c();
                    yield return new WaitForSeconds(1);
                    break;


                case State.Seq05:
                    yield return new WaitForSeconds(1);
                    Seq05();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq05a:
                    yield return new WaitForSeconds(1);
                    Seq05a();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq05b:
                    yield return new WaitForSeconds(1);
                    Seq05b();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq05c:
                    yield return new WaitForSeconds(1);
                    Seq05c();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq06:
                    yield return new WaitForSeconds(3);
                    Seq06();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq07:
                    yield return new WaitForSeconds(1);
                    Seq07();
                    yield return new WaitForSeconds(1);
                    break;
            }
        }
    }


    private void PreLoad ()
    {
        DialogueLua.SetVariable("wave", "1");
        Invoke("InvokeConversation", 0.1f);
        enemies = DialogueLua.GetVariable("enemies").asString;

    }

    private void InvokeConversation()
    {
        DialogueManager.StartConversation("ArenaCross");  
        spawnPointsList.Clear();
        state = State.Seq01;
        StartCoroutine("FSM");
        PopulatePoints();
        foreach (GameObject go in gc.players)
        {
            foreach (string st in reservoir1)
            {
                reservoir.Add(st);
            }
        }
    }



    private void Seq01 ()
    {
    //    Debug.Log("1");
        InstantiateEnemies();
        state = State.Seq01a;
        enemiesLoaded = false;
    }

 
    private void Seq01a()
    {
        Debug.Log(reservoir.Count);
        if (reservoir.Count > 0)
        {
            ReservoirWatcher();
        }
        else
        {
            state = State.Seq01b;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }

    }

    private void Seq01b()
    {
        Debug.Log("Seq01b");
        if (gc.enemies.Count == 0)
        {
            timer = Time.timeSinceLevelLoad + 3;
            DialogueLua.SetVariable("wave", "1b");
            ResetWave();
            state = State.Seq01c;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }


    }

    private void Seq01c()
    {
        Debug.Log("Seq01c");
        if (Time.timeSinceLevelLoad > timer)
        {
            timer = Time.timeSinceLevelLoad + 2;
 
            DialogueManager.StartConversation("ArenaCross");            
            state = State.Seq02;
        }
    }



    private void Seq02()
    {
        Debug.Log("Seq02");
        if (gc.inConversation == false)
        {
            DialogueLua.SetVariable("wave", "2");            
            state = State.Seq02a;
        }
    }

    private void Seq02a()
    {
        Debug.Log("Seq02a");
        DialogueManager.StartConversation("ArenaCross");
        state = State.Seq02b;
    }

    private void Seq02b()
    {
        Debug.Log("Seq02b");
        if (gc.inConversation == false)
        {
            DialogueLua.SetVariable("wave", "2b");
            spawnPointsList.Clear();
            PopulatePoints();

            currentWave.Clear();

            foreach (string st in wave2)
            {
                currentWave.Add(st);
            }
            InstantiateEnemies();

            reservoir.Clear();
            foreach (GameObject go in gc.players)
            {
                foreach (string st in reservoir2)
                {
                    reservoir.Add(st);
                }
            }
            state = State.Seq02c;
        }
    }



    private void Seq02c()
    {
        Debug.Log("Seq02c");

        Debug.Log(reservoir.Count);
        if (reservoir.Count > 0)
        {
            ReservoirWatcher();
        }
        else
        {
            state = State.Seq02d;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }

    private void Seq02d ()
    {
        Debug.Log("Seq02d");

        if (gc.enemies.Count == 0)
        {
            timer = Time.timeSinceLevelLoad + 2;
            DialogueLua.SetVariable("wave", "2b");
            ResetWave();
            state = State.Seq02e;
            DialogueManager.ShowAlert("Debug: Wave 2d");
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }

    }

    private void Seq02e()
    {
        Debug.Log("Seq02e");
        if (Time.timeSinceLevelLoad > timer)
        {
            timer = Time.timeSinceLevelLoad + 2;

            DialogueManager.StartConversation("ArenaCross");
            state = State.Seq03;
        }
    }

    
    private void Seq03()
    {
        Debug.Log("Seq03");
        if (gc.inConversation == false)
        {
            DialogueLua.SetVariable("wave", "3");
            state = State.Seq03a;
        }
    }

    private void Seq03a()
    {
        Debug.Log("Seq03a");
        if (gc.inConversation == false)
        {
            DialogueManager.StartConversation("ArenaCross");
            DialogueLua.SetVariable("wave", "3b");
            spawnPointsList.Clear();
            PopulatePoints();

            currentWave.Clear();

            foreach (string st in wave3)
            {
                currentWave.Add(st);
            }
            InstantiateEnemies();

            reservoir.Clear();
            foreach (GameObject go in gc.players)
            {
                foreach (string st in reservoir3)
                {
                    reservoir.Add(st);
                }
            }
            state = State.Seq03b;
        }
    }

    private void Seq03b()
    {
        Debug.Log("Seq03b");

        Debug.Log(reservoir.Count);
        if (reservoir.Count > 0)
        {
            ReservoirWatcher();
        }
        else
        {
            state = State.Seq03c;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }


    private void Seq03c ()
    {
        Debug.Log("Seq03c");
        if (gc.enemies.Count == 0)
        {
            timer = Time.timeSinceLevelLoad + 3;
            DialogueLua.SetVariable("wave", "3b");
            ResetWave();
            state = State.Seq04;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }

    private void Seq03d ()
    {
        Debug.Log("Seq03d");
        if (Time.timeSinceLevelLoad > timer)
        {
            timer = Time.timeSinceLevelLoad + 2;
            DialogueManager.StartConversation("ArenaCross");
            DialogueLua.SetVariable("wave", "4");
            state = State.Seq04;
        }
    }

    private void Seq04()
    {
        Debug.Log("Seq03");
        if (gc.inConversation == false)
        {
            DialogueManager.StartConversation("ArenaCross");
            state = State.Seq04a;
        }
    }

    private void Seq04a()
    {
        Debug.Log("Seq04a");
        if (gc.inConversation == false)
        {
            DialogueLua.SetVariable("wave", "4b");
            spawnPointsList.Clear();
            PopulatePoints();

            currentWave.Clear();

            foreach (string st in wave4)
            {
                currentWave.Add(st);
            }
            InstantiateEnemies();

            reservoir.Clear();
            foreach (GameObject go in gc.players)
            {
                foreach (string st in reservoir4)
                {
                    reservoir.Add(st);
                }
            }
            state = State.Seq04b;
        }
    }
    private void Seq04b ()
    {
        Debug.Log("Seq04b");
        Debug.Log(reservoir.Count);
        if (reservoir.Count > 0)
        {
            ReservoirWatcher();
        }
        else
        {
            DialogueLua.SetVariable("wave", "4b");
            state = State.Seq04c;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }
    private void Seq04c()
    {
        Debug.Log("Seq04c");
        if (gc.enemies.Count == 0)
        {
            timer = Time.timeSinceLevelLoad + 3;            
            ResetWave();
            DialogueManager.StartConversation("ArenaCross");
            DialogueLua.SetVariable("wave", "5");
            state = State.Seq05;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }
    private void Seq05()
    {
        Debug.Log("Seq05");
        if (gc.inConversation == false)
        {
            DialogueManager.StartConversation("ArenaCross");
            DialogueLua.SetVariable("wave", "5b");

            state = State.Seq05a;
        }
    }
    private void Seq05a()
    {
        Debug.Log("Seq05a");
        if (gc.inConversation == false)
        {
            spawnPointsList.Clear();
            PopulatePoints();

            currentWave.Clear();

            foreach (string st in wave5)
            {
                currentWave.Add(st);
            }
            InstantiateEnemies();

            reservoir.Clear();
            foreach (GameObject go in gc.players)
            {
                foreach (string st in reservoir5)
                {
                    reservoir.Add(st);
                }
            }
            state = State.Seq05b;
        }
    }
    private void Seq05b()
    {
        Debug.Log("Seq05b");
        Debug.Log(reservoir.Count);
        if (reservoir.Count > 0)
        {
            ReservoirWatcher();
        }
        else
        {
            state = State.Seq05c;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }
    private void Seq05c()
    {
        Debug.Log("Seq05c");
        if (gc.enemies.Count == 0)
        {
            timer = Time.timeSinceLevelLoad + 3;
            ResetWave();
            DialogueManager.StartConversation("ArenaCross");
            DialogueLua.SetVariable("wave", "End");
            state = State.Seq06;
        }

        if (AllPlayersDead() == true)
        {
            Debug.Log("Game Over");
            SaveAndExit();
        }
    }
    private void Seq06()
    {
        SaveAndExit();
        steamManager.AddStatsByName("Beta_Tester");
        steamManager.SetAchievement("Beta_Tester");
    }

    private void Seq07 ()
    {

    }

    private void ResetWave()
    {
        foreach (GameObject go in gc.players)
        {
            go.GetComponent<PlayerStats>().AddjustHealth(1000, go, false);
        }

        gc.Reinitialize();
    }

    private bool AllPlayersDead ()
    {
        bool allPlayersDead = true;

        foreach (GameObject go in gc.players)
        {
           
            if (go.GetComponent<PlayerStats>().health > 0)
            {
                allPlayersDead = false;
            }
        }

        if (allPlayersDead == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SaveAndExit ()
    {
        GetComponent<SaveGame>().SaveProfile("AutoSave", "01MainMenu", true);
    }


    private void PopulatePoints()
    {
        int childCount = spawnPoints.childCount;
        int backUpLoop = 0;
        bool tempReady = true;
        foreach (Transform ta in spawnPoints)
        {
            if (spawnPointsList.Contains(ta) == false)
            {
       //         Debug.Log("false");
                tempReady = false;
            }
        }
        int randomChild = Random.Range(0, childCount);
        Transform tempTa = spawnPoints.GetChild(randomChild);
        if (spawnPointsList.Contains(tempTa) == false)
        {
            spawnPointsList.Add(spawnPoints.GetChild(randomChild));
        }

        if (tempReady == true || backUpLoop > 200)
        {
            listCompleted = true;

        }
        else
        {
            Invoke("PopulatePoints", 0.01f);
        }
    }

    private void InstantiateEnemies ()
    {
        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            int tempInt = Random.Range(0, gc.players.Count);

            Transform taGO = spawnPoints.GetChild(tempInt);
     //       Debug.Log(currentWave.Count);
            for (int cnt2 = 0; cnt2 < currentWave.Count ; cnt2++)
            {
                Transform pointGO = taGO.GetChild(cnt2);
      //          Debug.Log(currentWave[cnt2]);
                GameObject enemyGO = Instantiate(Resources.Load("Enemy/" + currentWave[cnt2]), pointGO.position, pointGO.rotation) as GameObject;
                enemyGO.name = currentWave[cnt2];
                if (enemyGO.activeSelf == false)
                {
                    enemyGO.SetActive(true);
                }

            }
        }
        enemiesLoaded = true;
        listCompleted = true;
    }

    private void ReservoirWatcher ()
    {

        int tempTotalEnemiesAtOnce = gc.players.Count * wave1.Length;
        int enemiesAlive = gc.enemies.Count;
   //     Debug.Log(reservoir.Count + "/" + enemiesAlive + "/" + tempTotalEnemiesAtOnce);

        if (enemiesAlive < tempTotalEnemiesAtOnce)
        {
            foreach (string st in reservoir)
            {
                Debug.Log(st);
            }

            int childCount = transform.Find("SpawnPoints").childCount;
            Debug.Log(childCount);
            int randomChild = Random.Range(0, childCount);
   
            Transform spawnPointTemp = transform.Find("SpawnPoints").GetChild(randomChild);
            Transform taGO = spawnPointTemp.transform.Find("0");
            //   Debug.Log(cnt.ToString() + "/" + wave1[cnt]);
            GameObject enemyGO = Instantiate(Resources.Load("Enemy/" + reservoir[0]), taGO.position, taGO.rotation) as GameObject;
            enemyGO.name = reservoir[0];
     //       Debug.Log(enemyGO.name);
            reservoir.Remove(reservoir[0]);
            if (enemyGO.activeSelf == false)
            {
                DialogueManager.ShowAlert("Debug: Addded" + enemyGO.name + enemyGO + "/ Enemies: " + enemiesAlive);
                enemyGO.SetActive(true);
            }
        }
    }

    private void EnemyLists ()
    {
        if (enemies == "Undead")
        {
            wave1 = undead1;  reservoir1 = undeadRes1;
            wave2 = undead2;  reservoir2 = undeadRes2;
            wave3 = undead3;  reservoir3 = undead3Res;
            wave4 = undead4;  reservoir4 = undead4Res;
            wave5 = undead5;  reservoir5 = undead5Res;
        }
    }


}
