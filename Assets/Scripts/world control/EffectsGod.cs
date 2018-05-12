using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsGod : MonoBehaviour {
    public static EffectsGod Instance;

    

    public AnimationCurve GlitchControl1;


	// Use this for initialization
	void Start ()
    {
        Instance = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FailGlitches()
    {
        EZCameraShake.CameraShaker.Instance.ShakeOnce(2, 3, 0, 0.4f);

    }

    void StartedcDrivingGlitches()
    {

    }

    void NaturalPassiveGlitches()
    {

    }

    void PointPickupGlitches()
    {

    }


}
