using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallCarControl : MonoBehaviour {

    [Tooltip("The car we move about")]
    public GameObject car;
    [Tooltip("the object that holds all the world scripts")]
    public GameObject World;

    // BIGCONTROL
    private bool IsCarPassive = true;

    // ACTIVE CONTROL
    [Tooltip("The max side to side the car can go when driving")]
    public float MaxXValueActiveValue;
    [Tooltip("How fast we drive broom broom")]
    public float SpeedVariable;
    [Tooltip("The max side to side the car can go when driving")]
    public float Turnvariable;


    // PASSIVE CONTROL

    //position control
    [Tooltip("The max side to side the car can go when passive")]
    public float MaxX;
    private float LeftX;
    private float RightX;
    [Tooltip("Where the car starts")]
    public Vector3 StartPosition;

    [Tooltip("How long it takes to move from one side of the road to another ")]
    public float transitionTime;
    [Tooltip("curve that controls that movement.")]
    public AnimationCurve transitioncurve;
    [Tooltip("time to reset")]
    public float ResetTime;
    private bool Movecunt;

    //rotation control
    [Tooltip("how far the car turns when passive")]
    public float TurnAngle;
    [Tooltip("the cars rotation anim control curve")]
    public AnimationCurve RotationCurve;

    //car placement checks
    private bool IsCarRight;

    public float DrivingCooldownTime;
    float DrivingCooldown;


    // Use this for initialization
    void Start()
    {
        
        Movecunt = true;
        IsCarRight = true;
        RightX = MaxX;
        LeftX = -MaxX;
    }


    //start driving
    public void Turnoff()
    {

        StopAllCoroutines();
        IsCarPassive = false;
    }

    //its passive time
    public void Turnon()
    {
        IsCarPassive = true;
        StopAllCoroutines();
        StartCoroutine(Reset(RightX, ResetTime));
        Invoke("CarMovedRight", ResetTime);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 && DrivingCooldown <= 0)
        {
            Turnoff();
            World.GetComponent<ModeSwitchControl>().PlayerDriving();
            RotateToMatch();
            Turn();
          
          //  Debug.Log("Shouldturn");
        }
        else if (IsCarPassive)
        {
            DrivingCooldown -= Time.deltaTime;
            if (Physics.Raycast(car.transform.localPosition, car.transform.forward, 20))
            {
               
               
                if (!IsCarRight && Movecunt)
                {
                    StopAllCoroutines();
                 //   Debug.Log("should move right");
                    StartCoroutine(Move(LeftX, RightX, transitionTime, 0, TurnAngle));
                    Invoke("CarMovedRight", transitionTime);
                    IsCarRight = true;
                    Movecunt = false;
                }

                else if (IsCarRight && Movecunt)
                {

                    StopAllCoroutines();
                  //  Debug.Log("should move left");
                    StartCoroutine(Move(RightX, LeftX, transitionTime, 0, -TurnAngle));
                    Invoke("CarMovedleft", transitionTime);
                    IsCarRight = false;
                    Movecunt = false;

                }
            }
        }




    }

    //DRIVE CONTROLS
    void Turn()
    {
       
        float X;

        X = Input.GetAxis("Horizontal") * SpeedVariable;
        car.transform.localPosition += new Vector3(X, car.transform.localPosition.y, car.transform.localPosition.z);
        car.transform.localPosition = new Vector3(Mathf.Clamp(car.transform.localPosition.x, -MaxXValueActiveValue, MaxXValueActiveValue), car.transform.localPosition.y, car.transform.localPosition.z);


    }
    //rotation in responce to drive controls
    void RotateToMatch()
    {
        float Yrot;
        Yrot = Input.GetAxis("Horizontal") * Turnvariable;
        Vector3 rotation = new Vector3(0, Yrot, 0);
        car.transform.localRotation = Quaternion.Euler(rotation);
        // car.transform.localRotation = Quaternion.Lerp(car.transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime);
    }



    //what position is the car on after we switch
    void CarMovedleft()
    {
      // Debug.Log(IsCarRight);
        Movecunt = true;
    }
    //what position is the car on after we switch
    void CarMovedRight()
    {
        //Debug.Log(IsCarRight);
        Movecunt = true;
    }

   
    //reset when passive starts
    IEnumerator Reset(float _RightX , float time)
    {
        //Debug.Log("car reset");
        float timer = 0.0f;
        while (timer <= time)
        {
            car.transform.localPosition = new Vector3(Mathf.Lerp(car.transform.localPosition.x, _RightX, transitioncurve.Evaluate(timer / time)),0,0);

            timer += Time.deltaTime;

            yield return null;
        }
    }
    // passive movement
    IEnumerator Move(float pos1, float pos2, float time, float Rot1, float Rot2)
    {
       // Debug.Log("ienumetatormove");
        float timer = 0.0f;
        float XValue;

        float RotYValue;
        Vector3 V1 = new Vector3(pos1, 0, 0);
        Vector3 V2 = new Vector3(pos2, 0, 0);
        while (timer <= time)
        {
            XValue = Mathf.Lerp(pos1, pos2, transitioncurve.Evaluate(timer / time));
            //car.transform.localPosition = new Vector3(XValue, car.transform.position.y, car.transform.position.z);
            car.transform.localPosition = Vector3.Lerp(V1, V2, transitioncurve.Evaluate(timer / time));

            RotYValue = Mathf.Lerp(Rot1, Rot2, RotationCurve.Evaluate(timer / time));
            car.transform.localRotation = Quaternion.Euler(0, RotYValue, 0);

            timer += Time.deltaTime;
           // Debug.Log("ienumetatormovebutlegit");
            yield return null;
        }
    }
}
