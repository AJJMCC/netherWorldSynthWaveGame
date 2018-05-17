using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reaktion
{
    public class EffectsGod : MonoBehaviour
    {
        public static EffectsGod Instance;

        public GameObject Camera1;

        //glitches that happen when you fail
        public float TimeForFailGlitches;
        //colour bleed
        public float MinBleed;
        public float MaxBleed;
        public AnimationCurve ColourBleedControl1;
        //VRAM control
        public float MinVRAM;
        public float MaxVRAM;

        //glitches that happen when you start driving

        public AnimationCurve PointPickupAnimCurve;
        public float TimeForPointPickupGlitches;


        public float TimeForDriveGlitches;
        // Use this for initialization
        void Start()
        {
           
            Instance = this;

        }

        // Update is called once per frame
        void Update()
        {
            
        }


      



        public void FailGlitches()
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(35, 35, 0, 1);
            StartCoroutine(FailEffects(TimeForFailGlitches));
            Debug.Log("should have started fail glitches");
        }

        public void StartedcDrivingGlitches()
        {
           // StartCoroutine(StartDrivingEffects(TimeForDriveGlitches));
        }

        public void NaturalPassiveGlitches()
        {
           // StartCoroutine(StartPassiveEffects(TimeForDriveGlitches));
        }

        public void PointPickupGlitches()
        {
            EZCameraShake.CameraShaker.Instance.ShakeOnce(5, 5, 0, 0.4f);
            //StartCoroutine(PointPickupEffects(TimeForPointPickupGlitches));
        }

        //IEnumerator PointPickupEffects(float time)
        //{
        //    //Debug.Log("car reset");
        //    float timer = 0.0f;
        //    while (timer <= time)
        //    {

        //        float TintY = (Mathf.Lerp(Drive_Y, PointPickup_Y, PointPickupAnimCurve.Evaluate(timer / time)));
        //        Camera1.GetComponent<ShaderEffect_Tint>().y = TintY;



        //        timer += Time.deltaTime;

        //        yield return null;
        //    }
        //}

        //reset when passive starts


        IEnumerator FailEffects(float time)
        {
            //Debug.Log("car reset");
            float timer = 0.0f;
            while (timer <= time)
            {

                float BleedEffect = (Mathf.Lerp(MinBleed, MaxBleed, ColourBleedControl1.Evaluate(timer / time)));
                Camera1.GetComponent<ShaderEffect_BleedingColors>().intensity = BleedEffect;

                float VRAMEffect = (Mathf.Lerp(MinVRAM, MaxVRAM, ColourBleedControl1.Evaluate(timer / time)));
                Camera1.GetComponent<ShaderEffect_CorruptedVram>().shift = VRAMEffect;

                timer += Time.deltaTime;

                yield return null;
            }
        }

       
        IEnumerator StartDrivingEffects(float time)
        {
            //Debug.Log("car reset");
            float timer = 0.0f;
            while (timer <= time)
            {

                //float TintY = (Mathf.Lerp(Passive_Y, Drive_Y, (timer / time)));
                //Camera1.GetComponent<ShaderEffect_Tint>().y = TintY;
                // float TVSide = (Mathf.Lerp(PassiveRadial, DriveRadial, (timer / time)));
                // Camera1.GetComponent<RetroAesthetics.RetroCameraEffect>().radialIntensity = TVSide;


                timer += Time.deltaTime;

                yield return null;
            }
        }

        //reset when passive starts
        IEnumerator StartPassiveEffects(float time)
        {
            //Debug.Log("car reset");
            float timer = 0.0f;
            while (timer <= time)
            {

                //float TintY = (Mathf.Lerp(Drive_Y, Passive_Y, (timer / time)));
                //Camera1.GetComponent<ShaderEffect_Tint>().y = TintY;
                // float TVSide = (Mathf.Lerp(DriveRadial, PassiveRadial, (timer / time)));
                // Camera1.GetComponent<RetroAesthetics.RetroCameraEffect>().radialIntensity = TVSide;

                timer += Time.deltaTime;

                yield return null;
            }
        }

    }
}
