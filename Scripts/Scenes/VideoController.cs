using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private bool alive = true;
    private string platform;
    private string warningText = "PRESS ANY KEY TO SKIP INTRO";
    private float timer;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private AudioClip audio;
    
    private enum State
    {
        Seq01,
        Seq02,
        Seq03,
        Seq04,
        Seq05
    }
    private State state;

    private IEnumerator FSM()
    {
        while (alive)
        {
            switch (state)
            {
                case State.Seq01:
                    yield return new WaitForSeconds(1);
                    Seq01();
                    yield return new WaitForSeconds(0);
                    break;

                case State.Seq02:
                    Seq02();
                    yield return new WaitForSeconds(3);
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
            }

        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = Camera.main.GetComponent<VideoPlayer>();
        audioSource = Camera.main.GetComponent<AudioSource>();
  //      videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
//        videoPlayer.EnableAudioTrack(0, true);
//videoPlayer.SetTargetAudioSource(0, audioSource);
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.controlledAudioTrackCount = 1;             // <-- We have added this line. It tells video player that you will have one audio track playing in Unity AudioSource.

        videoPlayer.Prepare();
        state = State.Seq01;
        StartCoroutine("FSM");

        platform = Application.platform.ToString();
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log(platform);
        }
        else if (Application.platform == RuntimePlatform.XboxOne)
        {

        }

    }



    private void OnDisable()
    {
        StopCoroutine("FSM");
    }

    private void Seq01()
    {

     //   string chosen1 = DialogueLua.GetActorField("Player1", "chosen").asString;
    //    Debug.Log(chosen1);
        string language = DialogueLua.GetVariable("language").asString;
    //    Debug.Log(language);

        if (language == "en" || language == "es" || language == "fr")
        {
            DialogueManager.SetLanguage(language);
            ChangeLanguage(language);
            DialogueManager.DisplaySettings.subtitleSettings.subtitleCharsPerSecond = 11;
            videoPlayer.Play();
            audioSource.Play();
            state = State.Seq02;
        } 
    }

    private void Seq02()
    {
        GetComponent<DialogueSystemTrigger>().OnUse();
        state = State.Seq03;
    }

    private void Seq03()
    {
        string endCon = DialogueLua.GetVariable("endConversation").asString;
        Debug.Log(endCon);
        if (endCon == "Done")
        {
            GetComponent<DialogueSystemTrigger>().OnUse();
            
        }
    }

    private void Seq04 ()
    {
        string endCon = DialogueLua.GetVariable("endConversation").asString;
        Debug.Log(endCon);
        if (endCon == "Final")
        {
            StopCoroutine("FSM");
            Invoke("ChangeScene", 5);
        }
    }


    private void Seq05()
    {

    }

    private void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("01Scene");
    }

    private void Update()
    {

        if (Input.anyKeyDown && timer > Time.timeSinceLevelLoad)
        {
            Debug.Log("skip");
            ChangeScene();
        }
        else if (Input.anyKeyDown)
        {
            Debug.Log("Hit");

            timer = Time.timeSinceLevelLoad + 2;
        }
    }

    private void OnGUI()
    {
        if (timer > Time.timeSinceLevelLoad)
        {
            GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.7f, Screen.width * 0.8f, Screen.height * 0.1f), warningText);
        }
    }

    private void ChangeLanguage (string languageActive)
    {
        if (languageActive == "en")
        {
            warningText = "PRESS ANY KEY TO SKIP INTRO";
        }
        else if (languageActive == "es")
        {
            warningText = "PRESIONAD CUALQUIER BUTTON PARA PASAR LA SECUENCIA";
        }
        else if (languageActive == "fr")
        {
            warningText = "APPUYEZ SUR TOUTE CLÉ POUR SAUTER L'INTRO";
        }
    }
}
