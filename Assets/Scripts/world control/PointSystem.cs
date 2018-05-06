using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointSystem : MonoBehaviour {
    public static PointSystem Instance;
    public float BasePushSpeed;
    public float KillZPos;

    //lets spawn some cars
    public GameObject[] PointSpawnPositions;
    public GameObject[] PointPrefabs;
    public float MaxPointTimer;
    public float MinPointTimer;
    private float RealPointTime;
   
    public float PointMovepointExponent;
    public List<GameObject> CurrentPoints;

    private bool Driving;

	// Use this for initialization
	void Start () {
        Instance = this;
        CurrentPoints = new List<GameObject>();

    }

    public void DrivingStart()
    {
        Driving = true;

    }

    public void PassiveStart()
    {



        Driving = false;
        for (int I = 0; I < CurrentPoints.Count; I++)
        {
            Destroy(CurrentPoints[I]);
            //  CurrentPoints.Remove(CurrentPoints[I]); ;
        }
        CurrentPoints = new List<GameObject>();
    }


    // Update is called once per frame
    void Update ()
    {
		if (Driving)
        {
            MovePoints();
            MtSpawnTimer();
          
        }
	}
    void MovePoints()
    {

        //foreach (GameObject _I in CurrentPoints)
        //{
        //    _I.transform.Translate(-Vector3.forward * (BasePushSpeed * PointMovepointExponent) * Time.deltaTime);
        //}


        for (int I = 0; I < CurrentPoints.Count; I++)
        {
            if (CurrentPoints[I].transform.position.z <= KillZPos)
            {
                CurrentPoints.Remove(CurrentPoints[I]);
                Destroy(CurrentPoints[I], 2);


            }
        }
    }

    void MtSpawnTimer()
    {
        RealPointTime -= Time.deltaTime;

        if (RealPointTime <= 0)
        {
            SpawnSingular(PointPrefabs, PointSpawnPositions,CurrentPoints);

            RealPointTime = Random.Range(MinPointTimer, MaxPointTimer);
            Debug.Log("SpawnPoints");
        }
    }


    void SpawnSingular(GameObject[] Prefabs, GameObject[] places, List<GameObject> EndList)
    {
        GameObject Prefab = Prefabs[Random.Range(0, Prefabs.Length)];
        GameObject NrePrefab = GameObject.Instantiate(Prefab, places[Random.Range(0, places.Length)].transform.position, Quaternion.identity);

        EndList.Add(NrePrefab);
    }

   
    public void PointHitKillzone(GameObject PointHit)
    {
        CurrentPoints.Remove(PointHit);
        Destroy(PointHit);
    }
    public void PlayerCollectedPoint(GameObject POintHit)
    {
        CurrentPoints.Remove(POintHit);
        Destroy(POintHit);
      
      
       
    }
}
