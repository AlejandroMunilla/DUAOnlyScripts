using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float planeD;
    public float planeI;
    public bool move = true;
    public bool playerRear = false;
    public bool playerFront = false;
    private Camera cam;
    private GameController gc;
    private Rigidbody rb;
    private Mesh mesh;
    private Vector3 boundPoint1;
    private Vector3 boundPoint2;
    private Vector3 boundPoint3;
    private Vector3 boundPoint4;
    private Vector3 boundPoint5;
    private Vector3 boundPoint6;
    private Vector3 boundPoint7;
    private Vector3 boundPoint8;
    private Collider cube;
    private Collider cube2;


    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    //    cube = transform.Find("CubeD").gameObject.GetComponent<Collider>();
    //    cube2 = transform.Find("CubeI").gameObject.GetComponent<Collider>();
        planeD = Time.timeSinceLevelLoad;
        planeI = Time.timeSinceLevelLoad -2;
    //    transform.Find("PlaneD").gameObject.SetActive(true);
    //    transform.Find("PlaneI").gameObject.SetActive(true);
        foreach (Camera ca in Camera.allCameras)
        {
            if (ca.name == "Camera1")
            {
                cam = ca;
            }
            
        }

    }

    private void OnEnable()
    {
        Start();
    }


    private void Update()
    {
        if (move == true )
        {
            playerRear = false;
            playerFront = false;
            foreach (GameObject go in gc.players)
            {
                Vector3 viewPos = cam.WorldToViewportPoint(go.transform.position);
                if (viewPos.x < 0.1f)
                {
                    playerRear = true;
                }

                if (viewPos.x > 0.8f)
                {
                    playerFront = true;

                }
            }

            if (playerRear == true)
            {
                rb.velocity = transform.right * 0;
                rb.velocity = Vector3.zero;

            }
            else
            {
                if (playerFront == true)
                {
                    rb.velocity = transform.right * 2.4f;
                }
                else
                {
                    rb.velocity = transform.right * 0;
                    rb.velocity = Vector3.zero;
                }
            }
        }
        else
        {
            rb.velocity = transform.right * 0;
            rb.velocity = Vector3.zero;
        }

   //     string debuggingString = "move:" +  move + "/playerFront:" + playerFront + "/playerRear" + playerRear;
   //     Debug.Log(debuggingString);
    }

    /*
    private void OnGUI()
    {
        GUI.skin.label.fontSize = (int)( Screen.width * 0.04f);
        GUI.Label (new Rect(Screen.width * 0.2f, Screen.height * 0.5f, Screen.width * 0.6f, Screen.height * 0.1f), debuggingString);
    }*/
}
