using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using PixelCrushers.DialogueSystem;


public class ThirdPersonUserRose : MonoBehaviour
{
    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private PlayerAttack pa;
    private PlayerStats ps;
    private NavMeshAgent nav;
    private Animator anim;
    private Rigidbody rb;

    public bool jumping = false;
    private string controller = "";
    private string fireControl;
    private string axisX;
    private string axisY;
    private string specialButton;
    private string block;
    private string jump;
    private float timer;
    private float jumpTimer;
    private float jumpSpeed;
    private GameObject specialGO = null;
   
    private void Start()
    {
        pa = GetComponent<PlayerAttack>();
        ps = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        m_Character = GetComponent<ThirdPersonCharacter>();

        Debug.Log(gameObject.name);

        if (gameObject.name == "Oleg")
        {
            fireControl = "mouse 0";
            axisY = "Vertical";
            axisX = "Horizontal";
            specialButton = "f";
            block = "mouse 1";
            jump = "space";


        }
        else if (gameObject.name == "Rose")
        {

            controller = DialogueLua.GetActorField(ps.player, "control").asString;
            Debug.Log(controller);
            fireControl = controller + " button 0";
            axisX = controller + "_X";
            axisY = controller + "_Y";
            specialButton = controller + " button 1";
            block = controller +" button 3";
            jump = controller + " button 2";
        }

        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )

        timer = Time.timeSinceLevelLoad;
    }


    /*
    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }*/



    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
    //    float h = CrossPlatformInputManager.GetAxis("Horizontal");
     //   float v = CrossPlatformInputManager.GetAxis("Vertical");

        float h = CrossPlatformInputManager.GetAxis(axisX);
        float v = CrossPlatformInputManager.GetAxis(axisY);
    //    Debug.Log(h + "/" + v);
        //    bool crouch = Input.GetKey(KeyCode.C);
        bool crouch = false;

        if (Input.GetKeyUp(fireControl) && anim.GetBool("Blocking") == false) 
        {
            if (ps.health > 0)
            {
                if (ps.specialActive == false)
                {
                    if (Time.timeSinceLevelLoad > timer)
                    {
                        int randomNo = UnityEngine.Random.Range(1, 4);
                        anim.SetTrigger("Attack" + randomNo);
                        //              Debug.Log("anim");
                        timer = Time.timeSinceLevelLoad + 1;
                        pa.Attack();
                    }
                }
            }
        }

        if (Input.GetKeyUp(specialButton))
        {
        //    Debug.Log(gameObject.name);
            if (ps.health > 0)
            {

                if (ps.specialActive == false)
                {
                    Debug.Log(gameObject.name);
                    if (specialGO == null)
                    {
                        Debug.Log(ps.special);
                        specialGO = transform.Find("Special/" + ps.special).gameObject;                        
                    }
                    specialGO.SetActive(true);
                }
            }
        }

        if (Input.GetKeyDown(block))
        {
            if (ps.health > 0)
            {

                if (ps.specialActive == false)
                {
                    anim.SetBool("Blocking", true);
                    anim.SetTrigger("Block");

                }
            }
        }


        if (Input.GetKeyUp(block))
        {
            if (ps.health > 0)
            {

                if (ps.specialActive == false)
                {
                    anim.SetBool("Blocking", false);
                    anim.SetTrigger("Idle");

                }
            }
        }



        if (Input.GetKey (jump))
        {
            //       Debug.Log(jump);
            if (jumping == false)
            {
                jumping = true;
                ps.jumpPos = transform.position;
                nav.isStopped = true;
                nav.enabled = false;
                rb.velocity = transform.up * 5;
                jumpTimer = Time.timeSinceLevelLoad;
                ps.jumpPos = transform.position;
                Invoke("CheckGround", 0);
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


            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;

#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

        // pass all parameters to the character control script
      
        m_Jump = false;
        m_Character.Move(m_Move, crouch, m_Jump);
    //    m_Jump = false;

    }

    private void InvokeTurnOffSpecial ()
    {
        ps.specialActive = false;
    }

    private void Jumping ()
    {
        Debug.Log(jump);



    }

    private void CheckGround ()
    {
        //   rb.velocity = transform.right * jumpSpeed;


        Debug.Log(jump);
        //    if (transform.position.y < 5.1)
        bool grounded = CheckGroundStatus();
        if (grounded == true & (jumpTimer + 1) < Time.timeSinceLevelLoad)
        {
            CancelInvoke("CheckGround");     
            nav.enabled = true;
            nav.isStopped = false;
            jumping = false;
        }
        else
        {
            //    rb.velocity = -transform.up * 2;
            float PosONeg = 1;
            float h = CrossPlatformInputManager.GetAxis(axisX);
       //     Debug.Log(h);
            if (h > 0)
            {
                PosONeg = -1;
            }
            rb.AddForce (Vector3.right * 2600 * h);
            Invoke("CheckGround", 0);
        //    anim.applyRootMotion = true;
        }

        if ((jumpTimer + 3) < Time.timeSinceLevelLoad)
        {
            CancelInvoke("CheckGround");
            nav.enabled = true;
            nav.isStopped = false;
            jumping = false;
            transform.position = ps.jumpPos;
            ps.health = 0;            
            ps.ResetDeath();
        }

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
         //   Debug.Log(distance);
            if (distance < 0.1f )
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
}



