using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.AI;

public class BossPolidori : MonoBehaviour
{
    public GameObject polidori;
    public GameObject lever;
    public bool chandelierAttack = false;

    private float timer;
    private int regeneration;
    private bool alive = true;
    private float lerpFloat = 0;
    private Transform destiny;
    private GameController gc;
    private Camera cam;
    private Animator anim;
    private NavMeshAgent nav;
    private AudioController audioC;
    private PlayerStats ps;
    private PlayerAttack pa;
    private EnemyAI ea;
    private MySteamManager steamManager;

    


    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04,
        Seq05,
        Seq06
    }
    private State state;

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(0);
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(0);
                    Seq02();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq03:
                    Seq03();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq04:
                    Seq04();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq05:
                    Seq05();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq06:
                    Seq06();
                    yield return new WaitForSeconds(0);
                    break;
            }

                
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        cam = Camera.main;
        steamManager = GameObject.FindGameObjectWithTag("SteamManager").GetComponent<MySteamManager>();
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;
            StartCourotine();
            
        }
    }

    private void StartCourotine ()
    {
        state = State.Seq01;
        StartCoroutine("FSM");

        gc.StartCinematic();
        
        Transform polidoriTransform = transform.Find("Polidori");
        polidori = Instantiate(Resources.Load("Enemy/Polidori"), polidoriTransform.position, polidoriTransform.rotation) as GameObject;
        polidori.name = "Polidori";
        anim = polidori.GetComponent<Animator>();
        nav = polidori.GetComponent<NavMeshAgent>();
        audioC = gc.GetComponent<AudioController>();
        audioC.ChangeToBattle("Outlaw Battle");
        ps = polidori.GetComponent<PlayerStats>();
        regeneration = ps.currentRegen;
        pa = polidori.GetComponent<PlayerAttack>();
        ea = polidori.GetComponent<EnemyAI>();
        polidori.GetComponent<AIPolidori>().enabled = false;
        polidori.GetComponent<BossHUD>().enabled = false;
        ea.enabled = false;
        pa.rangeEffect = "Mind";
        pa.InstantiateBullets();
   
        polidori.SetActive(true);
        cam.transform.position = transform.Find("CamPos01").position;
        cam.transform.rotation = transform.Find("CamPos01").rotation;
        destiny = transform.Find("CamPos02");
        DialogueManager.StartConversation("Polidori");


    }

    private void Seq01()
    {
    //    Debug.Log("Seq01");

        lerpFloat += Time.deltaTime * 0.01f;

        cam.transform.position = Vector3.Lerp(cam.transform.position, destiny.position, lerpFloat);
     //   Debug.Log(lerpFloat);
        if (lerpFloat >= 0.1f || gc.inConversation == false)
        {
            state = State.Seq02;

        }
 

    }

    private void Seq02()
    {
        Debug.Log("Seq02");
        Vector3 polidoriFront = new Vector3(polidori.transform.position.x - 2, polidori.transform.position.y + 1, polidori.transform.position.z);
        cam.transform.position = polidoriFront;
        anim.SetFloat("Forward", 0.4f);
        nav.speed = 1;
        nav.destination = transform.Find("CamPos01").position;

        float distance = Vector3.Distance(polidori.transform.position, transform.Find("Polidori").position);
    //    Debug.Log(distance);

        if (distance > 3)
        {
            state = State.Seq03;
            anim.SetFloat("Forward", 0);
            anim.SetTrigger("Rage");
            nav.isStopped = true;
            
            Invoke("DelayAudio", 1.8f);
        }


    }

    private void Seq03()
    {

        Vector3 polidoriFront = new Vector3(polidori.transform.position.x - 2, polidori.transform.position.y + 1, polidori.transform.position.z);
        cam.transform.position = polidoriFront;
        Debug.Log("Seq03");
        anim.SetFloat("Forward", 0);
        nav.isStopped = true;
        timer = Time.timeSinceLevelLoad + 4;
        state = State.Seq04;
        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            GameObject go = gc.players[cnt];
            go.transform.position = transform.Find(cnt.ToString()).position;
            go.transform.rotation = transform.Find(cnt.ToString()).rotation;
        }

    }

    private void Seq04 ()
    {
        Vector3 polidoriFront = new Vector3(polidori.transform.position.x - 2, polidori.transform.position.y + 1, polidori.transform.position.z);
        cam.transform.position = polidoriFront;
        if (timer < Time.timeSinceLevelLoad )
        {
            
            ChangePolidori();
            state = State.Seq05;
        }
    }

    private void Seq05 ()
    {
        if (ps.health == 0 || polidori.tag == "EnemyDead")
        {
            DialogueManager.ShowAlert("You have won! Wait for more content to continue the adventures of this party and venture forth");
            DialogueLua.SetActorField(DialogueLua.GetVariable("profile").asString, "level1", "done");
            Invoke("SaveAndExit", 4);
        }
        else
        {
            if (chandelierAttack == true)
            {
                chandelierAttack = false;

                if (ea.state != EnemyAI.State.Stun)
                {
                    ChangePosition();
                }
                
            }
            else
            {
                bool closePlayer = false;
                float minDistance = Mathf.Infinity;
                GameObject newTarget = null;
                foreach (GameObject go in gc.players)
                {
                    float playerDistance = Vector3.Distance(polidori.transform.position, go.transform.position);

                    if (playerDistance < 5)
                    {
                        closePlayer = true;
                        if (playerDistance < minDistance)
                        {
                            newTarget = go;
                            minDistance = playerDistance;
                        }
                    }

                }

                if (newTarget != null)
                {
                    ea.target = newTarget;                    
                    pa.rangedAttack = false;
                    ps.attacktRange = 2;
                    
                }
            }
        }

        
        
    }

    private void Seq06()
    {

    }

    private void ChangePosition ()
    {
        polidori.transform.position = new Vector3(lever.transform.position.x, lever.transform.position.y, lever.transform.position.z - 1); ;

        bool closePlayer = false;
        float minDistance = Mathf.Infinity;
        GameObject newTarget = null;
        foreach (GameObject go in gc.players)
        {
            float playerDistance = Vector3.Distance(polidori.transform.position, go.transform.position);

            if (playerDistance < 5)
            {
                closePlayer = true;
                if (playerDistance < minDistance)
                {
                    newTarget = go;
                    minDistance = playerDistance;
                }
            }

        }

        if (newTarget != null)
        {
            ea.target = newTarget;
        }
        else
        {
            pa.rangeEffect = "Mind";
            pa.rangedAttack = true;
            ps.attacktRange = 30;
            ea.state = EnemyAI.State.Search;
        }
    }

    private void ChangePolidori ()
    {
        Debug.Log("End");
        gc.EndCinematic();        
        
        polidori.tag = "Enemy";
        polidori.GetComponent<EnemyAI>().enabled = true;
        polidori.GetComponent<Regenerate>().enabled = true;
        polidori.GetComponent<PlayerStats>().enabled = true;
        polidori.GetComponent<PlayerAttack>().enabled = true;
        polidori.GetComponent<BossHUD>().enabled = true;
        polidori.GetComponent<AIPolidori>().enabled = false;
        polidori.GetComponent<Animator>().SetFloat("Forward", 1);
        nav.isStopped = false;
        cam.transform.position = new Vector3 (401.42f, 15.07f, 11.63f);
        audioC.ChangeToBattle("Enemies LOOP");

        if (gc.players.Count == 1)
        {
            polidori.GetComponent<PlayerStats>().currentRegen = 0;
        }

        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            gc.players[cnt].transform.position = transform.Find(cnt.ToString()).transform.position;
        }
    }

    private void SaveAndExit ()
    {
        string profile = DialogueLua.GetVariable("profile").asString;
        GetComponent<SaveGame>().SaveData(profile, "Autosave");

        ///FOR BETA ONLY
        steamManager.AddStatsByName("Beta_Tester");
        steamManager.SetAchievement("Beta_Tester");
    }

    private void DelayAudio ()
    {
        polidori.GetComponent<AudioSource>().Play();
    }

    private void TestLan ()
    {
        DialogueManager.SetLanguage("fr");
    }
}