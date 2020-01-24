using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {
    public float gen_progress;
	public bool complete;
	public Slider UISlider;
	public float progress_bar;
	public float gen_speed;


	// Use this for initialization
	void Start () {
		gen_progress = 0f;
		complete = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(gen_progress == 100f) {      
			complete = true;
			gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,1,1);
		}

		
        if (Input.GetMouseButtonDown(0)) {
			if(gen_progress != 100f) {
				InvokeRepeating("ProgressGenerator", 0f, gen_speed);  //1s delay, repeat every 1s
			}
		}

		
        if (Input.GetMouseButtonUp(0)) {
			
            CancelInvoke();
		}
		
		UISlider.GetComponent<Slider> ().value = gen_progress;
	}
	void ProgressGenerator() {	
		if(complete != true) {
			gen_progress++;
		}
 	}
}
