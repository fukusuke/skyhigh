using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float resizeRate = Mathf.Sin(Time.time * 2.0f) / 10f;
		transform.localScale =  new Vector3(resizeRate + 1.0f, resizeRate + 1.0f, 1); 
	}
}
