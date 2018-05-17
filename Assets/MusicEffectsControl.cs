using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffectsControl : MonoBehaviour {
    public static MusicEffectsControl instance = null;
    // Use this for initialization

    public AudioSource _audio;
    float[] spectrum = new float[128];
    // Spectrum is multiplayed with volume. To take any values from spectrum, volume should have not zero value
    [SerializeField]
    [HideInInspector]
    private float minVolume = 0.005f;
    [SerializeField]
    private Color color = Color.white;
    public AudioClip Song;



    private float red;
    private float green;
    private float blue;
    private float emission;


    public Material SideWalksDriven;
    public Material RetroSkyBox;


    private float spectrumMultiply = 1;





    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeMusic();
        UpdateColor();
    }

    private void AnalyzeMusic()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = minVolume;
        };

        _audio.GetOutputData(spectrum, 0);
        spectrumMultiply = 1 / _audio.volume;
    }

    private void UpdateColor()
    {
        //red = 0;
        //for (int i = 1; i < 3; i++)
        //{
        //    red += spectrum[i];
        //}
        //red *= multiplyRed * spectrumMultiply;
        //if (red < 0) red = 0;
        //else if (red > 1) red = 1;


       
    }

}
