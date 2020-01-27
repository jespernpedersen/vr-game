using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Generator : MonoBehaviour, iGazeReceiver {
    public float genProgress;
	public bool complete;
	public float genDelay;
	public float genSpeed;

	// UI
	public Slider UISlider;
	public Text statusText;
	public string RepairText;

	// Raycast
	private bool isGazingUpon;



	// Use this for initialization
	void Start () {

		// Gen Status
		genProgress = 0f;
		complete = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(genProgress == 100f) {      
			complete = true;
			UISlider.gameObject.SetActive (false);
			gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,1,1);
		}
		
		UISlider.GetComponent<Slider> ().value = genProgress;
	}
	void ProgressGenerator() {	
		if(complete != true) {
			statusText.text = RepairText.ToString();
			UISlider.gameObject.SetActive (true);
			genProgress++;
            transform.Rotate(0, 3, 0);
		}
 	}
	public void Generator_Interact() {	
		if(isGazingUpon){
			if(genProgress != 100f) {
				InvokeRepeating("ProgressGenerator", 0f, genSpeed);  //1s delay, repeat every 1s
			}
		}
		else {
			Cancel_Interact();
		}
	}
	public void Cancel_Interact() {
		Debug.Log("Canceling");
		CancelInvoke();
		UISlider.gameObject.SetActive (false);
	}

	// Raycast 
    
    public void GazingUpon() {
        isGazingUpon = true;
    }

    public void NotGazingUpon() {
        isGazingUpon = false;
    }
}
