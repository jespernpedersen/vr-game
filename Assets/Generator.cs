using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    public float progress;
	public bool complete;

	// Use this for initialization
	void Start () {
		progress = 0f;
		complete = false;
		

		if(progress != 100f) {
     		InvokeRepeating("ProgressGenerator", 0f, 0.1f);  //1s delay, repeat every 1s
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(progress == 100f) {      
			complete = true;
		}
	}
	void ProgressGenerator() {
		if(complete != true) {
			progress++;
		}
 	}
}
