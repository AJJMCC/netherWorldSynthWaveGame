using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScroller : MonoBehaviour {

    public bool Driving = false;
    public float scrollSpeed;

    public GameObject Road;
    public float RoadScrollExponent = 0.5F;

    public GameObject Ground;
    public float GroundScrollExponent = 0.3f;




  
    void Start()
    {
       
    }

    void Update()
    {
        

        Movement();
    }
    void Movement()
    {
        //lets make the road scroll
        float offset = Time.time * (scrollSpeed * RoadScrollExponent);
        Road.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, -offset));

        //lets make the ground scroll
        float offsetG = Time.time * (scrollSpeed * GroundScrollExponent);
        Ground.GetComponent<Renderer>().material.SetTextureOffset("_GridTex", new Vector2(0, -offsetG));
    }

    public void PassiveRoad()
    {
        scrollSpeed = GetComponent<WorldControl>().RealSpeed;
    }

    public void DrivingRoad()
    {
        scrollSpeed = GetComponent<WorldControl>().RealSpeed;
    }

    
}
