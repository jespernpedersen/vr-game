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
		// Every Gen starts uncompleted
		complete = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(genProgress == 100f) {  
			// Gen is Complete    
			complete = true;
			// Set Complete Color
			gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,1,1);
			// Cancel Invoke
			Cancel_Interact();
		}
		// Slider = Gen Progress
		UISlider.GetComponent<Slider> ().value = genProgress;
	}
	void ProgressGenerator() {	
		if(complete != true) {
			// Set Repair Text
			statusText.text = RepairText.ToString();
			// Show Progress Bar
			UISlider.gameObject.SetActive (true);
			// Increase Progress for Gen
			genProgress++;
			// Rotate Gen
            transform.Rotate(0, 3, 0);
		}
 	}
	public void Generator_Interact() {	
		// If we're looking at object
		if(isGazingUpon){
			// If the Gen isn't complete
			if(genProgress != 100f) {
				// Repeat Generator Progress
				InvokeRepeating("ProgressGenerator", 0f, genSpeed);  //1s delay, repeat every 1s
			}
		}
	}
	public void Cancel_Interact() {
		// Cancel Invoke Repeating
		CancelInvoke();
		// Hide Progress Bar
		UISlider.gameObject.SetActive (false);
	}

	// Raycast 
    public void GazingUpon() {
        isGazingUpon = true;
		Generator_Interact();
    }
    public void NotGazingUpon() {
        isGazingUpon = false;
		Cancel_Interact();
    }
}
