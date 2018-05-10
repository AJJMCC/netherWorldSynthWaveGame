using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointScript : MonoBehaviour {

    public float RotSpeed;
    public GameObject Child;
    public GameObject[] MeshObjects;

    public float KillPos;
    public float TimeForDeathBlip = 0.5f;

    public float ExpandRate = 1;
    bool Expand;
    bool TriggeredByCar = false;

    float MoveSpeed;


	// Use this for initialization
	void Start ()
    {
        MoveSpeed = PointSystem.Instance.BasePushSpeed * PointSystem.Instance.PointMovepointExponent;

    }
	
	// Update is called once per frame
	void Update ()
    {
        
        Rotate();

        if (transform.position.z <= KillPos)
        {
          PointSystem.Instance.PointHitKillzone(this.gameObject);
            Debug.Log("world killed Point");
        }
     

        if (Expand)
        {
            transform.localScale += new Vector3(ExpandRate, ExpandRate, ExpandRate) * Time.deltaTime;
           transform.Translate( Vector3.up * (MoveSpeed/4) * Time.deltaTime);
        }

        else transform.Translate(-Vector3.forward * (MoveSpeed) * Time.deltaTime);

    }

    void Rotate()
    {
        Child.transform.Rotate(Vector3.up * RotSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        
      if (other.tag == "Car" && !TriggeredByCar)
        {
            RotSpeed = RotSpeed * 3;
            AudioManager.Instance.Play("Points");
            PointSystem.Instance.PlayerCollectedPoint(this.gameObject, TimeForDeathBlip);
            
            Expand = true;
            TriggeredByCar = true;
          
        }
    }

    

}
