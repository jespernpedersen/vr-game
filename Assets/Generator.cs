using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Generator : MonoBehaviour, iGazeReceiver {
    public float Gen_Progress;
	public bool Gen_Complete;
	public float Gen_Delay;
	public float Gen_Speed;

	// UI
	public Slider Progress_Slider;
	public Text Status_Text;
	public Text SkillCheck_Text;
	public string Repair_Text;

	// Raycast
	private bool isGazingUpon;

	// Skill Check
	public float SkillCheck_MinTime = 1f;
	public float SkillCheck_MaxTime = 5f;
    public float SkillCheck_Time = 2f;

	private float SkillCheck_Countdown;
	private bool SkillCheck_Success = false;
	private bool SkillCheck_Status = false;



	// Use this for initialization
	void Start () {
		// Gen Status
		Gen_Progress = 0f;
		// Every Gen starts uncompleted
		Gen_Complete = false;
		// Skill Check
		SkillCheck_Countdown = SkillCheck_Time;
		SkillCheck_Success = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Gen_Progress == 100f) {  
			// Gen is Complete    
			Gen_Complete = true;
			// Set Complete Color
			gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,1,1);
			// Cancel Invoke
			Cancel_Interact();
		}
		if(SkillCheck_Status) {
			SkillCheck_Timer();
		}
	}
	public void SkillCheck_Timer() {
		// Timer Begins - Initiate
        SkillCheck_Countdown -= Time.deltaTime;

		// Timer Ran Out Only if Skill Check didn't get completed
		if(SkillCheck_Countdown < 0) {
			if(SkillCheck_Success != true) {
				// Show SkillCheck Warning
				SkillCheck_Text.gameObject.SetActive(false);
				// Penalty
				SkillCheck_Penalty();
				// Reset
				SkillCheck_Countdown = SkillCheck_Time;
				SkillCheck_Status = false;
				// Repeat Skill Check Timer
				Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));

			}
		}
	}

	public void SkillCheck() {
		Debug.Log("Skill Check");
		SkillCheck_Text.gameObject.SetActive(true);
		SkillCheck_Status = true;
	}

	public void SkillCheck_Penalty() {
		CancelInvoke("Generator_Progress");
		Gen_Progress -= 15f;
		InvokeRepeating("Generator_Progress", 1f, Gen_Speed);

		// SkillCheck_Text.text.SetActive(false);
	}

	public void SkillCheck_Interact() {
		if(SkillCheck_Status) {
			Debug.Log("Pressed Skill Check");
			SkillCheck_Success = true;
			SkillCheck_Status = false;
			SkillCheck_Text.gameObject.SetActive(false);
			Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));
		}
	}

	void Generator_Progress() {	
		if(Gen_Complete != true) {
			// Set Repair Text
			Status_Text.text = Repair_Text.ToString();
			// Show Progress Bar
			Progress_Slider.gameObject.SetActive (true);
			// Increase Progress for Gen
			Gen_Progress++;
			// Rotate Gen
            transform.Rotate(0, 60, 0);
			// Slider = Gen Progress
			Progress_Slider.GetComponent<Slider> ().value = Gen_Progress;
		}
 	}
	public void Generator_Interact() {	
		// If we're looking at object
		if(isGazingUpon){
			// If the Gen isn't complete
			if(Gen_Progress != 100f) {
				// Skill Check
				Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));
				// Repeat Generator Progress
				InvokeRepeating("Generator_Progress", 0f, Gen_Speed);
			}
		}
	}
	public void Cancel_Interact() {
		if(SkillCheck_Status) {
			SkillCheck_Penalty();
		}
		else {	
			// Cancel Invoke Repeatings
			CancelInvoke();
		}
		// Hide Progress Bar
		Progress_Slider.gameObject.SetActive(false);
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
