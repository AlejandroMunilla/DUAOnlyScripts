using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.AI;

public class SmugglersCross : MonoBehaviour
{
    private bool alive = true;
    private AudioClip clip;
    private GameController gc;
    private GameObject rose;
    private MouseOrbitImproved mouseOrbit;
    private Camera cam;
    private List<GameObject> enemies = new List<GameObject>();
    private enum State
    {
        Prefight,
        Fight,
        Leave,
        Seq01,
        Seq02,

    }
    private State state;

    private IEnumerator FSM ()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    
                    Seq01();
                    yield return new WaitForSeconds(1);
                    break;

                case State.Seq02:
                    yield return new WaitForSeconds(0.01f);
                    Seq02();
                    break;

                case State.Leave:
                    yield return new WaitForSeconds(0.1f);
                    Leave();
                    break;

                case State.Fight:
                    yield return new WaitForSeconds(1);
                    Fight();
                    break;

                case State.Prefight:
                    yield return new WaitForSeconds(1);
                    PreFight();
                    break;

            }
        }
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        clip = (AudioClip)(Resources.Load("Audio/sfx_shield"));
        GetComponent<AudioSource>().clip = clip;
        foreach (Camera ca in Camera.allCameras)
        {
            if (ca.name == "Camera1")
            {
                cam = ca;
                mouseOrbit = ca.gameObject.GetComponent<MouseOrbitImproved>();
            }
        }

        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            gc.players[cnt].GetComponent<NavMeshAgent>().enabled = false;
            gc.players[cnt].transform.position = transform.Find("Positions").GetChild(cnt).transform.position;
            gc.players[cnt].transform.rotation = transform.Find("Positions").GetChild(cnt).transform.rotation;
            Debug.Log(transform.Find("Positions").GetChild(cnt).name);
            gc.players[cnt].GetComponent<NavMeshAgent>().enabled = true;
            Transform playerCam = gc.player1.transform.Find("Camera");
            cam.transform.position = playerCam.transform.position;
            cam.transform.rotation = playerCam.transform.rotation;
        }
        state = State.Seq01;
        StartCoroutine("FSM");
    }

    

    private void Seq01 ()
    {
        string profile = DialogueLua.GetVariable("profile").asString;
        Debug.Log(profile);

        if (profile != "" && profile != "nill")
        {
            DialogueLua.SetActorField("Smuggler", "way", "None");
            DialogueManager.StartConversation("SmugglerCross");
            Transform playerCam = gc.player1.transform.Find("Camera");
            cam.transform.position = playerCam.transform.position;
            cam.transform.rotation = playerCam.transform.rotation;
            state = State.Seq02;
        }
    }

    private void Seq02 ()
    {
        string way = DialogueLua.GetActorField("Smuggler", "way").asString;
        Debug.Log(way);

        Transform playerCam = gc.player1.transform.Find("Camera");

        if (way == "start")
        {
            cam.transform.position = playerCam.transform.position;
            cam.transform.rotation = playerCam.transform.rotation;
        }

        if (way == "maiden")
        {
            foreach (GameObject go in gc.players)
            {
                if (go.name == "Rose")
                {
                    rose = go;
                    //        rose.transform.Find("Fairy").gameObject.layer = LayerMask.NameToLayer("YourLayerName");
                    //         rose.transform.Find("Fairy").gameObject.layer = LayerMask.NameToLayer("YourLayerName");
                    rose.transform.Find("Fairy").gameObject.SetActive(false);
                    rose.transform.Find("Wings").gameObject.SetActive(false);
                    Transform ilusion = rose.transform.Find("IlusionAvatar");
                    ilusion.gameObject.layer = LayerMask.NameToLayer("RealAvatar");
                    foreach (Transform ta in ilusion)
                    {
                        ta.gameObject.layer = LayerMask.NameToLayer("RealAvatar");
                    }
                    Transform roseCam = rose.transform.Find("CamDialogue");
                    mouseOrbit.target = rose.transform;
                    mouseOrbit.gameObject.transform.position = roseCam.position;
                    mouseOrbit.gameObject.transform.rotation = roseCam.rotation;
                }
            }
        }
        if (way == "rose")
        {
            GetComponent<AudioSource>().Play();
            rose.transform.Find("Fairy").gameObject.SetActive(true);
            rose.transform.Find("Wings").gameObject.SetActive(true);
            Transform ilusion = rose.transform.Find("IlusionAvatar");
            ilusion.gameObject.layer = LayerMask.NameToLayer("Ilusion");
            foreach (Transform ta in ilusion)
            {
                ta.gameObject.layer = LayerMask.NameToLayer("Ilusion");
            }
            rose.transform.Find("IlusionAvatar").gameObject.layer = LayerMask.NameToLayer("Ilusion");
            DialogueLua.SetActorField("Smuggler", "way", "None");
        }

        if (way == "brazalet")
        {
            DialogueLua.SetActorField("Smuggler", "way", "None");
            bool alreadyTaken = false;
            for (int cnt = 0; cnt < gc.questItems.Count; cnt++)
            {
                
                if (gc.questItems[cnt].name == "2400")
                {
                    alreadyTaken = true;
                }
            }

            if (alreadyTaken == false)
            {
                int newSlot = gc.questItems.Count;
                gc.questItems.Add(new InventoryRPG("2400", 1, newSlot));
                Debug.Log("questItem");
            }
            else
            {
                Debug.Log(alreadyTaken);
            }

        }

        if (way == "afterRose" && rose != null)
        {
            DialogueLua.SetActorField("Smuggler", "way", "None");
            mouseOrbit.target = playerCam;
            mouseOrbit.gameObject.transform.position = playerCam.transform.position;
            mouseOrbit.gameObject.transform.rotation = playerCam.transform.rotation;
        }

        if (way == "leave")
        {
            SmugglersToLeave();
            state = State.Leave;
        }

        if (way == "fight")
        {
            Debug.Log("fight");
            state = State.Prefight;
        }

    }

    private void Leave ()
    {
        SmugglersToLeave();
        
    }

    private void PreFight ()
    {
        Debug.Log(gc.inConversation);
        if (gc.inConversation == false)
        {
            SetUpFight();
        }
    }

    private void Fight()
    {
        bool allDead = true;

        foreach (GameObject go in gc.enemies)
        {
            if (go.GetComponent<PlayerStats>().health > 0)
            {
                allDead = false;
            }
        }

        if (allDead == true)
        {
            StopCoroutine("FSM");
            Debug.Log("Update Quest");
        }
    }

    private void SetUpFight()
    {
        Transform npcTa = transform.Find("NPC");
        foreach (Transform ta in npcTa)
        {
            Debug.Log(ta.name);
            ta.parent = null;
            ta.gameObject.tag = "Enemy";
            ta.gameObject.GetComponent<PlayerAttack>().enabled = true;
            ta.gameObject.GetComponent<PlayerStats>().enabled = true;
            ta.gameObject.GetComponent<EnemyAI>().enabled = true;
        }

        if (gc.inBattle == false)
        {
            gc.ChangeToBattle();

        }
        state = State.Fight;
    }

    private void SmugglersToLeave ()
    {
        Debug.Log("Smugglers");
        bool allInactive = true;
        foreach (Transform ta in transform.Find ("NPC"))
        {
            float distanceNPC = Vector3.Distance(ta.position, transform.parent.position); 
            Debug.Log(ta.name + "/" + distanceNPC);
            if (distanceNPC > 3)
            {
                ta.GetComponent<NavMeshAgent>().destination = transform.parent.position;
                ta.GetComponent<Animator>().SetFloat("Forward", 1);
                allInactive = false;
            }
            else
            {
        
                if (ta.gameObject.activeSelf)
                {
                    ta.GetComponent<NavMeshAgent>().isStopped = true;
                    ta.GetComponent<Animator>().SetFloat("Forward", 0);
                    ta.gameObject.SetActive(false);
                    allInactive = false;
                }
            }           
        }

        if (allInactive == true)
        {
            gc.inConversation = false;
            StopCoroutine("FSM");
            Debug.Log("End");
        }
        else
        {
            gc.inConversation = true;
        }
    }

    private void ChangeRose ()
    {

    }
}
