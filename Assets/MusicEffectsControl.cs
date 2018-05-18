using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicEffectsControl : MonoBehaviour {
    public static MusicEffectsControl instance = null;


    private AudioSource _audio;
    public AudioSource DriveAudio;
    public AudioSource PassiveAudio;


    int qSamples = 1024;  // array size
    float refValue = 0.1f; // RMS value for 0 dB
    float rmsValue ;   // sound level - RMS
    float dbValue ;    // sound level - dB
    float volume  = 2; // set how much the scale will vary

    private float[] samples; // audio samples


    public Material SideWalksDriven;
    public float SideWalkMultiplier;
    public float SideWalksMinEmission;
    public float SideWalksMaxEmission;

    public Material CarMat;
    public float CarMultiplier;
    public Gradient CarcolorGradient;


    public Material PointMat;
    public float PointMultiplier;
    public float PointMinEmission;
    public float PointMaxEmission;

    public Material SolidPalm;
    public float MinSolidPalm;
    public float MaxSolidPalm;
    public float SolidPalmAudioMultiplier;





    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        samples = new float[qSamples];

    }

    public void StartDriving()
    {
        _audio = DriveAudio;
    }

    public void StartPassive()
    {
        _audio = PassiveAudio;
    }


    void Update()
    {
        GetVolume();
        UpdateColor();
    }


   void GetVolume()
    {
        _audio.GetOutputData(samples, 0); // fill array with samples
        int i ;
        float sum  = 0;
        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min
    }

    private void UpdateColor()
    {
        CarControl();

        UpdateSideWalkEmission();

        PointMatColour();

        TreeSolidColour();


       
    }

    void UpdateSideWalkEmission()
    {
        float ChangeValue = rmsValue * SideWalkMultiplier;
        
        float ClampedValue = Mathf.Clamp(ChangeValue, SideWalksMinEmission, SideWalksMaxEmission);

        SideWalksDriven.SetFloat("_EmissionGain", ClampedValue);
    }

    void CarControl()
    {
        float ChangeValue1 = rmsValue * CarMultiplier;

        //float ClampedValue1 = Mathf.Clamp(ChangeValue1, CarMinEmission, CarMaxEmission);
        CarMat.SetColor("_EmissionColor", CarcolorGradient.Evaluate((ChangeValue1)));

    }

    void PointMatColour()
    {
        float ChangeValue2 = rmsValue * PointMultiplier;
       // Debug.Log(ChangeValue2);
        float ClampedValue2 = Mathf.Clamp(ChangeValue2, PointMinEmission, PointMaxEmission);

       // FloorMat.SetFloat("_EmissionGain", ClampedValue2);
       
    }

    void TreeSolidColour()
    {
        float SolidPalmIntensity = (rmsValue * SolidPalmAudioMultiplier);
        float CmapledSolid = Mathf.Clamp(SolidPalmIntensity, MinSolidPalm, MaxSolidPalm);
        Color SolidMat = new Color(255, 143, 0, CmapledSolid);
        //  Debug.Log(SolidPalmIntensity);
        SolidPalm.SetColor("_TintColor", SolidMat);
    }
}
