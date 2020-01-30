using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Generator : MonoBehaviour, iGazeReceiver {
    private float Gen_Progress;
	public bool Gen_Complete;
	public float Gen_Delay;
	public float Gen_Speed;

	// UI
	public Slider Progress_Slider;
	public Text Status_Text;
	public Text SkillCheck_Text;
	public string Repair_Text;
	public Text GeneratorCount;

	// Raycast
	public GameObject Player;
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

		// Skill Check, Set SkillCheck Countdown to Time for used repeatably in a loop.
		SkillCheck_Countdown = SkillCheck_Time;
		SkillCheck_Success = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(Gen_Complete != true) {
			if(Gen_Progress == 100f) {  
				CompleteGenerator();
			} // end if Gen_Progress

		} // end if Gen_Complete

		if(SkillCheck_Status) {
			SkillCheck_Timer();
		} // end if SkillCheck_Status

	}


	public void CompleteGenerator() {
		Debug.Log("Completed Generator");
		
		// Gen is Complete    
		Gen_Complete = true;
		
		// Set Complete Color
		gameObject.GetComponent<Renderer>().material.color = new Color(0.5f,1,1);
		
		// Cancel Invoke
		Cancel_Interact();

		// Decrease Generator UI Count
		GeneratorCount.GetComponent<GenCount>().CompleteGenerator();

	}  // end function CompleteGenerator()

	public void SkillCheck_Timer() {
		// Timer Begins - Initiate
        SkillCheck_Countdown -= Time.deltaTime;

		// Timer Ran Out Only if Skill Check didn't get completed
		if(SkillCheck_Countdown < 0) {
			if(SkillCheck_Success != true) {
				Debug.Log("Skill Check Failed");

				// Show SkillCheck Warning
				SkillCheck_Text.gameObject.SetActive(false);

				// Penalty
				SkillCheck_Penalty();

				// Reset To Allow Looping
				SkillCheck_Countdown = SkillCheck_Time;
				SkillCheck_Status = false;

				// Repeat Skill Check Timer At Random
				Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));

			} // end if SkillCheck false
		} // end if countdown
	} // end function SkillCheck_Timer()

	public void SkillCheck() {
		Debug.Log("Skill Check");

		// Show Skill Check Text
		SkillCheck_Text.gameObject.SetActive(true);

		// Set SkillCheck Active to True
		SkillCheck_Status = true;

	} // end function SkillCheck()

	public void SkillCheck_Penalty() {

		// Cancel Momentarily the Progress
		CancelInvoke("Generator_Progress");

		// Penalty
		Gen_Progress -= 15f;
		
		// Loop Generator Progress after 1 second
		InvokeRepeating("Generator_Progress", 2.5f, Gen_Speed);

	} // end function SkillCheck_Penalty() 

	public void SkillCheck_Interact() {
		if(SkillCheck_Status) {

			Debug.Log("Skill Check Complete");
			// Set Skill Check Success to True
			SkillCheck_Success = true;

			// Set Skill Check Status Active to False
			SkillCheck_Status = false;

			// Hide UI Notification
			SkillCheck_Text.gameObject.SetActive(false);

			// Loop Skill Check
			Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));

		} // end if SkillCheck_Status()
	} // end function SkillCheck_Interact()

	private void Generator_Progress() {	
		if(Gen_Complete != true) {
			
			// Set Repair Text
			Status_Text.text = Repair_Text.ToString();
			
			// Show Progress Bar
			Progress_Slider.gameObject.SetActive (true);
			
			// Increase Progress for Gen
			Gen_Progress++;
			
			// Rotate Gen
			transform.Rotate(0, 60, 0);

			// Slider Bar = Gen Progress
			Progress_Slider.GetComponent<Slider> ().value = Gen_Progress;

		} // end if Gen_Complete
 	} // end function Generator_Progress()

	public void Generator_Interact() {	
		// If we're looking at object
		if(isGazingUpon){

			// If the Gen isn't complete
			if(Gen_Progress != 100f) {
				
				// Skill Check
				Invoke("SkillCheck", Random.Range(SkillCheck_MinTime, SkillCheck_MaxTime));

				// Repeat Generator Progress
				InvokeRepeating("Generator_Progress", 0f, Gen_Speed);

			} // end if Gen_Progress
		} // end if isGazingUpon
	} // end function Generator_Interaction()

	public void Cancel_Interact() {
		
		// Cancel Generator Progress
		CancelInvoke("Generator_Progress");
		
		// If in the middle of a skill check, apply penalty
		if(SkillCheck_Status) {
			SkillCheck_Penalty();
		}

		// Stop Skill Check from looping
		CancelInvoke("SkillCheck");

		// Hide Progress Bar
		Progress_Slider.gameObject.SetActive(false);

	} // end function Cancel_Interact()


	// Raycast 
    public void GazingUpon() {
		// Set variable that we're looking at object
        isGazingUpon = true;
		// Allow Interaction of Generator
		Generator_Interact();
		// Set parameter to player object that we're looking at this interactable
		Player.GetComponent<Interactable>().SetInteractable(gameObject);
    }
    public void NotGazingUpon() {
		// Set variable that we're NOT looking at object
        isGazingUpon = false;
		// If we aren't looking at the object, cancel the interaction
		Cancel_Interact();
    }
} // end constructor