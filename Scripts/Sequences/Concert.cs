using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concert : MonoBehaviour
{
    private bool alive = true;
    private bool fredDrikned = false;
    private float timer = 0;
    private float lerpTimer = 0;
    private float initialVolumen;
    public AudioClip song;
    private AudioClip intialAudio;
    public GameObject canola;
    private AudioSource audio;
    private Transform taPoint1;
    private Transform taPoint2;
    private Camera cam;
    private GameObject gcon;
    private GameController gc;
    private enum State
    {
        WaitForStart,
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
        Seq10b,
        Seq11,
        Seq12,
        Seq12b,
        Seq13,
        Seq14
    }
    private State state;

    private IEnumerator FSM ()
    {
        while (alive)
        {
            switch (state)
            {
                case State.WaitForStart:
                   
                    WaitForStart();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq01:
                    yield return new WaitForSeconds(0.01f);
                    Seq01();
                    break;

                case State.Seq02:
                    Seq02();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq03:
                    Seq03();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq04:
                    Seq04();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq05:
                    Seq05();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq06:
                    Seq06();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq07:
                    Seq07();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq08:
                    Seq08();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq09:
                    Seq09();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq10:
                    Seq10();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq10b:
                    Seq10b();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq11:
                    Seq11();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq12:
                    Seq12();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq12b:
                    Seq12b();
                    yield return new WaitForSeconds(0.01f);
                    break;

                case State.Seq13:
                    Seq13();
                    yield return new WaitForSeconds(0.01f);
                    break;

            }
        }
        yield return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        gcon = GameObject.FindGameObjectWithTag("GameController");
        gc = gcon.GetComponent<GameController>();
        audio = gcon.GetComponent<AudioSource>();
       
        foreach (Camera ca in Camera.allCameras )
        {
            if (ca.name == "Camera1")
            {
                cam = ca;
            }
        }

        Invoke("StartLater", 1);
       
    }


    private void StartLater ()
    {
        state = State.WaitForStart;
        StartCoroutine("FSM");
    }

    private void WaitForStart ()
    {
        if (gc.player1 != null)
        {
            Transform camTransform = transform.Find("CamPos/Start");
            transform.Find("Crowd").gameObject.SetActive(true);
            cam.transform.position = camTransform.position;
            cam.transform.rotation = camTransform.rotation;
            cam.GetComponent<MouseOrbitImproved>().enabled = false;
            transform.Find("Bonfire").gameObject.SetActive(true);
            timer = Time.realtimeSinceStartup + 5;
            Invoke("SetUpAudio", 0.3f);
            gc.StartCinematic();
            foreach (GameObject go in gc.players)
            {
                go.SetActive(false);
            }
            state = State.Seq01;
        }
    }

    private void Seq01 ()
    {
        if (Time.realtimeSinceStartup >= timer)
        {
            taPoint1 = transform.Find("CamPos/Start");
            taPoint2 = transform.Find("CamPos/1");
            state = State.Seq02;
        }
    }

    private void Seq02 ()
    {
        lerpTimer += Time.deltaTime * 0.006f;
        cam.transform.position =   Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        float distance = Vector3.Distance(cam.transform.position, taPoint2.transform.position);
        if (distance <= 0.5f)
        {
            lerpTimer = 0;
            taPoint1 = transform.Find("CamPos/1");
            taPoint2 = transform.Find("CamPos/2");
            state = State.Seq03;
        }
    }

    private void Seq03 ()
    {
        lerpTimer += Time.deltaTime * 0.007f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        float distance = Vector3.Distance(cam.transform.position, taPoint2.transform.position);
        if (distance <= 0.5f)
        {
            lerpTimer = 0;
            taPoint1 = transform.Find("CamPos/2");
            taPoint2 = transform.Find("CamPos/3");
            state = State.Seq04;
        }
    }

    private void Seq04 ()
    {
        lerpTimer += Time.deltaTime * 0.01f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);
        //     float distance = Vector3.Distance(cam.transform.position, taPoint2.transform.position);
        Debug.Log(audio.time);
        if (audio.time >= 58.0f)
        {
            lerpTimer = 0;
            taPoint1 = transform.Find("CamPos/2");
            taPoint2 = transform.Find("CamPos/3");
            canola.GetComponent<Animator>().SetTrigger("SingTrigger");
            state = State.Seq05;
        }
    }


    private void Seq05 ()
    {

        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);
        Debug.Log(audio.time);
        if (audio.time >= 72.0f)
        {
            lerpTimer = 0;
            taPoint1 = transform.Find("CamPos/3");
            taPoint2 = transform.Find("CamPos/4");
            transform.Find("Fires").gameObject.SetActive(true);
            state = State.Seq06;
        }
    }


    private void Seq06 ()
    {
        lerpTimer += Time.deltaTime * 0.03f;
        Debug.Log("Look at fires");
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time >= 82.0f)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/4");
            taPoint2 = transform.Find("CamPos/5");            
            state = State.Seq07;
            Debug.Log("Change to bonfire");
        }
    }

    private void Seq07 ()
    {
        
        if (audio.time >= 99.0f)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 10;
            taPoint1 = transform.Find("CamPos/5");
            taPoint2 = transform.Find("CamPos/6");
            state = State.Seq08;
            transform.Find("Fires").gameObject.SetActive(false);
   

        }
        
    }

    private void Seq08 ()
    {
        lerpTimer += Time.deltaTime * 0.007f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time >= 115)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/6");
            taPoint2 = transform.Find("CamPos/7");
            
            transform.Find("Fires").gameObject.SetActive(false);
            state = State.Seq09;

        }
    }

    private void Seq09 ()
    {
        Debug.Log("9");
        lerpTimer += Time.deltaTime * 0.005f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time > 122)
        {
            if (transform.Find("GreenKnight").gameObject.activeSelf == false)
            {
                transform.Find("GreenKnight").gameObject.SetActive(true);
            }
        }

        if (audio.time >= 135)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/7");
            taPoint2 = transform.Find("CamPos/8");
            state = State.Seq10;

        }




    }

    private void Seq10()
    {
        Debug.Log("10");
        lerpTimer += Time.deltaTime * 0.01f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        // Stop singing

        if (audio.time > 148)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/8");
            taPoint2 = transform.Find("CamPos/9");
            state = State.Seq10b;
        }

    }


    private void Seq10b()
    {
        Debug.Log("10b");
        lerpTimer += Time.deltaTime * 0.01f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time > 158 && fredDrikned == false)
        {
            fredDrikned = true;
            transform.Find("Crowd/Fred").gameObject.GetComponent<Animator>().SetTrigger("DrinkTrigger");
        }

        if (audio.time > 169)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/9");
            taPoint2 = transform.Find("CamPos/9b");
            state = State.Seq11;
        }

    }

    private void Seq11()
    {
        Debug.Log("10b");
        lerpTimer += Time.deltaTime * 0.003f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);
        float distance = Vector3.Distance(cam.transform.position, taPoint2.position);
        if (distance < 1)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/9b");
            taPoint2 = transform.Find("CamPos/10");
            state = State.Seq12;
        }
    }

    private void Seq12()
    {
        Debug.Log(audio.time);
        lerpTimer += Time.deltaTime * 0.003f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time > 175)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;

            state = State.Seq12b;
        }
    }

    private void Seq12b()
    {
        Debug.Log(audio.time);
        lerpTimer += Time.deltaTime * 0.004f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.time > 188)
        {
            lerpTimer = 0;
            timer = Time.timeSinceLevelLoad + 5;
            taPoint1 = transform.Find("CamPos/10");
            taPoint2 = transform.Find("CamPos/11");
            state = State.Seq13;
        }
    }

    private void Seq13()
    {
        Debug.Log(audio.volume);

        if (audio.time > 199)
        {
            audio.volume = audio.volume - (Time.deltaTime * 0.05f);
            lerpTimer += Time.deltaTime * 0.003f;
        }

        cam.transform.position = Vector3.Lerp(cam.transform.position, taPoint2.position, lerpTimer);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, taPoint2.rotation, lerpTimer);

        if (audio.volume < 0)
        {
            EndSequence();
        }
    }

    private void SetUpAudio ()
    {
        initialVolumen = audio.volume;
        intialAudio = audio.clip;
        audio.volume = 0.75f;
        audio.clip = song;
        audio.time = 25.0f;
        audio.Play();

    }


    private void EndSequence ()
    {
        transform.Find("Fires").gameObject.SetActive(false);
        audio.volume = initialVolumen;
        audio.clip = intialAudio;
        audio.Play();
        transform.Find("Crowd").gameObject.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Cross RPG");
    }


}
