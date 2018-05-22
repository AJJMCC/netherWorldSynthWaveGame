using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //can get this publicly but can only set privately, right now the script is on soundgod
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;
    //how long it takes to fade out the previous sound

    public AudioSource sfxSource;

    public AudioSource passiveScore;

    public AudioSource drivingScore;
    // Use this for initialization

    public AudioClip passiveClip;

    public AudioClip passiveAltClip;

    public AudioClip drivingClip;

    public AudioClip drivingAltClip;
    public float fadeTime;
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

        passiveScore.volume = 0;
        drivingScore.volume = 0;
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
        if (s.sfx){
            sfxSource.clip = s.clip;
            sfxSource.volume = s.volume;
            sfxSource.pitch = s.pitch;
            sfxSource.loop = s.loop;

            sfxSource.Play();
        } else
        {

            if (s.passiveTheme){                
                //passiveScore.volume = s.volume;
                passiveScore.pitch = s.pitch;
                passiveScore.loop = s.loop;
                StartCoroutine(FadeIn(passiveScore, fadeTime, passiveClip, passiveAltClip));
                StartCoroutine(FadeOut(drivingScore, fadeTime));
                
            } else
            {
                //drivingScore.volume = s.volume;
                drivingScore.pitch = s.pitch;
                drivingScore.loop = s.loop;
                StartCoroutine(FadeIn(drivingScore, fadeTime, drivingClip, drivingAltClip));
                StartCoroutine(FadeOut(passiveScore, fadeTime));
                
                
            }
        }
        
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

        audioSource.volume = 0;
        audioSource.Stop();
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float fadeTime, AudioClip clip1, AudioClip clip2)
    {          
        if (audioSource.clip == clip1)
            audioSource.clip = clip2;
        else
            audioSource.clip = clip1;
        //audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 0.5f) 
        {
            audioSource.volume += 0.5f * Time.deltaTime / fadeTime;
 
            yield return null;
        }

        audioSource.volume = 0.5f;
    }

    public void Start()
    {
        //This is just chucked here for now
        Play("Theme");
    }

}
