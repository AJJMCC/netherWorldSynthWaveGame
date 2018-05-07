using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitchControl : MonoBehaviour {

    public float TimeTillPassiveShift;
    float timechecker;
    bool isPassive;

    public GameObject CarGod;
    public GameObject WorldGod;
    

	// Use this for initialization
	void Start () {
        PassiveActivate();

    }
	
	// Update is called once per frame
	void Update ()
    {
        //this controls our return to passive!
        if (!isPassive && !Input.anyKey)
        {
            timechecker += Time.deltaTime;

            if (timechecker >= TimeTillPassiveShift)
            {
                Debug.Log("becomepassive");
                PassiveActivate();
                timechecker = 0;
                isPassive = true;
            }
        }
        if (Input.anyKey)
        { timechecker = 0; }
		

	}

    void PassiveActivate()
    {
        
        //once we hit passive we run this function
        CarGod.GetComponent<OverallCarControl>().Turnon();
        CarGod.GetComponent<CameraController>().DriveCar = false;

        //This was me calling the sounds from the audiomanager
        AudioManager.Instance.Play("Theme");
        AudioManager.Instance.Stop("Driving theme");

        WorldGod.GetComponent<WorldControl>().WorldPassiveResponce();
        isPassive = true;

    }

    public void PlayerDriving()
    {
        if (isPassive)
        {
            //when the player starts driving this function activates (controlled by the car)
         
            CarGod.GetComponent<CameraController>().DriveCar = true;

            WorldGod.GetComponent<WorldControl>().WorldDriveResponce();

            //This was me calling the sounds from the audiomanager
            AudioManager.Instance.Stop("Theme");
            AudioManager.Instance.Play("Driving theme");
            isPassive = false;
        }
      
       
    }
}
