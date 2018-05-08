using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour {

    //grab the other game objects we need
    public WorldScroller ScrollScript;
    public EnviroPusher EnviroScript;
    
    //the speed of the world base
    public float DrivingSpeed;
    public float PassiveSpeed;
    public float RealSpeed;

    //control the road width here
    [Tooltip("our road to widen")]
    public GameObject Road;
    public float RoadPassiveWidth;
    public float RoadDriveWidth;

   

    //control the tunnel here
    public GameObject Tunnel;

    //control the sidewalk here.
    [Tooltip("everything that dissappears below the ground")]
    public GameObject[] ObjectsToLower;
    public float PassiveHeight;
    public float DriveHeight;

    public float Set2PassiveHeight;

    [Tooltip("everything that appears from the ground")]
    public GameObject[] ObjectsToRaise;

    [Tooltip("how long it takes to make the world transition to drive mode")]
    public float TransitionEffectTime;

    bool passive = false;
    

	// Use this for initialization
	void Start () {
      //  ScrollScript = GetComponent<WorldScroller>();

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void WorldPassiveResponce()
    {
        
        //change the speed of the scrolling world
        ScrollScript.Driving = false;
        RealSpeed = PassiveSpeed;

        //tell the worldscroller to change the speed of the road

        GetComponent<EnviroPusher>().PassiveActivate();

        GetComponent<PointSystem>().PassiveStart();

        GetComponent<PointSystem>().BasePushSpeed = PassiveSpeed;
        
        OurOwnPassive();
        passive = true;
        Debug.Log("WorldDecidedToBePassive");
        //turn the cars on
    }


    void OurOwnPassive()
    {
        if (!passive)
        {
            StartCoroutine(WidenRoad(Road, RoadDriveWidth, RoadPassiveWidth, TransitionEffectTime));
            StartCoroutine(WidenTunnel(Tunnel, 1, 0, TransitionEffectTime));
            Invoke("FullyFlattenTUnnel", TransitionEffectTime);
        }

        foreach (GameObject I in ObjectsToLower)
        {if (!passive)
            StartCoroutine(SinkSideWalks(I, DriveHeight, PassiveHeight,  TransitionEffectTime));
           // Debug.Log("start raising sides");
        }

        foreach (GameObject I in ObjectsToRaise)
        {
            if (!passive)
                StartCoroutine(RaiseSideWalks(I, DriveHeight + 0.2f, Set2PassiveHeight, TransitionEffectTime));
           // Debug.Log("start lowering outer sides");
        }

    }

    public void WorldDriveResponce()
    {

        //change the speed of the scrolling world
        ScrollScript.Driving = true;
        RealSpeed = DrivingSpeed;
        GetComponent<WorldScroller>().DrivingRoad();

        GetComponent<EnviroPusher>().DrivingResponce();

        GetComponent<PointSystem>().DrivingStart();

        GetComponent<PointSystem>().BasePushSpeed = DrivingSpeed;

        OurOwnDriveStuff();
        Debug.Log("WorldDecidedToDrive");
        //  Debug.Log("told to do our own drive changes");

        passive = false;
        //turn the cars off
    }



    void OurOwnDriveStuff()
    {
        StartCoroutine(WidenTunnel(Tunnel, 0, 1, TransitionEffectTime));
        StartCoroutine(WidenRoad(Road, RoadPassiveWidth, RoadDriveWidth, TransitionEffectTime));
   
        
      //  Debug.Log("should have started the coroutine");
        foreach(GameObject I in ObjectsToLower)
        {
          
            StartCoroutine(SinkSideWalks(I,PassiveHeight,DriveHeight,TransitionEffectTime));
        }

        foreach (GameObject I in ObjectsToRaise)
        {

            StartCoroutine(RaiseSideWalks(I, Set2PassiveHeight, DriveHeight + 0.2f, TransitionEffectTime));
        }
        
    }

    IEnumerator WidenTunnel(GameObject _Tunnel, float StartWidth, float EndWidth, float TIme)
    {
        float Timer = 0.0f;
        while (Timer <= TIme)
        {
            //  Debug.Log("should be widening road");

            _Tunnel.transform.localScale = new Vector3(_Tunnel.transform.localScale.x, Mathf.Lerp(StartWidth, EndWidth, (Timer / TIme)), Mathf.Lerp(StartWidth, EndWidth, (Timer / TIme)));

            Timer += Time.deltaTime;

            yield return null;
        }
    }

    void FullyFlattenTUnnel()
    {
        Tunnel.transform.localScale = new Vector3(Tunnel.transform.localScale.x, 0, 0);
    }

    IEnumerator WidenRoad(GameObject road, float StartWidth, float EndWidth, float TIme)
    {
        float Timer = 0.0f;
        while (Timer <= TIme)
        {
          //  Debug.Log("should be widening road");

            road.transform.localScale = new Vector3(Mathf.Lerp(StartWidth, EndWidth, (Timer / TIme)), road.transform.localScale.y, road.transform.localScale.z);

            Timer += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator SinkSideWalks(GameObject Obj, float StartY, float EndY, float TIme)
    {
        float Timer = 0.0f;

        float Yvalue = StartY;
        while (Timer <= TIme)
        {
            Yvalue = Mathf.Lerp(StartY, EndY, (Timer / TIme));

            Obj.transform.localPosition = new Vector3(Obj.transform.localPosition.x, Yvalue, Obj.transform.localPosition.z);

            Timer += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator RaiseSideWalks(GameObject Obj, float StartY, float EndY, float TIme)
    {
        float Timer = 0.0f;

        float Yvalue = StartY;
        while (Timer <= TIme)
        {
            Yvalue = Mathf.Lerp(StartY, EndY, (Timer / TIme));

            Obj.transform.localPosition = new Vector3(Obj.transform.localPosition.x, Yvalue, Obj.transform.localPosition.z);

            Timer += Time.deltaTime;

            yield return null;
        }
    }

    //IEnumerator OpenTunnel(GameObject Obj, float StartZ, float EndZ, float TIme)
    //{
    //    float Timer = 0.0f;
    //    while (Timer <= TIme)
    //    {
    //    //    Debug.Log("should be widening road");

    //        Obj.transform.localScale = new Vector3(Obj.transform.localScale.x, Obj.transform.localScale.y,Mathf.Lerp(StartZ, EndZ, (TIme / TIme)));

    //        TIme += Time.deltaTime;

    //        yield return null;
    //    }
    //}
}
