using UnityEngine;
using Mirror;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class Scores : MonoBehaviour
{
    private string currentScene;
    private bool messageRegistered = false;
    public string ip2 = "None";
    public string ip3 = "None";
    public string ip4 = "None";

    public ThirdPersonUserControl tpc2;
    public ThirdPersonUserControl tpc3;
    public ThirdPersonUserControl tpc4;

    public GameObject playerPuppet;
    public GameObject NetManager;
    private ThirdPersonUserControl tpc;
 //  private NetworkConnection netClient;
    private NetworkIdentity clientID;
    private PreGame preGame;
    public class ScoreMessage : MessageBase
    {
        public float x;
        public float y;
        public int attack;
        public int block;
        public int jump;
        public int special;
        public int left;
        public int right;
        public string message;        
    }

    

    private void Start()
    {
        //    Invoke("CheckConnection", 1);
        Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        currentScene = scene.name;
     //   Debug.Log(currentScene);

        if (GetComponent<PreGame>() != null)
        {
            preGame = GetComponent<PreGame>();
        }

        CheckConnection();

    }

    public void CheckConnection()
    {
   
        if (NetworkServer.active)
        {
            Debug.Log("Server");
            if (messageRegistered == false)
            {
                NetworkServer.RegisterHandler<ScoreMessage>(OnScore);
                NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
                messageRegistered = true;
            }
  
        }
      
        Invoke("CheckConnection", 1);
    }

    public void SendScore(float x, float y, int attack, int block, int jump, int special, int left, int right, string message)
    {
        ScoreMessage msg = new ScoreMessage()
        {
            x = x,
            y = y,
            attack = attack,
            block = block,
            jump = jump,
            special = special,
            left = left,
            right = right,
            message = message
            
        };   
    }

    public void SetupClient()
    {
        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
        NetworkClient.Connect("localhost");
    }

    public void OnScore(NetworkConnection conn, ScoreMessage msg)
    {
    //    Debug.Log("OnScoreMessage " + msg.x + "/" + msg.y + "/" +  msg.attack);
    //    Debug.Log(conn.identity + "/" + msg.attack + "/" + conn.address + "/" + conn.connectionId );

        if (currentScene != "PreLevel")
        {

           if (ip2 == conn.address)
            {
                tpc2.attackWifi = msg.attack;

                tpc2.blockWifi = msg.block;

                tpc2.jumpWifi = msg.jump;

                tpc2.specialWifi = msg.special;


                tpc2.xAxis = msg.x;
                tpc2.yAxis = msg.y;
            }

        }
        else
        {
            preGame.MobileMessages(conn.address, msg.x, msg.y, msg.attack, msg.block, msg.jump, msg.special, msg.left, msg.right, msg.message);
        }
    }



}

