using UnityEngine;
using System.Collections;
using Rewired;


public class MouseOrbitImproved : MonoBehaviour
{
    public Transform target;
    public bool aiming = false;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yOffset = 1;

    public float yMinLimit = -20f;
    public float yMaxLimit = 100f;
    public float xTemp;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Rigidbody rigidbody;

    public string axisX = "Mouse X";
    public string axisY = "Mouse Y";

    public float x = 0.0f;
    private float xMin;
    private float xMax;
    float y = 0.0f;
    private bool control = false;
    float tempRotY;
    ThirdPersonCharacter tpc;
    ThirdPersonUserControl tpu;
    GameController gc;
    Animator anim;
    public Player playerR = null;

    private bool loaded = false;



    // Use this for initialization
    void Start()
    {
        if (loaded == false)
        {
            gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            loaded = true;
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            rigidbody = GetComponent<Rigidbody>();

            if (target != null)
            {
                tpc = target.root.gameObject.GetComponent<ThirdPersonCharacter>();
                tpu = target.root.gameObject.GetComponent<ThirdPersonUserControl>();
                anim = target.root.gameObject.GetComponent<Animator>();
            }


            // Make the rigid body not change rotation
            if (rigidbody != null)
            {
                rigidbody.freezeRotation = true;
            }
        }

    }

    private void OnEnable()
    {

        Start();
    }

    void LateUpdate()
    {
    //    Debug.Log(target.root.name + "/" + gameObject.name + "/" + playerR);

        if (playerR != null && gc.inConversation == false)
        {

            Transform taGO = target.transform;
            x += playerR.GetAxis("AxisX2") * xSpeed * distance * 0.02f;
            y -= playerR.GetAxis("AxisY2") * ySpeed * 0.02f;

            Quaternion rotation = Quaternion.identity;
            if (aiming == true && control == false)
            {
               
                x = taGO.eulerAngles.y;
                transform.rotation = target.rotation;
                control = true;      

            }


     //       xMin = target.transform.eulerAngles.y - 50;
      //      xMax = target.transform.eulerAngles.y +50 ;

            xMin = ClampAngle(taGO.eulerAngles.y - 30, taGO.eulerAngles.y - 30, taGO.eulerAngles.y - 30);   
            xMax = ClampAngle(target.transform.eulerAngles.y + 30,target.transform.eulerAngles.y + 30, target.transform.eulerAngles.y + 30);

            //      Debug.Log(x + "/" + xMin + "/" + xMax);
      //      Debug.Log(playerR.GetAxis("AxisX2"));
            if (x > xMax * 0.98f)
            {

     //           target.root.transform.Rotate(0, x * 1 * Time.deltaTime, 0);
                x = xMax;
                //          float rotationY = Input.GetAxis(axisX) * xSpeed * 0.02f;
                float rotationY = playerR.GetAxis("AxisX2") * xSpeed * 0.08f;
                target.root.transform.Rotate(0, rotationY, 0);
        //        target.root.rotation = Quaternion.Euler(target.root.gameObject.GetComponent<Rigidbody>().rotation.eulerAngles + new Vector3(0f, 10 * x, 0));

            }
            else if (x  < xMin * 1.02f)
            {
                  x = xMin;
         //       float rotationY = Input.GetAxis(axisX) * xSpeed * 0.02f;
                float rotationY = playerR.GetAxis("AxisX2") * xSpeed * 0.08f;
                target.root.transform.Rotate(0, rotationY, 0);

                //          float tempX = x - (2 * x * Time.deltaTime * distance * 0.02f);
                //           target.root.transform.Rotate(0, -tempX, 0);

            }
            else
            {
         //       Debug.Log("else");
                target.root.transform.Rotate(0, 0, 0);
            }


            y = ClampAngle(y, yMinLimit, yMaxLimit);
    //        rotation = Quaternion.Euler(y, x, 0);    //needed option 1;

    //        Debug.Log(x + "/" + xMin + "/" + xMax);



    //        Debug.Log(rotation);


            //      distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position + new Vector3(0, yOffset, 0);


            
            if (aiming == true)
            {
                //      Debug.Log(transform.rotation.y);
                rotation = Quaternion.Euler(y, x, 0);    //option 2 
                transform.rotation = rotation; 
                transform.position = target.position;
            }

            else 
            {
                control = false;
               

            //    transform.rotation = rotation;
                Quaternion rotTemp = Quaternion.Euler(y, target.transform.rotation.eulerAngles.y, target.transform.rotation.eulerAngles.z);
                transform.rotation = rotTemp;
                //           transform.rotation = target.rotation;    // neeeded option 1
                transform.position = target.position;
            }

            /*
            else if ( target.transform.root.GetComponent<ThirdPersonUserControl>().v < -0.2f)
            {
                control = false;
                transform.position = target.position;
                transform.rotation = target.rotation;
         //       distance = 3.4f;
            }*/



        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


    

    
}


