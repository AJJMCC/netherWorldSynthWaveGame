using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitchControl : MonoBehaviour {
    public static ModeSwitchControl Instance;

    public float TimeTillPassiveShift;
    float timechecker;
    bool isPassive;

    public GameObject CarGod;
    public GameObject WorldGod;
    

	// Use this for initialization
	void Start () {
        Instance = this;
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
                //Debug.Log("becomepassive");
                PassiveActivate();
                timechecker = 0;
                isPassive = true;
            }
        }
        if (Input.anyKey)
        { timechecker = 0; }
		

	}

    public void PassiveActivate()
    {
        Debug.Log("becomepassive");
        timechecker = 0;
        isPassive = true;
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
