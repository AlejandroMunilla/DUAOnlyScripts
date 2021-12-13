///This class controls the attributes of playable characters, enemies, allies, summnoned creatures. 
///In the case of players, it get the info saved on the Lua database. 
///It addjust health and mana though damage or healing caused from other gameobjects. In the case of damage, it triggers animation if exist. 


using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.AI;
using Rewired;


public class PlayerStats : MonoBehaviour {

    // *********** PUBLIC VARIABLES
    public bool attackSpecial = false;
    public bool specialActive = false;
    public bool invisible = false;
    public bool boss = false;
    public bool checkedDead = false;
    public bool beingHealed = false;
    public bool etheral = false;
    public bool smallSize = false;
    public bool mobileController = true;

    public int multiplayerID;                                   //The unique ID to be used when sending / receiving info in multiplayer online Steam Server
    public int armor;                                           
    public int health;
    public int totalHealth;
    public int mana;
    public int totalMana;
    public int maxRegen = 0;                                    //For certain monsters, e.g. Vampires, with Regeneration capacities, this is the max regeneration for that creature
    public int currentRegen = 0;                                //Current regenation value
    public int minDam;                                          //Minimum Damage in the damage dice roll
    public int maxDam;                                          //Maximum Damage on the damage dice roll
    public int addDam;                                          //Add Damage to the damage dice roll  
    public int poisonDam;                                       //Poison damage caused by object
    public int fireDam;                                          //Fire damage caused by object
    public int iceDam;                                           //Ice damage caused by object
    public int mindDam;                                         //Mind damage caused by object
    public int necroDam;                                        //etc
    public int magicDam;
    public int entropyDam;
    public int secondMinDam;                                    //Minimum Damage for secondary weapons in the dice roll
    public int secondMaxDam;
    public int secondAddDam;
    public int dalilaDamage = 0;                                //Special vampiric damage from one special weapon. 
    public int exp;                                             //Current experienced gained in game
    public int level = 1;                                       //level of the character(this gameobject)
    public int greenBar = 0;                                    //Health greenbar size, displayed on the GUI
    public int blueBar = 0;                                     //Mana blue bar size, displayed on the GUI
    public int regenBar = 0;                                    //Regeneration marked size, displayed on the GUI for certain creatures
    public int coins;                                           //Beta testing; coins gained in game for killing enemies
    public int internalCNT;                                     //
    public int poisonRes = 0;                                   //Resistances to different type of damages
    public int fireRes = 0;
    public int iceRes = 0;
    public int mindRes = 0;
    public int necroRes = 0;
    public int entropyRes = 0;
    public int magidRes = 0;
    public int strength = 10;                                   //Main attributes
    public int agility = 10;
    public int intelligence = 10;
    public int constitution = 10;
    public int carisma = 10;
    public int wisdom = 10;
    public int maxBarrier = 0;                               //Maximum barrier value
    public int barrier = 0;                                  //Current barrier value
    public float maxJumpHeight = 2;                         //Used the thirdpersoncharacter controller when jumping
    public float timerBurning = 0;                            //This is to control objects in fire so damage is per second and not by OnTriggerStay
    public float navRangeSearch = 4;                        //Distancce an object could spot enemies
    public float attacktRange = 30;                          //For Arena Mode and first person
    public float speedFP = 2;                               //To control speed of movement and animations, used by the thirdpersonuser contorller 
    public float headShotMultiplayer = 2;                   //Multiply to physical damage when shooting on the head, 
    public float deathOffSet = 0;                           //Testing. Some gameobjects animations need correction though the agent movement when dying
    public float setInactiveTime = 12;                      //Counter for skills 
    public float backStabMod = 1.2f;                        //Multiply damage when backstabbing 
    public float deadCounter = 12;                          //Time it takes before a team mate may revive falling party member. 
    public string type = "physical";                        //Type of damage, by default physical. 
    public string special;                                  //Special skills.
    public string player;                                   //It tells which player controls it. 
    public string race;                                     //Race of this gameobject
    public string steamName;                                //Taken from Steamworks API. Steam Name of player playing multiplayer
    public List<string> inventory = new List<string>();     //List of items a player/gameobject has
    public List<GameObject> invisibleParts = new List<GameObject>();      //Only when need to turn a gameobject invisible for a certain period. E.g. spells. 
    public Vector3 jumpPos;                                 //Save last jumpPos, in the event a fall killing character, it will respawn on the last known position
    public GameObject coffin = null;                        //A coffin is intantiate or enabled one a player dies, before re-spawning
    public GameObject shield = null;                        //Script "Add Shield" will search for root parent and assign this variable. 
    public GameObject rewardObj = null;                     //Reward a enemy provide after being elminated. 
    public GameController gc;                               //Link to GameController class. 
    public myGUI myGui;                                     //Link to myGUI class. 


    // ********** PRIVATE VARIABLES
    private string chat;
    private string masterIDSteam = "None";
    private Scores scores;
    private Animator anim;
    private UnityEngine.AI.NavMeshAgent nav;
    private MySteamManager steamManager;
    private bool loaded = false; 
    private bool xBoxController = false;
    private bool onSteam = false;
    

    //VARIABLES SPECIFIC FOR FPS GAME MODE
    public GameObject camGO;
    private GameObject aider;
    private ThirdPersonUserControl tpu;
    private GameObject canvas;
    private Text myText;
    private int cureCounter = 5;
    private GameObject gameController;
    private GameObject skull;
    private GameObject deadBody = null;
    private Camera cam;
    public string skill1;
    public string skill2;

    //VARIABLES TO TURN OBJECT INVISIBLE
    private List<Material> parts = new List<Material>();
    private Material transparent;

    // Use this for initialization
    void Start ()
    {        
        if (loaded == false)
        {
            loaded = true;
            anim = GetComponent<Animator>();
            gameController = GameObject.FindGameObjectWithTag("GameController");
            scores = gameController.GetComponent<Scores>();
            gc = gameController.GetComponent<GameController>();
            myGui = gameController.GetComponent<myGUI>();
            steamManager = GameObject.FindGameObjectWithTag("SteamManager").GetComponent<MySteamManager>();
            transparent = (Material)(Resources.Load("Materials/Transparente1"));
        

            for (int cnt = 0; cnt < invisibleParts.Count; cnt++)
            {
                Material mat = null;
                if (invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>() != null)
                {
                    mat = invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>().material;
                }
                else if (invisibleParts[cnt].GetComponent<MeshRenderer>())
                {
                    mat = invisibleParts[cnt].GetComponent<MeshRenderer>().material;
                }
                if (mat != null)
                {
                    parts.Add(mat);
                }
            }

             if (gameObject.tag == "Ally")
            {
                gc.allies.Add(gameObject);
            }

            if (gameObject.name == "Oleg")
            {
                special = "Healing";
            }

            if (gameObject.tag == "Player" )
            {
                string namePC = gameObject.name;
                armor = DialogueLua.GetActorField(namePC, "armor").asInt;
                minDam = DialogueLua.GetActorField(namePC, "minDam").asInt;
                maxDam = DialogueLua.GetActorField(namePC, "maxDam").asInt;
                addDam = DialogueLua.GetActorField(namePC, "addDam").asInt;
                //    health = DialogueLua.GetActorField(namePC, "curHealth").asInt;
                //    Debug.Log(DialogueLua.GetActorField(gameObject.name, "buffTotalHealth").asInt);
                //        Debug.Log(level);
                //         Debug.Log(gameObject.name + "/" + DialogueLua.GetActorField(namePC, "health").asInt);
                totalHealth = DialogueLua.GetActorField(namePC, "health").asInt + (3 * level);
       //         Debug.Log(totalHealth + "/" + gameObject.name);
                health = totalHealth;
                dalilaDamage = DialogueLua.GetActorField(namePC, "dalilaDam").asInt;
                mana = DialogueLua.GetActorField(namePC, "mana").asInt;
                totalMana = DialogueLua.GetActorField(namePC, "totalMana").asInt;
                tpu = GetComponent<ThirdPersonUserControl>();
                //           Debug.Log(namePC + "/" + health + "/" +  totalHealth);

                if (gc.isRPG)
                {
                    level = DialogueLua.GetActorField("Player1", "levelRPG").asInt;
                    if (level == 0)
                    {
                        level = 1;
                        DialogueLua.SetActorField("Player1", "levelRPG", 1);
                    }
                    if (GetComponent<EnemyAI>() == null)
                    {
                        gameObject.AddComponent<EnemyAI>();
                        GetComponent<EnemyAI>().enabled = false;
                    }
                    else
                    {
                        GetComponent<EnemyAI>().enabled = false;
                    }

                    tpu.playerR = ReInput.players.Players[0];
                    tpu.playerR.controllers.hasKeyboard = true;
                    tpu.playerR.controllers.hasMouse = true;


                    GameObject go = Instantiate(Resources.Load("PC/MapNamePC"), transform.position, transform.rotation) as GameObject;
                    go.name = "MapNamePC";
                    go.transform.parent = transform;


                    if (gameObject != gc.activePlayer)
                    {
                        GetComponent<EnemyAI>().state = EnemyAI.State.PlayerIdle;
                        GetComponent<EnemyAI>().enabled = true;
                        GetComponent<ThirdPersonCharacter>().enabled = false;
                        GetComponent<ThirdPersonUserControl>().enabled = false;
                    }

                }
                else if (gc.arenaMode == true)
                {
                    level = DialogueLua.GetActorField(gameObject.name, "levelArena").asInt;
                    if (level == 0)
                    {
                        level = 1;
                        DialogueLua.SetActorField(gameObject.name, "levelArena", 1);
                    }
                }          
                else
                {
                    level = DialogueLua.GetActorField(gameObject.name, "levelArena").asInt;
                    if (level == 0)
                    {
                        level = 1;
                        DialogueLua.SetActorField(gameObject.name, "levelArcade", 1);
                    }
                }



                if (gc.multiplayer == true)
                {
                    Debug.Log("multi");
                    tpu.playerR = ReInput.players.Players[0];
                    tpu.playerR.controllers.hasKeyboard = true;
                    tpu.playerR.controllers.hasMouse = true;

                    foreach (Joystick j in ReInput.controllers.Joysticks)
                    {
                        tpu.playerR.controllers.AddController(j, true);
                    }
                }

                if (gc.isRPG)
                {

                }


                float barMaxHeight = Screen.width * 0.08f;
                float tempRateHealth = (float)(health) / (float)(totalHealth);
                greenBar = (int)(tempRateHealth * barMaxHeight);
                float tempRateMana = (float)(mana) / (float)(totalMana);
                blueBar = (int)(tempRateMana * barMaxHeight);
                GetComponent<PlayerAttack>().enabled = true;
                nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (transform.Find("Skull") == null)
                {
                    Vector3 tempPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    skull = Instantiate(Resources.Load("Models/Skull"), tempPos, transform.rotation) as GameObject;
                    skull.name = "Skull";
                    skull.transform.parent = transform;
                    skull.SetActive(false);
                }
      //          Debug.Log("SceneFPS " + gc.sceneFP + "/" + gc.isRPG);
                if (gc.sceneFP)
                {
                    string TempName = gameObject.name;

                    if (gameObject.name == "Nanna")
                    {
                        TempName = "Balder";
                    }

                    if (gc.isRPG == true)
                    {
                        if (Resources.Load("Animator/" + TempName + "RPG") != null)
                        {
                            GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)(Resources.Load("Animator/" + TempName + "RPG"));
                        }
                        else
                        {
                            //                   Debug.Log("Animator/" + gameObject + "RPG");
                            GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)(Resources.Load("Animator/" + TempName + "FPS")) as RuntimeAnimatorController;

                        }
                    }
                    else
                    {
                        GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)(Resources.Load("Animator/" + TempName + "FPS")) as RuntimeAnimatorController;

                    }


                    skill1 = DialogueLua.GetActorField(gameObject.name, "skill1").asString;
                    skill2 = DialogueLua.GetActorField(gameObject.name, "skill2").asString;

                    if (Resources.Load("Skills/" + skill1 + "/Pasive" + skill1) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("Skills/" + skill1 + "/Pasive" + skill1), transform.position, transform.rotation) as GameObject;
                        go.name = "Pasive" + skill1;
                        go.transform.parent = transform.Find("Special");
                        go.SetActive(true);
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " no resources");
                    }

                    if (Resources.Load("Skills/" + skill1 + "/Active" + skill1) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("Skills/" + skill1 + "/Active" + skill1), transform.position, transform.rotation) as GameObject;
                        go.name = "Skill1";
                        go.transform.parent = transform.Find("Special");
                        //     go.SetActive(true);
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " no resources");
                    }


                    if (Resources.Load("Skills/" + skill2 + "/Pasive" + skill2) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("Skills/" + skill2 + "/Pasive" + skill2), transform.position, transform.rotation) as GameObject;
                        go.name = "Pasive" + skill2;
                        go.transform.parent = transform.Find("Special");
                        go.SetActive(true);
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " no resources");
                    }

                    //           Debug.Log("Skills/" + skill2 + "/Active" + skill2);
                    if (Resources.Load("Skills/" + skill2 + "/Active" + skill2) != null)
                    {
                        GameObject go = Instantiate(Resources.Load("Skills/" + skill2 + "/Active" + skill2), transform.position, transform.rotation) as GameObject;
                        go.name = "Skill2";
                        go.transform.parent = transform.Find("Special");
                    }
                    else
                    {
                        Debug.Log(gameObject.name + " no resources");
                    }
                }
                else
                {
                    string TempName = gameObject.name;

                    if (gameObject.name == "Nanna")
                    {
                        TempName = "Balder";
                    }

                    if (GetComponent<Animator>() == null)
                    {
                        Debug.Log("Null");
                    }

         //           Debug.Log("Anim");
                    if (Resources.Load("Animator/" + TempName) != null)
                    {
                        GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)(Resources.Load("Animator/" + TempName));
                    }
                }
            }
            else if (gameObject.tag == "Enemy")                
            {
                //        Debug.Log(gameObject.name);
                gc.enemies.Add(gameObject);
                if (gc.multiplayer == true)
                {
                    multiplayerID = gc.GetMultiEnemyID();
                    if (gc.server == true)
                    {
                        gameObject.AddComponent<MultiplayerSendInfo>();
                    }

                }

                float barMaxHeight = Screen.width * 0.3f;
                float tempRateHealth = (float)(health) / (float)(totalHealth);
                greenBar = (int)(tempRateHealth * barMaxHeight);

                currentRegen = maxRegen;
                float regenMaxBar = Screen.width * 0.3f;
                float tempRateRegen = (float)(currentRegen) / (float)(maxRegen);
                regenBar = (int)(tempRateRegen * regenMaxBar);
            }
            GetExperience(gameObject.name);
        }
	}

    private void OnEnable()
    {
  //      Debug.Log("Enable");
        Start();
    }

    private void OnDisable()
    {
        CancelInvoke("SetInactive");
        CancelInvoke("CheckDead");
    }

    private void GetExperience(string playerName)
    {
        int luaExperience = 0;
        if (gc.isRPG)
        {
            luaExperience = DialogueLua.GetActorField(playerName, "expRPG").asInt;
        }
        else if (gc.arenaMode)
        {
            luaExperience = DialogueLua.GetActorField(playerName, "expArena").asInt;
        }
        else
        {

        }

        int nextLevelExp = 500;
        int arimeticIncrease = 0;
        bool foundLevel = false;
        int levelTemp = 1;
        for (int cnt = 1; cnt < 10; cnt++)
        {
            //    Debug.Log(cnt);
            if (foundLevel == false)
            {
                arimeticIncrease = (cnt * 500);
                nextLevelExp = nextLevelExp + arimeticIncrease;
         //       Debug.Log(cnt + "/" + nextLevelExp);
                if (luaExperience >= 26000)
                {
                    luaExperience = 26000;
                    levelTemp = 10;
                    foundLevel = true;
                    nextLevelExp = 26000;
                }

                else if (nextLevelExp > luaExperience)
                {
                    levelTemp = cnt;
                    foundLevel = true;
                }
            }
        }

        if (gc.isRPG)
        {
            int levelMainPlayer = gc.player1.GetComponent<PlayerStats>().level;
            if (levelTemp < levelMainPlayer)
            {
                levelTemp = levelMainPlayer;
                int expMain = DialogueLua.GetActorField(gc.player1.name, "expRPG").asInt;
                DialogueLua.SetActorField(playerName, "expRPG", expMain);

            }
            int levelSpent = DialogueLua.GetActorField(playerName, "levelSpentRPG").asInt;
            int tempPoints = levelTemp - levelSpent;
            DialogueLua.SetActorField(playerName, "levelRPG", levelTemp);
        }
        else if (gc.arenaMode)
        {
            int levelSpent = DialogueLua.GetActorField(playerName, "levelSpent").asInt;
            int tempPoints = levelTemp - levelSpent;
            DialogueLua.SetActorField(playerName, "levelArena", levelTemp);
        }
        else
        {

        }
        level = levelTemp;
    }

    public void DelayedGetExperience ()
    {
        GetExperience(gameObject.name);
    }

    public void AddjustHealth (int dam, GameObject go, bool hit)
    {
        if (barrier > 0)
        {
            if (dam < 0)
            {
                if ((barrier + dam) >= 0)
                {
                    dam = 0;
                    AddjustBarrier(dam);
                }
                else 
                {
                    dam = barrier + dam;
                }
            }
        }

        health = health + dam;
       
        if (dam > 0)
        {
            Debug.Log(health + "/" + dam + "/" + totalHealth);
        }
        

        if (dam < 0)
        {
            if (transform.Find("Blood") != null)
            {
                transform.Find("Blood").gameObject.SetActive(true);
            }
        }

        

        if (mobileController == true)
        {

            scores.SendScore(0, 0, 1, 0, 0, 0, 0, 0, gameObject.name);
        }

    //    Debug.Log(dam + "/" + gameObject.name + "/" + health);
        if (health > totalHealth)
        {
            health = totalHealth;
        }
        else if (health <= 0)
        {
           
            if (currentRegen <= 0)
            {
                health = 0;
                Death();
                if (gameObject.tag == "Player")
                {
                    CheckLanguage(go.name);
                }
                else if (gameObject.tag == "Enemy")
                {
                    steamManager.AddStatInt(gameObject.name);
                    if (go.tag == "Player")
                    {
                        go.GetComponent<PlayerStats>().AddjustExp(exp);
                        steamManager.AddExperience(go.name, exp);
                    }                              
                }
                else if (go.tag == "Inter")
                {
                    foreach (GameObject go2 in gc.players)
                    {
                        if (gc.multiplayer == false)
                        {
                            go2.GetComponent<PlayerStats>().AddjustExp(exp);
                        }
                        else
                        {
                            steamManager.AddExperience(go2.name, exp);
                        }

                    }
                }

           
          //      Debug.Log("Dead");
                if (exp <= 0)
                {
                    exp = totalHealth + maxRegen;
                }


         }
            else
            {
                health = 1;
                currentRegen--;
            }

            

        }
        else if (hit == true)
        {
            Invoke("DelayedHit", 0.2f);       
            if (gameObject.tag == "Player")
            {
                tpu.timer = Time.timeSinceLevelLoad + 0.4f;
            }
            else if (gameObject.tag == "Enemy")
            {
     //           Debug.Log("Hut");
                if (boss == false)
                {
                    GetComponent<EnemyAI>().GotHit();
                }
            }
        }


        if (gameObject.tag == "Player" || gameObject.tag == "PlayerDead")
        {
   //         Debug.Log(health + "/" + totalHealth);
            float tempRate = (float)(health) / (float)(totalHealth);
            tempRate = (Screen.width * 0.08f * tempRate);
            greenBar = (int)(tempRate);
        }
        else if (gameObject.tag == "Enemy" || gameObject.tag == "EnemyDead")
        {
      //      Debug.Log(health + "/" + totalHealth);
            float tempRate = (float)(health) / (float)(totalHealth);
            tempRate = (Screen.width * 0.3f * tempRate);
            greenBar = (int)(tempRate);
        }
    }

    public void AddjustRegen (int dam, GameObject go, bool hit)
    {
 
        currentRegen = currentRegen + dam;
        if (currentRegen > maxRegen)
        {
            currentRegen = maxRegen;
        }

        if (gameObject.tag == "Enemy" || gameObject.tag == "EnemyDead")
        {
     //       Debug.Log(currentRegen + "/" + maxRegen);
            float tempRate = (float)(currentRegen) / (float)(maxRegen);
            tempRate = (Screen.width * 0.3f * tempRate);
            regenBar = (int)(tempRate);
        }
    }

    public void Death ()
    {
        anim.SetTrigger("Die");
        anim.SetBool("Dead", true);
        health = 0;
        if (gameObject.tag == "Enemy" || gameObject.tag == "EnemyDead")
        {
            gameObject.SetActive(false);
            gc.enemies.Remove(gameObject);
            gc.enemiesDead.Add(gameObject);
            if (Resources.Load("Enemy/" + gameObject.name + "Dead") != null)
            {
                deadBody = Instantiate(Resources.Load("Enemy/" + gameObject.name + "Dead"), transform.position, transform.rotation) as GameObject;
                deadBody.name = "Enemy/" + gameObject.name + "Dead";
                deadBody.SetActive(true);
                Debug.Log(deadBody.name);
            }

               
   
            Invoke("CheckDead", 0.1f);
            anim.SetBool("Stun", false);
            anim.SetBool("Dead", true);
            anim.SetTrigger("Die");
            gameObject.tag = "EnemyDead";
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
         //   GetComponent<Rigidbody>().isKinematic
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<EnemyAI>().state = EnemyAI.State.Dead;
            GetComponent<EnemyAI>().dead = true;
            Invoke("SetInactive", 12);
            if (transform.Find ("Reward") != null)
            {
                GameObject go = transform.Find("Reward").gameObject;
                go.transform.parent = null;
                go.SetActive(true);
            }       
            
            if (rewardObj != null)
            {
                rewardObj.transform.position = transform.position;
                rewardObj.SetActive(true);
                /*
                GameObject go = new GameObject();
                go.name = rewardQuest;*/

            }


        }
        else if (gameObject.tag == "Player")
        {
            GetComponent<ThirdPersonCharacter>().enabled = false;
            GetComponent<ThirdPersonUserControl>().enabled = false;
        //    GetComponent<ThirdPersonUserControl>().enabled = false;
            nav.isStopped = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.tag = "PlayerDead";
            anim.SetTrigger("Die");
            if (gc.arenaMode == false)
            {
                Invoke("ResetDeath", 5);
            }
            else
            {
                skull.SetActive(true);
            }
            if (gc.sceneFP == true)
            {
                if (gameController.transform.Find("MemberDown") != null)
                {
                    AudioSource audioS = gameController.transform.Find("MemberDown").GetComponent<AudioSource>();
                    audioS.Play();
                }
                else
                {
                    GameObject go2 = Instantiate(Resources.Load("Audio/MemberDown"), gc.transform.position, gc.transform.rotation) as GameObject;
                    go2.name = "MemberDown";
                    go2.transform.parent = gameController.transform;
                    AudioSource audioS =go2.GetComponent<AudioSource>();
                    audioS.Play();
                }
  
            }
 
            nav.baseOffset = nav.baseOffset -  deathOffSet;
            jumpPos = transform.position;
            Invoke("CheckDead", 0.01f);
        }
        else if (gameObject.tag == "Ally")
        {
            Invoke("CheckDead", 0.1f);
            anim.SetBool("Stun", false);
            anim.SetBool("Dead", true);
            anim.SetTrigger("Die");
            gameObject.tag = "AllyDead";
            UnityEngine.AI.NavMeshAgent nav = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            nav.isStopped = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //   GetComponent<Rigidbody>().isKinematic
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<EnemyAI>().state = EnemyAI.State.Dead;
            GetComponent<EnemyAI>().dead = true;
            Invoke("SetInactive", setInactiveTime);
            gc.allies.Remove(gameObject);
        }
    }

    public void AddjustMana (int dam, GameObject go)
    {
        mana = mana + dam;
        if (mana > totalMana)
        {
            mana = totalMana;
        }
        else if (mana < 0)
        {
            mana = 0;

        }

        if (gameObject.tag == "Player")
        {
            float tempRate = (float)(mana) / (float)(totalMana);
            tempRate = (float) ( myGui.portraitWidth * tempRate);
            blueBar = (int)(tempRate);
        }
    }

    public void AddjustExp (int expToAdd)
    {
        exp = exp + expToAdd;
    //    Debug.Log(gameObject.name + "/" + expToAdd);
        if (gc.arenaMode == true || gc.multiplayer)
        {
            int luaExperience = DialogueLua.GetActorField(gameObject.name, "expArena").asInt + expToAdd;
            DialogueLua.SetActorField(gameObject.name, "expArena", luaExperience);
        }
        else
        {
            int luaExperience = DialogueLua.GetActorField(gameObject.name, "exp").asInt + expToAdd;
            DialogueLua.SetActorField(gameObject.name, "exp", luaExperience);
        }
  
    //    Debug.Log(luaExperience);
        
    }

    private void SetInactive ()
    {
        gameObject.SetActive(false);
    }

    private void DelayedHit ()
    {
        anim.SetTrigger("Hit");
        
    }

    public void CriticalHit ()
    {
        Debug.Log("critical");
        if (name == "Skeleton" || name == "SkeletonArcher")
        {
            Debug.Log("critical");
            GameObject go = Instantiate(Resources.Load("Models/BrokenSkeleton1"), transform.position, transform.rotation) as GameObject;
            go.SetActive(true);
            deadBody.SetActive(false);
            foreach (Transform ta in go.transform)
            {
                ta.parent = null;
            }
        }
    }    

    public void ResetDeath ()
    {
        transform.position = jumpPos;
        gameObject.SetActive(false);
        if (coffin == null)
        {
            coffin = Instantiate(Resources.Load("PC/Coffin"), jumpPos, Quaternion.identity) as GameObject;

        }
        else
        {
            coffin.SetActive(true);
            
        }

        coffin.transform.position = jumpPos;

        Invoke("Revive", 4);

    }

    public void InstantiateCoffin ()
    {

        if (CheckValidNav())
        {
            health = 0;
            navRangeSearch = 4;
            ResetDeath();
        }
        else
        {
            navRangeSearch = navRangeSearch + 2;
            Invoke("InstantiateCoffin", 0);
        }
    }

    private bool CheckValidNav ()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(jumpPos, out hit, navRangeSearch, NavMesh.AllAreas))
        {
            jumpPos = new Vector3(hit.position.x, (hit.position.y + 0.4f), hit.position.z);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Revive ()
    {
        if (gc.arenaMode == false)
        {
            coffin.SetActive(false);
            transform.Find("Explosion").gameObject.SetActive(true);
        }
        
        transform.position = jumpPos; 
       
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<ThirdPersonUserControl>().enabled = true;
        GetComponent<ThirdPersonCharacter>().enabled = true;        
        nav.enabled = true;
        nav.isStopped = false; 
        gameObject.SetActive(true);
        health = (totalHealth);
        AddjustHealth(totalHealth, gameObject, false);
        AddjustMana(totalMana, gameObject);
        gameObject.tag = "Player";
        anim.SetTrigger("Idle");
        anim.SetBool("Dead", false);
        anim.SetBool("OnGround", true);
        anim.SetBool("Aiming", false);
        
        if (skull != null)
        {
            if (skull.activeSelf)
            {
                skull.SetActive(false);
            }
        }

        nav.baseOffset = nav.baseOffset + deathOffSet;
    }

    private void CheckDead ()
    {
        Debug.Log(checkedDead + "/" + gameObject.name);

        /*
        if (gameObject.tag == "Enemy" || gameObject.tag == "EnemyDead")
        {
            deadCounter = deadCounter - 0.1f;
            Invoke("SetInactive", deadCounter);
            DialogueManager.ShowAlert(deadCounter.ToString());
            if (gc.enemies.Contains(gameObject))
            {               
                gc.enemies.Remove(gameObject);
                DialogueManager.ShowAlert(gameObject.name);
            }
            if (gc.enemiesDead.Contains(gameObject) == false)
            {
                gc.enemiesDead.Add(gameObject);
            }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //   GetComponent<Rigidbody>().isKinematic
            GetComponent<CapsuleCollider>().enabled = false;
            Invoke("CheckDead", 0.1f);
        }*/

        if (checkedDead == false)
        {
            anim.SetTrigger("Die");
            anim.SetBool("Dead", true);
            Invoke("CheckDead", 0.01f);
        } 



    }


    private void WaitHelp ()
    {

    }

    private void CheckLanguage (string killer)
    {
        string lan = DialogueLua.GetVariable("language").asString;
        if (lan == "en")
        {
            chat = gameObject.name + " was taken down by " + killer;
        }
        else if (lan == "es")
        {
            chat = gameObject.name + " fue abatido por " + killer;
        }
        else if (lan == "fr")
        {
            chat = gameObject.name + " a été abattu par " + killer;
        }

        DialogueManager.ShowAlert(chat);
    }

    public void TurnInvisible (float invTime)
    {
        if (gc.arenaMode == false)
        {
            transform.Find("Effects/Smoke").gameObject.SetActive(true);
        }
        else
        {
            for (int cnt = 0; cnt < invisibleParts.Count; cnt++)
            {


                if (invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>() != null)
                {
                    invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>().material = transparent;
                }
                else if (invisibleParts[cnt].GetComponent<MeshRenderer>())
                {
                    invisibleParts[cnt].GetComponent<MeshRenderer>().material = transparent;
                }


            }
        }
        invisible = true;
        Invoke("TurnVisible", invTime);
    }

    public void TurnVisible ()
    {
        if (gc.arenaMode == false)
        {
            transform.Find("Effects/Smoke").gameObject.SetActive(false);
        }
        else
        {
            for (int cnt = 0; cnt < invisibleParts.Count; cnt++)
            {
                if (invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>() != null)
                {
                    invisibleParts[cnt].GetComponent<SkinnedMeshRenderer>().material = parts[cnt];
                }
                else if (invisibleParts[cnt].GetComponent<MeshRenderer>())
                {
                    invisibleParts[cnt].GetComponent<MeshRenderer>().material = parts[cnt];
                }

            }
        }
        invisible = false;
        CancelInvoke("TurnVisible");
    }

    public void AddjustBarrier (int addBarrier)
    {
        barrier = barrier + addBarrier;
        if (barrier > maxBarrier)
        {
            barrier = maxBarrier;
        }
        if (barrier < 0)
        {
            barrier = 0;
            maxBarrier = 0;
            if (transform.Find("Barrier") != null)
            {
                transform.Find("Barrier").gameObject.SetActive(false);
            }
            
        }
    }



}
