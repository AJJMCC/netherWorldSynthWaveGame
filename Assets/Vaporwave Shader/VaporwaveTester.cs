using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporwaveTester : MonoBehaviour 
{
	/* Variables */
	public Material material;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		material.SetFloat( "_SplitHeight", Time.time % 1 );
	}
}
