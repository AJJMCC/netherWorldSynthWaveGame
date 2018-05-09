﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    [HideInInspector]
    public AudioSource source;

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool doesFade;

    public float fadeTime;

    /* [Range(0f, 1f)]
    public float spatialBlend; */
}
