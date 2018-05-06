using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCarControl : MonoBehaviour {

    public GameObject car;

    //position control
    public float LeftX;
    public float RightX;
    public Vector3 StartPosition;

    public float transitionTime;
    public AnimationCurve transitioncurve;
    public float ResetTime;

    //rotation control
    public float TurnAngle;
    
    public AnimationCurve RotationCurve;


    //car placement checks
    private bool IsCarRight;
    private bool Enabled = true;
   

    // Use this for initialization
    void Start()
    {
        Debug.Log("exists");
    }

    // Update is called once per frame
    void Update()
    {
        if (Enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StopAllCoroutines();
                StartCoroutine(Move(RightX, LeftX, transitionTime, 0, -TurnAngle));
            }

            if (Input.GetMouseButtonDown(1))
            {
                StopAllCoroutines();
                StartCoroutine(Move(LeftX, RightX, transitionTime, 0, TurnAngle));
            }

            if (Physics.Raycast(car.transform.localPosition, car.transform.forward, 20))
            {

                Debug.Log("should move now");
                if (!IsCarRight)
                {
                    StopAllCoroutines();
                    Debug.Log("should move right");
                    StartCoroutine(Move(LeftX, RightX, transitionTime,0, TurnAngle));
                    Invoke("CarPosRegister", transitionTime);
                }

                else if (IsCarRight)
                {

                    StopAllCoroutines();
                    Debug.Log("should move left");
                    StartCoroutine(Move(RightX, LeftX, transitionTime,0,-TurnAngle));
                    Invoke("CarPosRegister", transitionTime);
                }
            }
        }
      

    }

    void CarPosRegister()
    {
        IsCarRight = !IsCarRight;
        Debug.Log(IsCarRight);
    }

    public void Turnoff()
    {
        StopAllCoroutines();
        Enabled = false;
    }

    public void Turnon()
    {
        Enabled = true;
        StartCoroutine(Reset(StartPosition, ResetTime));
    }


    IEnumerator Reset(Vector3 PosToMoveTo, float time)
    {
        Debug.Log("ienum reset");
        float timer = 0.0f;
        while (timer <= time)
        {
            car.transform.localPosition = Vector3.Lerp(car.transform.localPosition, PosToMoveTo, transitioncurve.Evaluate(timer / time));
       
            timer += Time.deltaTime;
           
            yield return null;
        }
    }

    IEnumerator Move(float pos1, float pos2, float time, float Rot1, float Rot2)
    {
        Debug.Log("ienumetatormove");
        float timer = 0.0f;
        float XValue;

        float RotYValue;
        Vector3 V1 = new Vector3(pos1,0,0);
        Vector3 V2 = new Vector3(pos2, 0, 0);
        while (timer <= time)
        {
            XValue = Mathf.Lerp(pos1, pos2, transitioncurve.Evaluate(timer / time));
            //car.transform.localPosition = new Vector3(XValue, car.transform.position.y, car.transform.position.z);
            car.transform.localPosition = Vector3.Lerp(V1, V2, transitioncurve.Evaluate(timer / time));

            RotYValue = Mathf.Lerp(Rot1, Rot2, RotationCurve.Evaluate(timer / time));
            car.transform.localRotation = Quaternion.Euler(0, RotYValue, 0);

            timer += Time.deltaTime;
            Debug.Log("ienumetatormovebutlegit");
            yield return null;
        }
    }
}
