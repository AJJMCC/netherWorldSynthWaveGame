using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointScript : MonoBehaviour {

    public float RotSpeed;
    public GameObject Child;
    public GameObject[] MeshObjects;

    public float KillPos;

    float MoveSpeed;


	// Use this for initialization
	void Start ()
    {
        MoveSpeed = PointSystem.Instance.BasePushSpeed * PointSystem.Instance.PointMovepointExponent;

    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(-Vector3.forward * (MoveSpeed) * Time.deltaTime);
        Rotate();

        if (transform.position.z <= KillPos)
        {
          PointSystem.Instance.PointHitKillzone(this.gameObject);
            Debug.Log("world killed Point");
        }

    }

    void Rotate()
    {
        Child.transform.Rotate(Vector3.up * RotSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
      if (other.tag == "Car")
        {
          //  Debug.Log("Told point to die");
            PointSystem.Instance.PlayerCollectedPoint(this.gameObject);
            //ExploderSingleton.Instance.ExplodeObject(Child);
           // Debug.Log("exploded point");
        //    Destroy(this.gameObject);
         
          //  Debug.Log("Told System we should die");
        }
    }

    

}
