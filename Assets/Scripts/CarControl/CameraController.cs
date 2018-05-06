using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject Obj;
    public float cameraSpring = 0.9f;

    public Vector3 PassiveCamPos;
    public Vector3 DriveCamPos;

    public bool DriveCar;
    private Vector3 wantedPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!DriveCar)
        {
            PassiveCamera();
        }
        else
        {
            DriveCam();
        }
        wantedPos = Obj.transform.position;
    }


    void PassiveCamera()
    {
       
        float Lerpx = Mathf.Lerp(Camera.main.transform.position.x, wantedPos.x, Time.deltaTime * cameraSpring);
        Camera.main.transform.position =  Vector3.Lerp(Camera.main.transform.position, new Vector3((PassiveCamPos.x ), PassiveCamPos.y, PassiveCamPos.z), Time.deltaTime * cameraSpring);

        Camera.main.transform.LookAt(Obj.transform.position);
    }

    void DriveCam()
    {
        float Lerpx = Mathf.Lerp(Camera.main.transform.position.x, wantedPos.x, Time.deltaTime * cameraSpring);
        Camera.main.transform.position =Vector3.Lerp(Camera.main.transform.position, new Vector3(Lerpx , DriveCamPos.y, DriveCamPos.z), Time.deltaTime * cameraSpring);

        Camera.main.transform.LookAt(Obj.transform.position);
    }
}
