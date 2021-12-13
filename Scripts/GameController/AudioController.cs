using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioController : MonoBehaviour {

    public AudioClip audio1 = null;
    public AudioClip audio2 = null;
    public AudioClip combat1 = null;
    public AudioClip combat2 = null;
    private AudioSource audio;

	// Use this for initialization
	void Start ()
    {
        audio = GetComponent<AudioSource>();

        if (combat1 == null)
        {
            combat1 = (Resources.Load("Audio/Boss LOOP")) as AudioClip;

        }
        if (audio1 == null)
        {
            audio1 = (Resources.Load("Audio/Scary Dungeon LOOP")) as AudioClip;
        }

        audio.clip = audio1;
        audio.Play();
    }

    public void ChangeToBattle (string combatAudio)
    {
    //    Debug.Log(combatAudio);
        if (combatAudio == "combat1")
        {
            audio.clip = combat1;
            audio.Play();
        }
        else
        {
            combat1 = (Resources.Load("Audio/" + combatAudio)) as AudioClip;
            audio.clip = combat1;
            audio.Play();
        }


    }

    public void ChangeToPeace (string peaceAudio)
    {
        if (peaceAudio == "audio1")
        {
            audio.clip = audio1;
            audio.Play();
        }
    }



}
