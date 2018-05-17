using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaktion
{
    public class PersonalReaktorControl : MonoBehaviour
    {
        public static PersonalReaktorControl Instance;
        public GameObject Camera1;
        //glitches that happen in relation to outside sounds. 
        public ReaktorLink reaktor;
        public Modifier intensity;
        // public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        //tint control
        public float MinTintCol;
        public float MaxTintCol;
        public float BaseTintColour;
        public float TintAudioMultiplier;

        //materials to change
        public Material SolidPalm;
        public float MinSolidPalm;
        public float MaxSolidPalm;
        public float SolidPalmAudioMultiplier;

        public Material PalmWireFrame1;
        public Gradient WirecolorGradient;
        public Color StartCol;
        public Color EndCol;
       


        public AnimationCurve Materialcurve = AnimationCurve.Linear(0, 0, 1, 1);






        // Use this for initialization
        void Start()
        {
            reaktor.Initialize(this);
            Instance = this;

         
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTintOnReactor(reaktor.Output, TintAudioMultiplier, BaseTintColour);
            //Debug.Log(reaktor.Output);
            UpdateMaterialEmissions(reaktor.Output, SolidPalmAudioMultiplier);
           
        }

        void UpdateTintOnReactor(float Param1, float audiomultiplier, float adder)
        {
            float TinTColour = ((Param1 * audiomultiplier) + adder);
           // Debug.Log(TinTColour);
            float CmapledTint = Mathf.Clamp(TinTColour, MinTintCol, MaxTintCol);
            //float TinTColour = Mathf.Clamp(((Param1 * audiomultiplier) + adder),MinTintCol, MaxTintCol);
            Camera1.GetComponent<ShaderEffect_Tint>().y = CmapledTint;

            Camera1.GetComponent<ShaderEffect_Tint>().u = CmapledTint;

            Camera1.GetComponent<ShaderEffect_Tint>().v = CmapledTint;
         
        }

        void UpdateMaterialEmissions(float Param1, float audiomultiplier)
        {
            float SolidPalmIntensity = (Param1 * audiomultiplier) ;
            float CmapledSolid = Mathf.Clamp(SolidPalmIntensity, MinSolidPalm, MaxSolidPalm);
            Color SolidMat = new Color(255,143,0, Materialcurve.Evaluate(CmapledSolid));
          //  Debug.Log(SolidPalmIntensity);
            SolidPalm.SetColor("_TintColor", SolidMat);


            float WirteFrameIntensity = (Param1 * 0.8f) ;
            PalmWireFrame1.SetColor("_EmissionColor", WirecolorGradient.Evaluate((WirteFrameIntensity)));
            PalmWireFrame1.SetColor("_Color", WirecolorGradient.Evaluate((WirteFrameIntensity)));
            //Debug.Log()
        }


        

        
    }
}
