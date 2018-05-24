using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnviroPusher : MonoBehaviour {
    public static EnviroPusher Instance;

    //this is the speed we change 
    private float BasePushSpeed;
    private float BuildingsAreAnnoyingSpeed;

    [Tooltip("this is where the things will die if they cross it")]
    public float KillZPos;

    private bool Driving;

    //lets spawn some cars
    [Tooltip("this is the list of possible spawn positions")]
    public GameObject[] CarSpawnPositions;
    [Tooltip("this is a list of the prefabs to spawn")]
    public GameObject[] CarPrefabs;
    [Tooltip("the maximum time between spawns")]
    public float MaxCarTimer;
    public float DriveMaxCarTimer;
    public float PassiveMaxCarTimer;
    [Tooltip("this minimin time between spawns")]
    public float MinCarTimer;
    public float DriveMinCarTimer;
    public float PassiveMinCarTimer;
    private float RealCarTime;
    private List<GameObject> CurrentCars;
    [Tooltip("How fast the cars move in relation to the base scroll speed")]
    public float CarMoveSpeedExponent;
    [Tooltip("How much car speed changes between passive and driving")]
    public float CarSpeedDriveDifference;
    public float CarSpeedPassiveDifference;

    //lets spawn some trees
    public GameObject[] TreeSpawnPositions;
    public GameObject[] TreePrefabs;
    public GameObject[] DrivingTreePrefabs;
    public GameObject[] DrivingTreeSpawnPositions;
    public float TimeBetweenTrees;
    private float RealTreeTime;
    private List<GameObject> Trees;
    public float TreeMoveSpeedExponent;
    public float TreeScaleUpTime;

    //lets Spawn some Mountains
    public GameObject[] MtSpawnPositions;
    public GameObject[] MtPrefabs;
    public float MaxMtTimer;
    public float MinMtTimer;
    private float RealMtTime;
    private List<GameObject> Mountains;
    public float MtMoveSpeedExponent;
    public float MtScaleUpTime;

    //lets Spawn some Buildings
    public GameObject[] BuildSpawnPositions;
    public GameObject[] BuildPrefabs;
    public float MaxBuildTimer;
    public float MinBuildTimer;
    private float RealBuildTime;
    private List<GameObject> Buildings;
    public float BuildMoveSpeedExponent;
    public float BuildScaleUpTime;

    //lets spawn some jets
    public GameObject[] JetSpawnPositions;
    public GameObject[] JetPrefabs;
    public float MaxJetTimer;
    public float MinJetTimer;
    private float RealJetTime;
    private List<GameObject> Jets;
    public float jetMoveSpeedExponent;
    public float JetRotateSpeed;


    [Tooltip("The animcurve that controls things scalingup")]
    public AnimationCurve ScaleUpCurve;

    [Tooltip("The Road")]
    public GameObject Road;
    [Tooltip("How fast the road scrolls in comparison.")]
    public float RoadScrollExponent = 0.5F;

    [Tooltip("The Ground")]
    public GameObject Ground;
    [Tooltip("How fast the Ground scrolls in comparison.")]
    public float GroundScrollExponent = 0.3f;

    [Tooltip("How fast the Tunnel scrolls in comparison.")]
    public float TunnelScrollExponent = 0.6f;

    [Tooltip("The wireframe to move when the tunnel is active")]
    public Renderer tunnelWireframe;

  

    // Use this for initialization
    void Start () {
        //BasePushSpeed = GetComponent<WorldControl>().RealSpeed;
        Instance = this;
        //this just instantiates all of our lists
        Trees = new List<GameObject>();
        Mountains = new List<GameObject>();
        CurrentCars = new List<GameObject>();
        Buildings = new List<GameObject>();
        Jets = new List<GameObject>();

        //start the proper spawning processes for everything
        RealCarTime = Random.Range(MinCarTimer, MaxCarTimer);
        RealMtTime = Random.Range(MinMtTimer, MaxMtTimer);
        RealTreeTime = TimeBetweenTrees;
        RealBuildTime = Random.Range(MinBuildTimer, MaxBuildTimer);
        RealJetTime = Random.Range(MinJetTimer, MaxJetTimer);
        BuildingsAreAnnoyingSpeed  = GetComponent< WorldControl > ().PassiveSpeed;
    }



    public void PassiveActivate()
    {
        BasePushSpeed = GetComponent<WorldControl>().RealSpeed;
       
        CarMoveSpeedExponent = CarSpeedPassiveDifference;
        MaxCarTimer = PassiveMaxCarTimer;
        MinCarTimer = PassiveMinCarTimer;
        Driving = false;
    }

    public void DrivingResponce()
    {
        BasePushSpeed = GetComponent<WorldControl>().RealSpeed;
        CarMoveSpeedExponent = CarSpeedDriveDifference;
        MaxCarTimer = DriveMaxCarTimer;
        MinCarTimer = DriveMinCarTimer;
        Driving = true;
    }


    // Update is called once per frame
    void Update ()
    {
        //if we are driving, then spawn trees
      if (!Driving)
        {
            
        }
        TreeSpawnTimer();

        //spawn timers
        CarSpawnTimer();
        MtSpawnTimer();
        BuildSpawnTimer();
        JetSpawnTimer();
        MoveOurObjects();

        RoadGroundMovement();


        //check when we kill things
        KillChecker();
   

    }

    void RoadGroundMovement()
    {
        //lets make the road scroll
        float offset = Time.time * (BasePushSpeed * RoadScrollExponent);
        Road.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -offset));

        //lets make the ground scroll
        float offsetG = Time.time * (BasePushSpeed * GroundScrollExponent);
        Ground.GetComponent<Renderer>().material.SetTextureOffset("_GridTex", new Vector2(0, -offsetG));
            
        float offsetT = Time.time * (BasePushSpeed * TunnelScrollExponent);
        tunnelWireframe.material.SetTextureOffset("_GridTex", new Vector2(0, -offsetT));
       // Debug.Log("Should be scrolling tunnel");
    }

    //this move all objects that are part of the world
    void MoveOurObjects()
    {
        foreach( GameObject I in Trees)
        {
            I.transform.Translate(-Vector3.forward *(BasePushSpeed* TreeMoveSpeedExponent) * Time.deltaTime);
        }

        foreach (GameObject I in Mountains)
        {
            I.transform.Translate(-Vector3.forward * (BasePushSpeed * MtMoveSpeedExponent) * Time.deltaTime);
        }

        foreach (GameObject I in CurrentCars)
        {
            I.transform.Translate(-Vector3.forward * (BasePushSpeed * CarMoveSpeedExponent) * Time.deltaTime);
        }

        foreach (GameObject I in Buildings)
        {
            I.transform.Translate(-Vector3.forward * (BuildingsAreAnnoyingSpeed * BuildMoveSpeedExponent) * Time.deltaTime);
        }

        foreach (GameObject I in Jets)
        {
            I.transform.Translate(-Vector3.forward * (BasePushSpeed * jetMoveSpeedExponent) * Time.deltaTime);
            I.transform.Rotate(Vector3.forward * JetRotateSpeed);
        }
    }

    //this spawns cars
    void CarSpawnTimer()
    {
        RealCarTime -= Time.deltaTime;
        if (RealCarTime <= 0)
        {
            SpawnSingular(CarPrefabs, CarSpawnPositions, CurrentCars,false,1,1,false,0);
            RealCarTime = Random.Range(MinCarTimer, MaxCarTimer);
        }
    }

    //this spawns trees
    void TreeSpawnTimer()
    {
        RealTreeTime -= Time.deltaTime;
        if (RealTreeTime <= 0)
        {
            if (Driving)
            {
                SpawnInEverySpawnPoint(DrivingTreePrefabs, DrivingTreeSpawnPositions, Trees, true, 0.5f, 1);
                RealTreeTime = TimeBetweenTrees / 2;
            }
            else
            {
                SpawnInEverySpawnPoint(TreePrefabs, TreeSpawnPositions, Trees, true, 1.5f, 1);
                RealTreeTime = TimeBetweenTrees;
            }
        }
       
    }

    //this spawns mountains
    void MtSpawnTimer()
    {
        RealMtTime -= Time.deltaTime;
       
        if (RealMtTime <= 0)
        {
            SpawnSingular(MtPrefabs, MtSpawnPositions, Mountains,true,1,0.8f,true,MtScaleUpTime);

            RealMtTime = Random.Range(MinMtTimer, MaxMtTimer);
        }
    }

    //this spawns buildings
    void BuildSpawnTimer()
    {
        RealBuildTime -= Time.deltaTime;
        //Debug.Log(RealBuildTime);
        if (RealBuildTime <= 0)
        {
            SpawnSingular(BuildPrefabs, BuildSpawnPositions, Buildings,true, 0.5f, 0.1f,true, BuildScaleUpTime);
            RealBuildTime = Random.Range(MinBuildTimer, MaxBuildTimer);
        }
    }

    //this spawns jets
    void JetSpawnTimer()
    {
        RealJetTime -= Time.deltaTime;
       // Debug.Log("JetSTimerChecking");
        //Debug.Log(RealBuildTime);
        if (RealJetTime <= 0)
        {
            SpawnSingular(JetPrefabs, JetSpawnPositions, Jets,false,1,1,false,0);
            RealJetTime = Random.Range(MinJetTimer, MaxJetTimer);
         //   Debug.Log("JetSpawned");
        }
    }

    //this takes a list of prefabs and locations, pick a random prefab and location, and tells it to exist, start scrolling up, and add itself to a lsit of those prefabs
    void SpawnSingular(GameObject[] Prefabs, GameObject[] places, List<GameObject> EndList, bool Scale, float Max, float Min, bool SizeUp, float SizeUpTime)
    {
        GameObject Prefab = Prefabs[Random.Range(0, Prefabs.Length)];
        GameObject PrefabClone = GameObject.Instantiate(Prefab, places[Random.Range(0, places.Length)].transform.position, Quaternion.identity);
        if (Scale)
        {
            float bepis = Random.Range(Min, Max);
            PrefabClone.transform.localScale = new Vector3(bepis, bepis, bepis);
            if (SizeUp)
            {
                StartCoroutine(ScaleUp(PrefabClone, bepis, SizeUpTime));
            }

        }
        EndList.Add(PrefabClone);
    }

    //this takes a random prefab from a list and spawns it in every possible building location
    void SpawnInEverySpawnPoint(GameObject[] Prefabs, GameObject[] places, List<GameObject> EndList, bool SizeUp, float SizeUpTime, float finalScale)
    {
        GameObject Prefab = Prefabs[Random.Range(0, Prefabs.Length)];

        foreach (GameObject I in places)
        {
            GameObject PrefabClone = GameObject.Instantiate(Prefab, I.transform.position, Quaternion.identity);
            if (SizeUp)
            {
                StartCoroutine(ScaleUp(PrefabClone,finalScale,SizeUpTime));
            }
            EndList.Add(PrefabClone);
        }
    }

    //if any of our objects are past a certain point in the world, they die
    void KillChecker()
    {
        for (int I = 0; I < Jets.Count; I++)
        {
            if (Jets[I].transform.position.z <= KillZPos)
            {
                StartCoroutine(ScaleDown(Jets[I], 2));
                Destroy(Jets[I], 2);
                Jets.Remove(Jets[I]);
               // Debug.Log("KilledJet");
            }
        }

        for (int I = 0; I < Trees.Count; I++)
        {
            if (Trees[I].transform.position.z <= KillZPos)
            {
                StartCoroutine(ScaleDown(Trees[I], 2));
                Destroy(Trees[I], 2);
                Trees.Remove(Trees[I]);
               // Debug.Log("KilledTree");
            }
        }

        for (int I = 0; I < CurrentCars.Count; I++)
        {
            if (CurrentCars[I].transform.position.z <= KillZPos)
            {
                StartCoroutine(ScaleDown(CurrentCars[I], 2));
                Destroy(CurrentCars[I], 2);
                CurrentCars.Remove(CurrentCars[I]);

            }
        }


        for (int I = 0; I < Mountains.Count; I++)
        {
            if (Mountains[I].transform.position.z <= KillZPos)
            {
                StartCoroutine(ScaleDown(Mountains[I], 7));
                Destroy(Mountains[I], 7);
                Mountains.Remove(Mountains[I]);
               // Debug.Log("KilledCar");
            }
        }

        for (int I = 0; I < Buildings.Count; I++)
        {
            if (Buildings[I].transform.position.z <= KillZPos)
            {
                StartCoroutine(ScaleDown(Buildings[I], 7));
                Destroy(Buildings[I], 7);
                Buildings.Remove(Buildings[I]);
                //Debug.Log("KilledCar");
            }
        }
    }

   IEnumerator ScaleDown(GameObject Obj, float ScaleTime)
    {
        float Timer = 0.0f;

       
         while (Timer <= ScaleTime)
            {
            if (Obj != null)
            Obj.transform.localScale = Vector3.Lerp(Obj.transform.localScale, new Vector3(0,0,0), (Timer / ScaleTime));

            Timer += Time.deltaTime;

            yield return null;
            }
    }

   IEnumerator ScaleUp(GameObject Obj, float EndScale, float ScaleTime )
    {
        float Timer = 0.0f;

        while (Timer <= ScaleTime)
        {
            //Debug.Log(EndScale);
            Obj.transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0),new Vector3(EndScale,EndScale,EndScale), ScaleUpCurve.Evaluate(Timer / ScaleTime));
          
            Timer += Time.deltaTime;

            yield return null;
        }
    }

  

    public void PlayerhitCarWhileDriving(GameObject CarHit,float TimeTOWait)
    {
       // Debug.Log("apparently we hit a car");
        CurrentCars.Remove(CarHit);
        Destroy(CarHit, TimeTOWait);
    }

   

  
}
