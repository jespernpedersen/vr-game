using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Generator : MonoBehaviour {
    public float genProgress;
	public bool complete;
	public float genDelay;
	public float genSpeed;

	// UI
	public Slider UISlider;
	public Text statusText;
	public string RepairText;

	// Controller
	// a reference to the action
	public SteamVR_Action_Boolean genRepairInput;
	// a reference to the hand
	public SteamVR_Input_Sources handType;



	// Use this for initialization
	void Start () {

		// Input
		genRepairInput.AddOnStateDownListener(TriggerDown, handType);
		genRepairInput.AddOnStateUpListener(TriggerUp, handType);

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
			genProgress++;
		}
 	}
	public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
		if(genProgress != 100f) {
			statusText.text = RepairText.ToString();
			UISlider.gameObject.SetActive (true);
			InvokeRepeating("ProgressGenerator", 0f, genSpeed);  //1s delay, repeat every 1s
		}
	}  
	public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
    	CancelInvoke();
		UISlider.gameObject.SetActive (false);
	}
}
