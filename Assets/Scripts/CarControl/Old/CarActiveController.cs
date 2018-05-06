using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarActiveController : MonoBehaviour {

    public GameObject car;
    public float MaxXValue;
    public float SpeedVariable;


    public float Turnvariable;



	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RotateToMatch();
        if (Input.GetAxis("Horizontal") != 0)
        {
            Turn();
            Debug.Log("Shouldturn");
        }
    }

    void Turn()
    {
        this.GetComponent<ModeSwitchControl>().PlayerDriving();
        float X;
       
            X = Input.GetAxis("Horizontal") * SpeedVariable;
            car.transform.localPosition += new Vector3(X, car.transform.localPosition.y, car.transform.localPosition.z);
            car.transform.localPosition = new Vector3(Mathf.Clamp(car.transform.localPosition.x,- MaxXValue,MaxXValue), car.transform.localPosition.y, car.transform.localPosition.z);

        
    }

    void RotateToMatch()
    {
        float Yrot;
        Yrot = Input.GetAxis("Horizontal") * Turnvariable;
        Vector3 rotation = new Vector3(0, Yrot, 0);
        car.transform.localRotation = Quaternion.Euler(rotation);
       // car.transform.localRotation = Quaternion.Lerp(car.transform.localRotation, Quaternion.Euler(rotation), Time.deltaTime);
    }

    



}
