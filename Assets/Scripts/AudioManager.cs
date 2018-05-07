using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //can get this publicly but can only set privately, right now the script is on worldgod
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;
    //how long it takes to fade out the previous sound
    public float fadeTime;
    //check for if fade out enabled
    public bool doesFade;

    // Use this for initialization
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
        
        //sets all sound class vars to their respective source equal
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            //s.source.spatialBlend = s.spatialBlend;
        }
    }

    //Plays the sound... wild
    public void Play (string name)
    {
        //checks the sound array and sets s to the sound with the correct name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    //Stops the sound from playing, used mainly for looping sounds
    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Can't stop " + name + " because it doesn't exist");
            return;
        }
        //checks to see if fade is enabled, if it is fade it out vice versa...
        if (doesFade)
            StartCoroutine(FadeOut (s.source, fadeTime));
        else
            s.source.Stop();
    }

    //thing to fade the sound out
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        //sets the current vol to relative 1 so the fade is evenly distrubuted regardless of vol
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) 
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
            yield return null;
        }

        //Stops the sound and resets vol for nextime
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }

    public void Start()
    {
        //This is just chucked here for now
        Play("Theme");
    }

    /* public void PlayAtPoint (string name, Vector3 pos)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.PlayClipAtPoint(s.clip, pos);
    } */

}
