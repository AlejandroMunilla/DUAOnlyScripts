using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using PixelCrushers.DialogueSystem;
using Rewired;


[RequireComponent(typeof(ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
    public bool mobileControl = false;
    public float timer;
    public bool aiming = false;
    public bool coolDown1 = false;
    public bool coolDown2 = false;
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private MultiplayerSendInfo multiSend;
    public Transform m_Cam;                  // A reference to the main camera in the scenes transform
    public Camera cam;                       // Main Camera
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private PlayerAttack pa;
    private PlayerStats ps;
    private NavMeshAgent nav;
    private Animator anim;
    private Rigidbody rb;
    private CameraController camController;
    private GameController gc;
    private CapsuleCollider cc;

    public bool jumping = false;
    public bool curing = false;
    public bool sniperZoom = false;
    public float h;                            //input axix x
    public float v;                            //input axis y
    public float xAxis;
    public float yAxis;
    public float coolDownTime1;
    public float coolDownTime2;
    public int attackWifi;
    public int blockWifi;
    public float blockWifiFloat;
    public int jumpWifi;
    public int specialWifi;
    public int leftWifi;
    public int rightWifi;
    public string messageWifi;

    private bool grounded = true;
    public string controller = "";
    private string fireControl;
    public string axisX;
    public string axisY;
    private string specialButton;
    private string block;
    private string jump;
    private string escape;
    private string aim;
    private string fire2;
    private string axisX2;
    private string axisY2;
    private string change;
 
   
    private float jumpTimer = 0;
    private float raycastTimer = 0;
    private float jumpSpeed;
    private float jumpInitialHeight;
    private bool dPadActive = true;
    private Material mat;
    private GameObject specialGO = null;
    private MouseOrbitImproved mouseOrImp;
    private myGUI myGui;
    public GameObject grayReticule;
    public GameObject redReticule;
    public GameObject secondWeaponBack;
    public GameObject secondWeaponHand;
    public GameObject[] firstWeaponsHand;
    public GameObject[] firstWeaponBack;
    public GameObject target;
    public GameObject rpgMenu;
    private Vector2 reticulePos;
    private GameObject playerBeingCure;    
    private GameObject rpgCamera;
    private GameObject virtualCursor;
    public int cureTotalTime = 7;
    private Transform camTransform;
    public Transform fpViewTransform;
   
    public Player playerR;
      
    
   
    private void Start()
    {
        GameObject gcon = GameObject.FindGameObjectWithTag("GameController");
        gc = gcon.GetComponent<GameController>();
        pa = GetComponent<PlayerAttack>();
        ps = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        myGui = gcon.GetComponent<myGUI>();
      
          

        if (cam != null)
        {
            mouseOrImp = cam.GetComponent<MouseOrbitImproved>();
            pa.cam = cam;
        }

        
        if (gc.sceneFP == false || gc.isRPG)
        {
        
            foreach (Camera ca in Camera.allCameras)
            {
                if (ca.name == "Camera1")
                {
                    cam = ca;
                }
            }
            camController = cam.GetComponent<CameraController>();
            redReticule = cam.transform.Find("RedReticle").gameObject;
            grayReticule = cam.transform.Find("GreyReticle").gameObject;

        }

        if (gc.multiplayer == true)
        {
            if (GetComponent<MultiplayerSendInfo>() == null)
            {
                gameObject.AddComponent<MultiplayerSendInfo>();
            }

            multiSend = GetComponent<MultiplayerSendInfo>();
        }
              
        cc = GetComponent<CapsuleCollider>();
        LoadLua();

        if (gc.isRPG)
        {
            if (secondWeaponBack != null)
            {
                secondWeaponBack.SetActive(false);
                secondWeaponHand.SetActive(false);
            }

            foreach (GameObject go in firstWeaponBack)
            {
                go.SetActive(true);
            }
            foreach (GameObject go in firstWeaponsHand)
            {
                go.SetActive(false);
            }
            rpgCamera = rpgMenu.transform.Find("Camera").gameObject;
        }

        // get the transform of the main camera
        if (Camera.main != null)
        {
            if (gc.sceneFP == false)
            {
                m_Cam = Camera.main.transform;
            }
           
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }


        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ThirdPersonCharacter>();
        timer = Time.timeSinceLevelLoad;
        camTransform = transform.Find("Camera");
        fpViewTransform = transform.Find("FPView");
    //    Debug.Log(fpViewTransform);

       
    }


    public void LoadLua ()
    {
        ps = GetComponent<PlayerStats>();
        controller = DialogueLua.GetActorField(ps.player, "controller").asString;
        //     Debug.Log(controller + "/" + ps.player + playerR.name);

        //    playerR = Rewired.ReInput.players.GetPlayer(controller);
        fireControl = "Fire";
        fire2 = "Fire2";
        axisX = "Move Horizontal";
        axisY = "Move Vertical";
        specialButton = "Special";
        block = "Block";
        jump = "Jump";
        escape = "Escape";
        aim = "Aim";
        axisX2 = "AxisX2";
        axisY2 = "AxisY2";
        change = "Change";
        if (cam != null)
        {
            cam.GetComponent<MouseOrbitImproved>().axisX = axisX2;
            cam.GetComponent<MouseOrbitImproved>().axisY = axisY2;
            cam.GetComponent<MouseOrbitImproved>().playerR = playerR;
        }
        else
        {
            Invoke("CheckCamera", 0.1f);
        }


    }
    
    private void Update()
    {
        myGui.skill1Cool[ps.internalCNT] -= Time.deltaTime;
        myGui.skill2Cool[ps.internalCNT] -= Time.deltaTime;

        if (mobileControl == false)
        {
            if (gc.inConversation == false)
            {
                if (gc.sceneFP && rpgMenu.activeSelf == false)
                {
                    Ray ray = cam.ViewportPointToRay(new Vector3(0.50f, 0.495f, 0.5f));
                    RaycastHit hit;
                    GameObject tempTarget = null;
                    if (Physics.Raycast(ray, out hit, 500))
                    {

                        tempTarget = hit.transform.gameObject;
                        GameObject hitArea = hit.collider.transform.gameObject;
                        //             Debug.Log(tempTarget.name + "/" + tempTarget.tag);

                        if (hitArea.name == "HeadShot" || tempTarget.name == "HeadShot")
                        {
                            redReticule.SetActive(true);
                        }

                        else if (tempTarget.tag == "Enemy" || tempTarget.tag == "AllyEnemy")
                        {
                            redReticule.SetActive(true);


                        }
                        else if (tempTarget.tag == "NPC" || tempTarget.tag == "Ally")
                        {
                            if (gc.inBattle == false)
                            {
                                if (gc.inConversation == false)
                                {
                                    redReticule.SetActive(true);
                                    if (tempTarget.GetComponent<DisplayName>() != null)
                                    {
                                        tempTarget.GetComponent<DisplayName>().timer = 0.2f;
                                        if (tempTarget.GetComponent<DisplayName>().enabled == false)
                                        {
                                            tempTarget.GetComponent<DisplayName>().enabled = true;
                                        }
                                    }
                                    //              Debug.Log(raycastTimer + "/" + Time.realtimeSinceStartup);
                                    if (raycastTimer < Time.realtimeSinceStartup)
                                    {
                                        if (tempTarget.GetComponent<DialogueSystemTrigger>() != null)
                                        {
                                            if (playerR.GetButtonDown(fireControl))
                                            {
                                                float distanceConv = Vector3.Distance(transform.position, tempTarget.transform.position);
                                                if (distanceConv <= 7)
                                                {
                                                    raycastTimer = Time.realtimeSinceStartup + 4;
                                                    tempTarget.GetComponent<DialogueSystemTrigger>().OnUse();
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (tempTarget.tag == "Inter")
                        {
                            redReticule.SetActive(true);

                            if (tempTarget.GetComponent<Exit>() != null)
                            {
                                GameObject exitChild = tempTarget.transform.Find("Exit").gameObject;
                                exitChild.GetComponent<DestroyObj_timer>().timer = 0.2f;
                                if (exitChild.activeSelf == false)
                                {

                                    exitChild.SetActive(true);
                                }

                                if (tempTarget.GetComponent<DisplayName>() != null)
                                {
                                    tempTarget.GetComponent<DisplayName>().timer = 0.2f;
                                    if (tempTarget.GetComponent<DisplayName>().enabled == false)
                                    {
                                        tempTarget.GetComponent<DisplayName>().enabled = true;
                                    }
                                }

                                if (raycastTimer < Time.realtimeSinceStartup)
                                {
                                    if (playerR.GetButtonDown(fireControl))
                                    {
                                        //                          Debug.Log("Fire");
                                        float distanceConv = Vector3.Distance(transform.position, tempTarget.transform.position);
                                        if (distanceConv <= 7)
                                        {
                                            raycastTimer = Time.realtimeSinceStartup + 4;
                                            tempTarget.GetComponent<Exit>().SaveAndExit();
                                        }
                                    }
                                }
                            }
                            else if (tempTarget.GetComponent <AddBook>() != null)
                            {
                              
                                if (tempTarget.transform.Find("Yellow") != null)
                                {
                      //              Debug.Log("Secret");
                                    GameObject childYellow = tempTarget.transform.Find("Yellow").gameObject;
                                    childYellow.GetComponent<DestroyObj_timer>().timer = 0.2f;
                                    childYellow.SetActive(true);
                                }
                                if (raycastTimer < Time.realtimeSinceStartup)
                                {
                                    if (playerR.GetButtonDown(fireControl))
                                    {
                                        //                          Debug.Log("Fire");
                                        float distanceConv = Vector3.Distance(transform.position, tempTarget.transform.position);
                                        if (distanceConv <= 5)
                                        {
                                            tempTarget.GetComponent<AddBook>().NewBook();
                                        }
                                    }
                                }
                            }
                            else if (tempTarget.GetComponent<SecretVisible>() != null)
                            {
                                Debug.Log("Secret");
                                if (tempTarget.transform.Find ("Yellow") != null)
                                {
                                    Debug.Log("Secret");
                                    GameObject childYellow = tempTarget.transform.Find("Yellow").gameObject;
                                    childYellow.GetComponent<DestroyObj_timer>().timer = 0.2f;
                                    childYellow.SetActive(true);
                                }
                                if (raycastTimer < Time.realtimeSinceStartup)
                                {
                                    if (playerR.GetButtonDown(fireControl))
                                    {
                                        //                          Debug.Log("Fire");
                                        float distanceConv = Vector3.Distance(transform.position, tempTarget.transform.position);
                                        if (distanceConv <= 5)
                                        {
                                            tempTarget.GetComponent<SecretVisible>().TriggerUncover();
                                        }
                                    }
                                }
                            }
                            else if (tempTarget.GetComponent<OpenRoom>() != null)
                            {
                                GameObject exitChild = tempTarget.transform.Find("Exit").gameObject;
                                exitChild.GetComponent<DestroyObj_timer>().timer = 0.2f;
                                if (exitChild.activeSelf == false)
                                {

                                    exitChild.SetActive(true);
                                }

                                if (tempTarget.GetComponent<DisplayName>() != null)
                                {
                                    tempTarget.GetComponent<DisplayName>().timer = 0.2f;
                                    if (tempTarget.GetComponent<DisplayName>().enabled == false)
                                    {
                                        tempTarget.GetComponent<DisplayName>().enabled = true;
                                    }
                                }

                                if (raycastTimer < Time.realtimeSinceStartup)
                                {
                                    if (playerR.GetButtonDown(fireControl))
                                    {
                                        //                          Debug.Log("Fire");
                                        float distanceConv = Vector3.Distance(transform.position, tempTarget.transform.position);
                                        if (distanceConv <= 7)
                                        {
                                            raycastTimer = Time.realtimeSinceStartup + 4;
                                            tempTarget.GetComponent<OpenRoom>().OpenDoor();
                                        }
                                    }
                                }
                            }
                        }

                        else
                        {
                            redReticule.SetActive(false);

                        }
                        pa.aimingPos = hit.point;
                        target = hit.transform.gameObject;
                    }
                    else
                    {
                        pa.aimingPos = new Vector3(0, 0, 0);
                    }

                    if (gc.inConversation == false)
                    {
                        //            Debug.Log(gc.isRPG + "/" + playerR.GetButtonUp(change));
                        if (gc.isRPG == true)
                        {
                            if (playerR.GetButtonUp(change))
                            {
                                gc.ChangeCharacter();
                            }
                        }

                        if (playerR.GetButtonDown(aim))
                        {
                            if (gc.safeArea == false)
                            {
                                if (anim.GetBool("Blocking") == false)
                                {
                                    mouseOrImp.aiming = true;
                                    aiming = true;

                                    pa.secondaryRangedActive = true;
                                    anim.SetTrigger("AimTrigger");
                                    anim.SetBool("Aiming", true);

                                }
                            }
                            grayReticule.SetActive(true);
                            mouseOrImp.target = fpViewTransform;


                            if (sniperZoom == true)
                            {
                                Debug.Log(cam.name);
                                cam.transform.Find("Zoom").gameObject.SetActive(true);
                            }
                        }
                        if (playerR.GetButton(aim))
                        {
                            //             Debug.Log(secondWeaponBack.name);
                            if (secondWeaponBack != null && gc.safeArea == false)
                            {
                                if (secondWeaponBack.activeSelf == true)
                                {

                                    secondWeaponBack.SetActive(false);
                                    secondWeaponHand.SetActive(true);
                                    if (firstWeaponBack != null)
                                    {

                                        foreach (GameObject go in firstWeaponBack)
                                        {
                                            go.SetActive(true);
                                        }
                                        foreach (GameObject go in firstWeaponsHand)
                                        {
                                            go.SetActive(false);
                                        }

                                    }
                                }
                            }

                            //    reticule.enabled = true;



                            //       Debug.Log(hit.point);
                            if (playerR.GetButtonDown(fire2) && gc.safeArea == false)
                            {
                                //             Debug.Log(pa.secondaryRangedActive + "/" +  pa.aimingPos);
                                FireButton();
                                //              Debug.Log(hit.point);
                            }
                        }
                        else if (playerR.GetButtonUp(aim))
                        {
                            mouseOrImp.aiming = false;
                            mouseOrImp.target = camTransform;
                            aiming = false;
                            //       GetComponent<Reticule>().enabled = false;
                            //      grayReticule.SetActive(false);
                            redReticule.SetActive(false);
                            anim.SetBool("Aiming", false);

                            if (gc.safeArea == false)
                            {
                                pa.secondaryRangedActive = false;
                                if (secondWeaponBack != null)
                                {
                                    if (secondWeaponBack.activeSelf == false)
                                    {

                                        secondWeaponBack.SetActive(true);
                                        secondWeaponHand.SetActive(false);
                                        if (firstWeaponBack != null)
                                        {

                                            foreach (GameObject go in firstWeaponBack)
                                            {
                                                go.SetActive(false);
                                            }
                                            foreach (GameObject go in firstWeaponsHand)
                                            {
                                                go.SetActive(true);
                                            }

                                        }
                                    }
                                }
                            }


                            if (sniperZoom == true)
                            {
                                cam.transform.Find("Zoom").gameObject.SetActive(false);
                            }
                            //       InvokeRepeating ("CheckAimingOff", 0.1f, 0.1f);



                        }

                    }

                }


                //   Debug.Log(coolDown1);
                if (playerR.GetButtonUp("Skill1") && myGui.skill1Cool[ps.internalCNT] <= 0)
                {
                    Debug.Log("SKill1");
                    bool test = true;
                    if (DialogueLua.GetActorField(gameObject.name, "Skill1/3").asString == "Yes" || test)
                    {
                        if (transform.Find("Special/Skill1") != null)
                        {
                            transform.Find("Special/Skill1").gameObject.SetActive(true);
                            myGui.skill1Cool[ps.internalCNT] = coolDownTime1;
                        }

                    }


                }

                if (playerR.GetButtonUp("Skill2") && myGui.skill2Cool[ps.internalCNT] <= 0)
                {
                    Debug.Log("SKill2");
                    Debug.Log(DialogueLua.GetActorField(gameObject.name, "skill2/3").asString);
                    if (DialogueLua.GetActorField(gameObject.name, "skill2/3").asString == "Yes")
                    {
                        if (transform.Find("Special/Skill2") != null)
                        {
                            transform.Find("Special/Skill2").gameObject.SetActive(true);
                            myGui.skill2Cool[ps.internalCNT] = coolDownTime2;
                        }

                    }

                }

                if (playerR.GetAxis("Dpad Horizontal") > 0.3f && dPadActive == true)

                {
                    //         Debug.Log("Dpad R" + gameObject.name);
                    myGui.ChangeTexture(gameObject, true);
                    dPadActive = false;
                    Invoke("ActivateDpad", 0.3f);

                }
                else if (playerR.GetAxis("Dpad Horizontal") < -0.3f && dPadActive == true)
                {
                    //          Debug.Log("Dpad L" + gameObject.name);
                    myGui.ChangeTexture(gameObject, false);
                    dPadActive = false;
                    Invoke("ActivateDpad", 0.3f);
                }
                else if (playerR.GetAxis("Dpad Vertical") < -0.3f && dPadActive == true)
                {

                    dPadActive = false;
                    Invoke("ActivateDpad", 0.3f);
                    myGui.DropItem(gameObject);
                }
                else if (playerR.GetAxis("Dpad Vertical") > 0.3f && dPadActive == true)
                {

                    dPadActive = false;
                    Invoke("ActivateDpad", 0.3f);
                    myGui.UseItem(gameObject);
                }

                if (anim.GetBool("Blocking") == false && gc.safeArea == false)
                {
                    if (playerR.GetButtonUp(fireControl) == true)
                    {
                        FireButton();
                        //         Debug.Log("Fire");
                    }
                }


                if (playerR.GetButtonUp(specialButton))
                {
                    SpecialButton();

                }

                if (playerR.GetButtonUp(block))
                {
             //       Debug.Log("Block");

                    if (ps.health > 0)
                    {

                        if (ps.specialActive == false)
                        {
                            StopBlocking();

                        }
                    }
                }

                if (playerR.GetButtonDown(block))
                {
       //             Debug.Log("BlockDown");
                    StartBlocking();
                }

                if (playerR.GetButtonDown(jump) && grounded == true & (jumpTimer + 1) < Time.timeSinceLevelLoad)
                {
                    //        Debug.Log(jump);
                    JumpButton();

                }



            }


        }
        else
        {
            if (anim.GetBool("Blocking") == false)
            {
                if (attackWifi == 1)
                {
                    FireButton();
                }
            }

            if (specialWifi == 1)
            {
                SpecialButton();

            }

            if (blockWifi == 1)
            {
                StartBlocking();
            }

            if (blockWifi == 0 && anim.GetBool("Blocking") == true)
            {
                //         Debug.Log(blockWifiFloat);
                blockWifiFloat = blockWifiFloat - (30 * Time.deltaTime);


                if (ps.health > 0)
                {

                    if (ps.specialActive == false)
                    {
                        if (blockWifiFloat <= 0)
                        {

                            StopBlocking();
                            blockWifiFloat = 0;
                        }

                    }
                }
            }

            if (jumpWifi == 1)
            {
                //        Debug.Log(jump);
                JumpButton();

            }

           
        }

    }



    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
   //     Debug.Log(rpgMenu.activeSelf);
        bool crouch = false;

        

            if (mobileControl == true)
            {
                //     Debug.Log("move");
                h = xAxis;
                v = yAxis;
            }
            else
            {

   //             h = CrossPlatformInputManager.GetAxis(axisX);

                h = playerR.GetAxis("Move Horizontal");
                v = playerR.GetAxis("Move Vertical");
                //         v = CrossPlatformInputManager.GetAxis(axisY);
   //             Debug.Log(h + "/" + v);
            }

    //    Debug.Log(h + "/" + v);



        
        if (gc.sceneFP == false)
        {
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

            if (viewPos.y < 0.02f)
            {
                v = 1;
            }

            if (viewPos.x < 0.01f)
            {
                h = 1;
            }
            else if (viewPos.x > 0.98f)
            {
                h = -1;
            }
      //      Debug.Log((h) + "/" + (v));
            m_Move = v * Vector3.forward + h * Vector3.right;
            m_Character.Move(m_Move, crouch, m_Jump);
        }
        else
        {
            if (m_Cam != null && gc.inConversation == false)
            {
                /*
                m_Move = v * m_CamForward + h * m_Cam.right;
                m_Character.Move(m_Move, crouch, m_Jump);*/


                // calculate camera relative direction to move:
                if (aiming == false)
                {
                    if (anim.GetBool("Blocking") == false)
                    {

                        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                        if (v < 0)
                        {
                            //       anim.SetTrigger("AimTrigger");
                            //       anim.SetBool("Aiming", true);
                            //        transform.position += (transform.forward * v * Time.deltaTime) + (transform.right * h * Time.deltaTime);


                            if (anim.GetBool("Rotate") == false)
                            {
                                anim.SetBool("Rotate", true);
                                anim.SetTrigger("RotateTrigger");
                            }
                            anim.SetFloat("Turn", h, 0.1f, Time.deltaTime);
                            anim.SetFloat("Forward", v);
                            transform.position += (transform.forward * v * 2 *  Time.deltaTime) + (transform.right * h * 2 * Time.deltaTime);

                            cam.transform.rotation = transform.rotation;

                        }
                        else
                        {
                            anim.SetFloat("Turn", h, 0.1f, Time.deltaTime);
                            anim.SetFloat("Forward", v);
                            //            Debug.Log(v);
                            transform.position += (transform.forward * v * ps.speedFP * 2 * Time.deltaTime) + (transform.right * ps.speedFP * h * 2 * Time.deltaTime);


                            //          m_Move = v * m_CamForward + h * m_Cam.right;
                            m_Move = v * Vector3.forward + h * Vector3.right;
                            m_Character.Move(m_Move, crouch, m_Jump);
                            //          cam.transform.rotation = transform.rotation;

                            /*
                            m_Move = v * m_CamForward + h * m_Cam.right;
                            anim.SetBool("Rotate", false);
                            m_Character.Move(m_Move, crouch, m_Jump);*/
                        }

                    }





                }
                else
                {
                    if (v > 0.5f)
                    {
                        v = 0.5f;
                    }
                    if (h > 0.5f)
                    {
                        h = 0.5f;
                    }

                    anim.SetFloat("Turn", h, 0.1f, Time.deltaTime);
                    anim.SetFloat("Forward", v);


                    transform.position += (transform.forward * v * Time.deltaTime) + (transform.right * h * Time.deltaTime);

                    m_Move = v * m_CamForward + h * m_Cam.right;
                    m_Character.Move(m_Move, crouch, m_Jump);


           //         transform.rotation += transform.Rotate(transform.rotation.x, transform.rotation.y + h * Time.deltaTime, transform.rotation.z);

                }        
            }
            else
            {
                if (cam != null)
                {
                    m_Cam = cam.transform;
                }
                 
            }
        }

        
        m_Jump = false;


    }

    private void LateUpdate()
    {
        if (playerR.GetButtonUp(escape))
        {
    //        Debug.Log(rpgMenu.activeSelf);
            if (gc.isRPG)
            {
                if (rpgMenu.activeSelf)
                {
                    TurnOffMenu();

                }
                else
                {
        //            Debug.Log("Enable");
                    myGui.enabled = false;
                    cam.enabled = false;
                    rpgMenu.SetActive(true);
                    rpgCamera.SetActive(true);
                    gc.inConversation = true;
                }
            }
            else
            {
                gc.TogglePause();
                gc.GetComponent<MenuInGame>().ToggleOnOff();
            }

        }
    }

    private void InvokeTurnOffSpecial ()
    {
        ps.specialActive = false;
    }

    private void Jumping ()
    {
        Debug.Log(jump);



    }

    public void CheckGround ()
    {
        //    if (transform.position.y < 5.1)
        grounded = CheckGroundStatus();


        if (grounded == true & (jumpTimer + 0.5f) < Time.timeSinceLevelLoad)
        {
            CancelInvoke("CheckGround");
            nav.enabled = true;
            nav.isStopped = false;
            cc.isTrigger = true;
            jumping = false;
            if (gameObject.name == "Rose")
            {
                anim.SetTrigger("Idle");
                anim.SetBool("OnGround", true);
            }
            else
            {
                anim.SetBool("OnGround", true);
            }
        }
        else
        {
            Invoke("CheckGround", 0);
        }


        /*
        if (grounded == true & (jumpTimer + 1.5f) < Time.timeSinceLevelLoad)
        {
            Debug.Log(transform.position.y + "/" + jumpInitialHeight + ps.maxJumpHeight);
            Debug.Log("grounded");
            CancelInvoke("CheckGround");     
            nav.enabled = true;
            nav.isStopped = false;
            jumping = false;
        }
        else*/


        /*
        else if ((jumpTimer + 3) < Time.timeSinceLevelLoad)
        {
            CancelInvoke("CheckGround");
            nav.enabled = true;
            nav.isStopped = false;
            jumping = false;
            transform.position = ps.jumpPos;
            ps.health = 0;            
            ps.ResetDeath();
        }
        else
        {
            Debug.Log("else");
            rb.velocity = transform.up * 2;            
            //    rb.AddForce(transform.up * 1000);
            float PosONeg = 1;
            float h = CrossPlatformInputManager.GetAxis(axisX);
            //     Debug.Log(h);
            if (h > 0)
            {
                PosONeg = -1;
            }
            //    rb.AddForce (Vector3.right * 2600 * h);
            Invoke("CheckGround", 0);
            //    anim.applyRootMotion = true;
        }*/

    }

    
    private bool CheckGroundStatus()
    {
        RaycastHit hitInfo;

     //   Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));

        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))

        {
            float distance = Vector3.Distance(hitInfo.point, transform.position);

            if (distance < 0.2f )
            {
                return true;
            }
        }
        else
        {
            return false;
        }


        return false;
    }

    private void AddjustPosition ()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
 
        if (viewPos.x < 0.98 && viewPos.x > 0.02f)
        {
            CancelInvoke("AddjustPosition");
        }
        else if (viewPos.x > 1)
        {


        }
        else if (viewPos.x < 0)
        {

        }
    }

    private void FireButton ()
    {
    //   Debug.Log(ps.health + "/");
        if (ps.health > 0)
        {
            


            if (ps.specialActive == false)
            {
                
         //       Debug.Log("Special false");
                if (Time.timeSinceLevelLoad > timer)
                {
                    if (gc.multiplayer == true)
                    {
                        multiSend.action = "At";
                        multiSend.SendInfo();
                    }


                    if (mouseOrImp == null)
                    {
                        int randomNo = UnityEngine.Random.Range(1, 4);
                        anim.SetTrigger("Attack" + randomNo);
                        //              Debug.Log("anim");

                        if (gameObject.name == "Fred")
                        {
                            Invoke("ForceIdle", 0.8f);

                        }
                    }
                    else if (mouseOrImp.aiming == false || secondWeaponBack == null)
                    {
                        int randomNo = UnityEngine.Random.Range(1, 4);
                       
                        //              Debug.Log("anim");
 
                        if (gameObject.name == "Fred")
                        {
                            if (mouseOrImp.aiming != true)
                            {
                                anim.SetTrigger("Attack" + randomNo);
                                Invoke("ForceIdle", 0.8f);

                            }                          

                        }
                        else
                        {
                            anim.SetTrigger("Attack" + randomNo);
                        }

                    }
 
                    timer = Time.timeSinceLevelLoad + 1;
                    pa.Attack();
        //            Debug.Log("Attack");
                    if (gc.multiplayer == true)
                    {
                        multiSend.action = "Attack";
                    }
                }
            }
            else
            {
                if (ps.attackSpecial == true)
                {
                    ps.attackSpecial = false;
                    
                }
            }
        }
    }

    private void SpecialButton ()
    {
        if (ps.health > 0)
        {

            if (ps.specialActive == false)
            {
                //            Debug.Log(gameObject.name);
                if (specialGO == null)
                {
           //         Debug.Log(ps.special);
                    specialGO = transform.Find("Special/" + ps.special).gameObject;
                }

                specialGO.SetActive(true);
            }
            else
            {
                ps.attackSpecial = true;
            }
        }
    }

    private void StartBlocking ()
    {
        if (ps.health > 0)
        {

            if (ps.specialActive == false)
            {
                blockWifiFloat = 1.0f;


                //       bool playerDead = false;
        //        Debug.Log(curing);
                if (curing == false && gc.sceneFP == true)
                {
                    GameObject goPlayer = null;
                    foreach (GameObject go in gc.players)
                    {
                        
                        if (playerBeingCure == null)
                        {
                            if (go.tag == "PlayerDead")
                            {
                                //                    Debug.Log(go.name);
                                float distanceGO = Vector3.Distance(go.transform.position, transform.position);
                                if (distanceGO < 4)
                                {
                                    if (go.GetComponent<PlayerStats>().beingHealed == false)
                                    {
                                        if (goPlayer != gameObject)
                                        {
                                            goPlayer = go;
                                            //                  Debug.Log(go.name);
                                        }

                                    }

                                }
                            }

                            if (goPlayer != null)
                            {
                                playerBeingCure = goPlayer;
                                StartCure();

                            }
                            else if (anim.GetBool("Blocking") == false)
                            {

                                anim.SetBool("Blocking", true);
                                anim.SetTrigger("Block");
                            }
                        }

                    }
                }
                else if (curing == false)
                {
                    if (anim.GetBool("Blocking") == false)
                    {

                        anim.SetBool("Blocking", true);
                        anim.SetTrigger("Block");
                    }
                }

            }
        }
    }

    private void StopBlocking ()
    {
   //     Debug.Log(curing);
        if (curing == true)
        {
            curing = false;
            GetComponent<CureCounter>().counter = cureTotalTime;
            anim.SetBool("Curing", false);
            StopCuring();
        }
        else
        {
            anim.SetBool("Blocking", false);
        }
        
        anim.SetTrigger("Idle");
    }

    private void JumpButton ()
    {
        if (jumping == false)
        {
            jumping = true;
            ps.jumpPos = transform.position;
            nav.isStopped = true;
            nav.enabled = false;
            cc.isTrigger = false;
            Invoke("CheckGround", 0);

            if (gameObject.name == "Rose")
            {
                //    rb.velocity = transform.up * 5;

                anim.SetTrigger("JumpTrigger");
                anim.SetBool("OnGround", false);
                rb.velocity = transform.up * 10;
            }
            else
            {
                rb.velocity = transform.up * 7;
                anim.SetBool("OnGround", false);
            }

            jumpTimer = Time.timeSinceLevelLoad;
            ps.jumpPos = transform.position;
            jumpInitialHeight = transform.position.y;

            //    jumpSpeed = anim.GetFloat("Forward");
            float PosONeg = 1;
            if (h < 0)
            {
                PosONeg = -1;
            }
            //     rb.AddForce(Vector3.right * 6 * h * PosONeg);
            //     rb.AddForce(Vector3.up * 300);
            anim.applyRootMotion = false;
            jumpTimer = Time.timeSinceLevelLoad;
        }
    }

    private void ForceIdle ()
    {
        anim.SetTrigger("Idle");
    }

    private void StartCure ()
    {
  //      Debug.Log("Cure");
        PlayerStats gops = playerBeingCure.GetComponent<PlayerStats>();
        gops.beingHealed = true;
        anim.SetBool("Curing", true);
        anim.SetTrigger("CureTrigger");
        curing = true;
        playerBeingCure.GetComponent<CureCounter>().counter = cureTotalTime;        
        playerBeingCure.GetComponent<CureCounter>().enabled = true;
        GetComponent<CureCounter>().enabled = true;
        GetComponent<CureCounter>().counter = cureTotalTime;
        AudioSource audioS = playerBeingCure.transform.Find("Skull").gameObject.GetComponent<AudioSource>();
        audioS.loop = true;
        audioS.clip = GetComponent<CureCounter>().healing;
        audioS.Play();
        Invoke ("Curing", 1);
    }

    private void Curing ()
    {
        Debug.Log("X");
        if (GetComponent<CureCounter>().counter <= 0)
        {
            
            playerBeingCure.GetComponent<PlayerStats>().Revive();
            StopCuring();
        }
        else
        {
            if (playerBeingCure != null)
            {
                playerBeingCure.GetComponent<CureCounter>().counter = playerBeingCure.GetComponent<CureCounter>().counter - 1;
                GetComponent<CureCounter>().counter = playerBeingCure.GetComponent<CureCounter>().counter;
                Invoke("Curing", 1);
            }

        }
    }

    private void StopCuring ()
    {
        CancelInvoke("StopCuring");
        curing = false;
        playerBeingCure.GetComponent<CureCounter>().counter = cureTotalTime;
        playerBeingCure.GetComponent<CureCounter>().enabled = false;
        GetComponent<CureCounter>().enabled = false;
        GetComponent<CureCounter>().counter = cureTotalTime;
        playerBeingCure.GetComponent<PlayerStats>().beingHealed = false;       

        AudioSource audioS = playerBeingCure.transform.Find("Skull").gameObject.GetComponent<AudioSource>();
        audioS.clip = GetComponent<CureCounter>().healed;
        audioS.Play();
        audioS.loop = false;
        playerBeingCure = null;
    }   

    private void CheckAimingOff ()
    {
        Debug.Log("Off");
        if (aiming == false)
        {
            grayReticule.SetActive(false);
            redReticule.SetActive(false);
        }
    }


    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    private void CheckCamera ()
    {
        Camera tempCam = null;
        int tempCNT = 1000;
        for (int cnt = 0; cnt < gc.players.Count; cnt++)
        {
            if (gameObject == gc.players[cnt])
            {
                foreach (Camera camera in Camera.allCameras)
                {
                    string cameraString = "Camera" + (cnt + 1).ToString();
                    if (camera.name == cameraString)
                    {
                        cam = camera;
                        cam.GetComponent<MouseOrbitImproved>().axisX = axisX2;
                        cam.GetComponent<MouseOrbitImproved>().axisY = axisY2;
                        cam.GetComponent<MouseOrbitImproved>().playerR = playerR;
                        tempCam = cam;
                    }
                }
            }
        }

        if (tempCam == null)
        {
            Invoke("CheckCamera", 0.1f);

        }
    }

    private void ActivateDpad ()
    {
        dPadActive = true;
        CancelInvoke("ActivateDpad");
    }

    public void TurnOffMenu ()
    {
        rpgMenu.SetActive(false);
        rpgCamera.SetActive(false);
        cam.enabled = true;
        myGui.enabled = true;
        gc.inConversation = false;
   //     cam.gameObject.GetComponent<MouseOrbitImproved>().enabled = true;
   //     gc.activePlayer.GetComponent<ThirdPersonCharacter>().enabled = true;
   //     gc.activePlayer.GetComponent<ThirdPersonUserControl>().enabled = true;
    }
    
}



