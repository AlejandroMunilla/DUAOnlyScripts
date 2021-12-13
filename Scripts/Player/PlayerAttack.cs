using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public float distanceToAttack = 1.8f;
    public float bulletAngle = -90;
    public float waitAnimCounter = 1;
    public bool rangedAttack = false;
    public bool secondaryRanged = false;
    public bool secondaryRangedActive = false;
    public bool waitForAnimation = false;
    public bool ignoreArmor = false;
    public bool hits = false;
    public string specialActive = "null";
    public string rangeEffect;
    public string typeDamage = "Slash";
    public GameObject spawnBullet;                 //this is the place where bullets will be instantiate
    public GameObject weapon;
    public GameObject bulletSpawnPoint = null;
    public GameObject bullet;
    public List<GameObject> bullets = null;
    public Vector3 aimingPos;                       //When aiming function is used
    public Quaternion aimingQua;
    public Vector3 relativePos;

    public bool aiming = false;     
    private PlayerStats myStats = null;
    private List<GameObject> attackers = new List<GameObject>();
    private GameObject miss;
    public Camera cam;
    public MouseOrbitImproved mouseOr;
    private GameController gc;
    private ThirdPersonUserControl tpu;
    private MySteamManager steammanager;
    private Server server;
    private PlayerStats ps;
    public bool ghostAvatar = false;
   

	void OnEnable ()
    {
        if (myStats == null)
        {
            myStats = GetComponent<PlayerStats>();
            tpu = GetComponent<ThirdPersonUserControl>();
            ps = GetComponent<PlayerStats>();
            GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
            gc = gcon.GetComponent<GameController>();

            GameObject steamObject = GameObject.FindGameObjectWithTag("SteamManager");
            steammanager = steamObject.GetComponent<MySteamManager>();
            server = steamObject.GetComponent<Server>();
            if (transform.Find ("Miss") != null)
            {
                miss = transform.Find("Miss").gameObject;
            }
            else
            {
                Debug.LogWarning(gameObject.name);
            }

            if (rangedAttack == true || secondaryRanged == true)
            {
                InstantiateBullets();
            }

            if (gameObject.tag == "Player" || gameObject.tag == "Ally")
            {
                hits = true;
            }            
        }
    }
	
    public void Attack ()
    {
        bool missedTarget = true;

        if (gc.inBattle == false)
        {

        }

        if (rangedAttack == true || secondaryRangedActive == true)
        {
            missedTarget = false;
            bullet = null;
            for (int cnt = 0; cnt < 8; cnt++)
            {
                if (bullets[cnt].activeSelf == false)
                {
                    bullet = bullets[cnt];
                }             
            }
            int randomDamage = Random.Range(myStats.minDam, myStats.maxDam) + myStats.addDam;
            bullet.GetComponent<BulletController>().damage = randomDamage;

            if (mouseOr == null)
            {
              
            }
            else if (mouseOr.aiming == true)
            {
                aiming = true;
            }
            else
            {
                aiming = false;
            }

            if (waitForAnimation == true)
            {
               Invoke ("WaitForAnimation",waitAnimCounter);

            }
            else
            {
                WaitForAnimation();
            }
           
        }

        else if (myStats.gc.enemies.Count > 0)
        {
    //        Debug.Log(gameObject.name);
            
            for (int cnt = 0; cnt < myStats.gc.enemies.Count; cnt++)
            {
                GameObject go = myStats.gc.enemies[cnt];
                if (go.tag == "Enemy")
                {
                    float distToTarget = Vector3.Distance(transform.position, go.transform.position);
                    if (distToTarget <= distanceToAttack)
                    {
                        missedTarget = false;
                        Vector3 directionToTargt = transform.position - go.transform.position;

                        float angle = Vector3.Angle(transform.forward, directionToTargt);
                        if (Mathf.Abs(angle) > 75)
                        {
                            if (go.tag == "Enemy")
                            {
                                ResolveAttack(go);
                                EnemyAI ea = go.GetComponent<EnemyAI>();

                                if (ea.damResetTimer == true)
                                {
                                    go.GetComponent<EnemyAI>().attackTimer = Time.timeSinceLevelLoad + 1.4f;
                                }
                            }
                            else if (go.tag == "EnemyDead")
                            {


                            }
                        }
                    }
                }               

            }
        }
    //    Debug.Log(missedTarget);
        if (missedTarget == true)
        {
            if (miss != null)
            {
                if (miss.activeSelf)
                {
                    miss.SetActive(false);
                }
                miss.SetActive(true);
            }

        }



    }
    
    public void CheckMeleeAttack ()
    {




    }

    public void  ResolveAttack (GameObject goToAttack)    
    {     

     //   Debug.Log(gameObject.name + "/" + goToAttack.GetComponent<Animator>().GetBool("Blocking") + "/" + specialActive);
        if (specialActive != "null")
        {
  //          Debug.Log("!null" + "/" + gameObject.name);
            if (specialActive == "CatchMe")
            {
                int damageToTarget = myStats.maxDam + myStats.addDam;
                transform.Find("Special/CatchMe").GetComponent<CatchMe>().Execution(goToAttack, damageToTarget);
            }
        }
        else if (goToAttack.GetComponent<Animator>().GetBool ("Blocking") == false)
        {

    //        Debug.Log("2");
            CalculateDamage(goToAttack, false, typeDamage, false);           
        }
        else
        {

            Collider col = goToAttack.transform.Find("Back").GetComponent<Collider>();

            
            if (col.bounds.Contains (transform.position) && goToAttack.name != "Rose")
            {
                Debug.Log("<140");
                CalculateDamage(goToAttack, false, typeDamage, false);
             
            }
            else
            {
                PlayerStats targetStats = goToAttack.GetComponent<PlayerStats>();
                if (targetStats.shield != null)
                {
                    targetStats.shield.transform.Find("Block").gameObject.SetActive(true);
             //       Debug.Log("140");
                }
                else if (goToAttack.transform.Find ("Shield/Block") != null)
                {
                    goToAttack.transform.Find("Shield/Block").gameObject.SetActive(true);
                }

            }
        }

    }

   

    public void CalculateDamage (GameObject goToAttack, bool headshot, string typeDamage, bool ranged)
    {
        PlayerStats targetStats = goToAttack.GetComponent<PlayerStats>();
       
        int damageToTarget = 0;
        if (secondaryRangedActive == false)
        {
            damageToTarget = Random.Range(myStats.minDam+1, myStats.maxDam+1) + myStats.addDam;
    //        Debug.Log(damageToTarget + "/" + myStats.minDam + "/" + myStats.maxDam);
        }
        else
        {
            if (myStats.secondMinDam == 0) myStats.secondMinDam = 1;
            if (myStats.secondMaxDam == 0) myStats.secondMinDam = 6;
            damageToTarget = Random.Range(myStats.secondMinDam, myStats.secondMaxDam+1) + myStats.secondAddDam;
   //         Debug.Log(damageToTarget + "/" + myStats.secondMinDam+ "/" + myStats.secondMaxDam);
        }

        if (headshot == true)
        {
     //       Debug.Log(damageToTarget);
            damageToTarget =  (int)(damageToTarget * myStats.headShotMultiplayer);
    //        Debug.Log(damageToTarget);
        }
    //    Debug.Log(goToAttack);

     //   Debug.Log(goToAttack + "/" + damageToTarget);
        int netDamage = 0;
        if (ignoreArmor == false)
        {
            netDamage = damageToTarget - targetStats.armor;
        }
        else
        {
            netDamage = damageToTarget;
        }

        if (myStats.magicDam > 0)
        {
            int targetMagicRes = targetStats.magidRes;
            int magicDamage = myStats.magicDam - targetMagicRes;
            if (magicDamage > 0)
            {
                netDamage = netDamage + magicDamage;
                if (targetStats.currentRegen > 0)
                {
                    targetStats.currentRegen = targetStats.currentRegen - magicDamage;
                }
            }
        }

        if (myStats.fireDam > 0)
        {
            int targetFireRes = targetStats.fireRes;
            int fireDamage = myStats.magicDam - targetFireRes;
            if (fireDamage > 0)
            {
                netDamage = netDamage + fireDamage;
                if (targetStats.currentRegen > 0)
                {
                    targetStats.currentRegen = targetStats.currentRegen - fireDamage;
                }
            }
        }

        if (targetStats.etheral == true)
        {
            if (typeDamage == "Mind" || typeDamage == "Magical")
            {

            }
            else
            {
                netDamage = (int)(netDamage * 0.5f);
            }
        }
     //   Debug.Log(goToAttack.name + "/" +  damageToTarget + "/" + targetStats.armor + "/" + netDamage);
        if (netDamage > 0)
        {

            if (myStats.dalilaDamage > 0 && ranged == false )
            {
                
                myStats.AddjustHealth(1, gameObject, false);
                if (goToAttack.transform.Find("Dalila") != null)
                {
                    goToAttack.transform.Find("Dalila").gameObject.SetActive(true);
                }
                else
                {
                     Vector3 tempPos = new Vector3(goToAttack.transform.position.x, goToAttack.transform.position.y + 1, goToAttack.transform.position.z);
                     GameObject dalila = Instantiate(Resources.Load("Effects/Dalila"), tempPos, goToAttack.transform.rotation) as GameObject;
                    dalila.name = "Dalila";
                    dalila.transform.parent = goToAttack.transform;
                    dalila.SetActive(true);

                }

                if (targetStats.currentRegen > 0)
                {
                    targetStats.AddjustRegen(-netDamage, gameObject, false); 
                }
            }
            else
            {
                if (goToAttack.transform.Find("Blood") != null)
                {
                    goToAttack.transform.Find("Blood").gameObject.SetActive(true);
                }
                else
                {
           //         Debug.Log("No Blood");
                    Vector3 tempPos = new Vector3(goToAttack.transform.position.x, goToAttack.transform.position.y + 1, goToAttack.transform.position.z);
                    GameObject fireTemp = Instantiate(Resources.Load("Effects/Blood"), tempPos, goToAttack.transform.rotation) as GameObject;
                    fireTemp.name = "Blood";
                    fireTemp.transform.parent = goToAttack.transform;
                    fireTemp.SetActive(true);
                }                
            }
            targetStats.AddjustHealth(-netDamage, gameObject, true);
            myStats.AddjustMana(1, gameObject);

            if (netDamage >0 && gameObject.tag == "Player")
            {
                myStats.AddjustExp(netDamage);
                if (gc.multiplayer == true)
                {
                    string state = goToAttack.GetComponent<EnemyAI>().state.ToString();
                    steammanager.AddExperience(gameObject.name, netDamage);

                    if (gc.server == false)
                    {
                        server.SendInfo("Server", 77, server.steamName, gameObject.name, ps.level, true, goToAttack.transform.position, goToAttack.transform.eulerAngles, targetStats.multiplayerID.ToString(), 0, 0, false, "None", Quaternion.identity, state, ps.multiplayerID.ToString(), netDamage, ps.health, false);
                    }
                    
                }         
            }          
        }
    }

    

    public void WaitForAnimation()
    {
        if (bulletSpawnPoint != null)
        {
            bullet.transform.position = bulletSpawnPoint.transform.position;
        }
        else
        {
            bullet.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z);
        }
        //   bullet.transform.rotation = Quaternion.Euler(0, (transform.rotation.y), 0);

        bullet.transform.rotation = transform.rotation;

        if (aiming == true || ghostAvatar == true)
        {
    //        Debug.Log(ghostAvatar + "/" + cam);
            if (cam != null)
            {
                bullet.transform.rotation = cam.transform.rotation;
                bullet.GetComponent<BulletController>().originalRot = cam.transform.rotation;
                bullet.GetComponent<BulletController>().aiming = true;
                if (aimingPos != new Vector3(0, 0, 0))
                {
                    relativePos = aimingPos - bullet.transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    bullet.transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                }
                else
                {
                    Debug.Log("else");
                }
            }
            else
            {
                Debug.Log(aimingPos);
                if (ghostAvatar == true)
                {
                    bullet.transform.rotation = aimingQua;
     //               Debug.Log(aimingQua);
                }
                else
                {
                    bullet.transform.rotation = Quaternion.Euler(aimingPos);
                    Debug.Log(bullet.transform.rotation);
                }
                
               
                /*
                bullet.transform.rotation = transform.Find("FPView").rotation;
                bullet.GetComponent<BulletController>().originalRot = transform.Find("FPView").rotation;*/
            }
        }
        else
        {
            bullet.GetComponent<BulletController>().aiming = false;
        }


        bullet.GetComponent<BulletController>().caster = gameObject;
        bullet.SetActive(true);
    }

    public void InstantiateBullets ()
    {
        for (int cnt = 0; cnt < 14; cnt++)
        {
            //   Debug.Log(rangeEffect + "/" +  gameObject.name);
            GameObject bulletGo = Instantiate(Resources.Load("RangeEffect/" + rangeEffect), gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            bulletGo.name = cnt.ToString();
            bulletGo.GetComponent<BulletController>().caster = gameObject;
            bulletGo.SetActive(false);
            bullets.Add(bulletGo);
        }
    }

    /*
     *  Vector3 fromPosition = source.transform.position;
 Vector3 toPosition = destination.transform.position;
 Vector3 direction = toPosition - fromPosition;*/

}
