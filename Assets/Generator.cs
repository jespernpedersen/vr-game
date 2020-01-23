using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    public float generator_progress;

	// Use this for initialization
	void Start () {
		generator_progress = 0f;

		if(generator_progress != 100f) {
     		InvokeRepeating("ProgressGenerator", 0f, 1f);  //1s delay, repeat every 1s
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Speed
		
		
		if(generator_progress == 100f) {        
			Debug.Log("Complete");

		}
	}
	void ProgressGenerator() {
		if(generator_progress != 100f) {
			generator_progress++;
		}
 	}
}
