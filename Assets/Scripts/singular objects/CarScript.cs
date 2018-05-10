using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour {

    bool TriggeredByCar = false;
    public float TimeTillTriggerPassive;
    public float TimeTillKillCar;

    public float DeathRotSpeed;
    public float DeathRiseSpeed;
    public Material DeathMaterial;

    public Renderer[] Parts;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (TriggeredByCar)
        {
            transform.Translate(Vector3.up * DeathRiseSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * DeathRotSpeed * Time.deltaTime);
            foreach (Renderer O in Parts)
            {
                O.material = (DeathMaterial);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car" && !TriggeredByCar)
        {
            Debug.Log("HitCar");
            AudioManager.Instance.Play("Crash");
            //Invoke("TellControlWeHitACar", TimeTillTriggerPassive);
            ModeSwitchControl.Instance.PlayerHitCar_GodTierResponce(this.gameObject, TimeTillTriggerPassive);
            EnviroPusher.Instance.PlayerhitCarWhileDriving(this.gameObject, TimeTillKillCar);
            TriggeredByCar = true;
          
        }
    }

    //void TellControlWeHitACar()
    //{
    //    ModeSwitchControl.Instance.PassiveActivate();
    //}
}
