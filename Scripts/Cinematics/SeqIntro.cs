using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeqIntro : MonoBehaviour {

    public GameObject inn;
    public GameObject town;
    public AudioClip[] sounds;
    private int soundNo = 0;
    private bool alive = true;
    private AudioSource audio;
    private Transform destination;
    private float timer = 0;
    private float timerSkip;
    private float timerEnd;
    private string textGUI = "";
    private Texture2D background;

    private enum State
    {
        Seq0,
        Seq00,
        Seq01,
        Seq02,
        Seq03,
        Seq04,
        Seq05,
        Seq06,
        Seq07,
        Seq08,
        Seq09,
        Seq10,
        Seq11,
        Seq12
    }
    private State state;

    private Camera mainCamera;

	void Start ()
    {
        mainCamera = Camera.main;
        audio = GetComponent<AudioSource>();
    //    Debug.Log(mainCamera);
        mainCamera.transform.position = transform.Find("1").position;
        mainCamera.transform.rotation = transform.Find("1").rotation;
        destination = transform.Find("2");
        background = (Texture2D)(Resources.Load("GUI/background"));
        state = State.Seq0;
        StartCoroutine("FSM");
    }

    private void Update()
    {

        if (Input.anyKeyDown && timerSkip > Time.timeSinceLevelLoad)
        {
    //        Debug.Log("skip");
            ChangeScene();
        }
        else if (Input.anyKeyDown)
        {
   //         Debug.Log("Hit");

            timerSkip = Time.timeSinceLevelLoad + 2;
        }
    }

    private void OnGUI()
    {
        if (timerSkip > Time.timeSinceLevelLoad)
        {
            GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.7f, Screen.width * 0.8f, Screen.height * 0.1f), DialogueLua.GetActorField ("Dictionary", "warningSkip").asString);
        }
    }

    private IEnumerator FSM ()
    {
        while (true)
        {
            switch (state)
            {
                case State.Seq0:
                    yield return new WaitForSeconds(2);
                    Seq0();
                    
                    break;

                case State.Seq00:
                    yield return new WaitForSeconds(2);
                    Seq00();

                    break;

                case State.Seq01:
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;


                case State.Seq02:
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

                case State.Seq07:
                    Seq07();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq08:
                    yield return new WaitForSeconds(2);
                    Seq08();
                    yield return new WaitForSeconds(2);
                    break;

                case State.Seq09:
                    Seq09();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq10:
                    Seq10();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq11:
                    Seq11();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq12:
                    Seq12();
                    yield return new WaitForSeconds(0);
                    break;
            }
        }
        yield return null;

    }

    private void Seq0 ()
    {
        

        string language = DialogueLua.GetVariable("language").asString;
        
        Debug.Log(language);
        DialogueManager.SetLanguage(language);

    //    state = State.Seq00;

        //    Debug.Log(language);

        if (language == "en" || language == "es" || language == "fr")
        {
            
            state = State.Seq00;
     //       DialogueManager.StartConversation("01Intro");
        }

        /*
        string chosen1 = DialogueLua.GetActorField("Player1", "chosen").asString;
        if (chosen1 != "")
        {
            state = State.Seq01;
            DialogueManager.StartConversation("01Intro");
        }*/
    }

    private void Seq00 ()
    {
        state = State.Seq01;
        DialogueManager.StartConversation("01Intro");
   //     GetComponent<DialogueSystemTrigger>().OnUse();
    }

    private void Seq01 ()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.5f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.5f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        if (distance <= 0.2f)
        {
            state = State.Seq02;
            timer = Time.timeSinceLevelLoad;
            destination = transform.Find("4");
        }
    }

    private void Seq02()
    {
    //    Debug.Log("2");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.5f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.5f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        if (timer + 3 <= Time.timeSinceLevelLoad)
        {
            timer = Time.timeSinceLevelLoad;
            state = State.Seq03;
            destination = transform.Find("5");
        }
    }

    private void Seq03()
    {
        Debug.Log("3");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.5f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.5f);


        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        if (timer + 6 <= Time.timeSinceLevelLoad)
        {
            state = State.Seq04;
            destination = transform.Find("6");           
        }
    }

    private void Seq04()
    {

        
        Debug.Log("4");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.5f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.5f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        
        if (distance <= 0.2f)
        {
            town.SetActive(true);
            inn.SetActive(false);
        //    GetComponent<AudioSource>().volume = 0.6f;
            state = State.Seq05;
            destination = transform.Find("7");
        }
    }

    private void Seq05()
    {


     //   Debug.Log("5");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.2f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.2f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        if (GetComponent<AudioSource>().volume >0)
        {
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume - 0.005f;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
        

        if (distance <= 0.2f)
        {
            state = State.Seq06;
            AudioClip audioClip = (AudioClip)(Resources.Load("Audio/Boss LOOP"));
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioClip;
            audio.Play();
            audio.volume = 0.15f;
            destination = transform.Find("8");
        }
    }

    private void Seq06()
    {


        Debug.Log("6");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.75f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.75f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);


        if (distance <= 0.2f)
        {
            state = State.Seq07;
        //    GetComponent<DialogueSystemTrigger>().OnUse();
            destination = transform.Find("9");
        }
    }

    private void Seq07()
    {


        Debug.Log("7");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.4f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.4f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);


        
        if (distance <= 0.2f)
        {
            state = State.Seq08;
            timerEnd = Time.timeSinceLevelLoad;
        }
    }


    private void Seq08()
    {
        if (Time.timeSinceLevelLoad > (timerEnd + 18))
        {
            ChangeScene();
        }
        
    }


    private void Seq09()
    {


        Debug.Log("9");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.4f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.4f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        /*
        if (distance <= 0.2f)
        {
        //    state = State.Seq05;
            destination = transform.Find("5");
        }*/
    }


    private void Seq10()
    {


        Debug.Log("10");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.4f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.4f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        /*
        if (distance <= 0.2f)
        {
        //    state = State.Seq05;
            destination = transform.Find("5");
        }*/
    }


    private void Seq11()
    {


        Debug.Log("11");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.4f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.4f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        /*
        if (distance <= 0.2f)
        {
        //    state = State.Seq05;
            destination = transform.Find("5");
        }*/
    }


    private void Seq12()
    {


        Debug.Log("12");
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, destination.transform.position, Time.deltaTime * 0.4f);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, destination.transform.rotation, Time.deltaTime * 0.4f);
        float distance = Vector3.Distance(mainCamera.transform.position, destination.position);

        
        if (distance <= 8)
        {
            PlaySound();
        }
    }

    private void PlaySound ()
    {
        AudioClip clip = sounds[soundNo];
        audio.clip = clip;
        soundNo++;
        float clipLength = clip.length;
        Invoke("PlaySound", clipLength);
    }


    private void ChangeScene ()
    {
        if (DialogueLua.GetVariable("rpgMode").asString == "Yes")
        {
            SceneManager.LoadScene("The Cross Inn");
        }
        else
        {
            SceneManager.LoadScene("01Scene");
        }
        
    }
}
