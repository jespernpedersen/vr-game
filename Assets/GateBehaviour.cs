using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GateBehaviour : MonoBehaviour, iGazeReceiver {
	private float Gate_Progress;
	private bool GensComplete;
	public bool Gate_Complete;
	public float Gate_Speed;
	public GameObject Gate_Blocker;
	public GameObject GenCounter;
	public int RemainingGen;

	// UI
	public Slider Progress_Slider;
	public Text Status_Text;
	public string Gate_Text;

	// Raycast
	public GameObject Player;
	private bool isGazingUpon;

	// Gate
    public Vector3 destination;
    Vector3 right = new Vector3 (0, 0, -10f); //Vector in the direction you want to move in.
 
	// Use this for initialization
	void Start () {
		Gate_Blocker = this.gameObject.transform.GetChild(0).gameObject;

		// Gate Status
		Gate_Progress = 0f;

		// Every Gen starts uncompleted
		Gate_Complete = false;
	}
	// Update is called once per frame
	void Update () {
		if(Gate_Complete != true) {
			if(Gate_Progress == 100f) {  
				CompleteGate();
			}
		}
		RemainingGen = GenCounter.GetComponent<GenCount>().RemainingGenerators;

	}

	public void CompleteGate() {
		Debug.Log("Won the Game");
		
		// Gen is Complete    
		Gate_Complete = true;
		
		// Open Gate
		StartCoroutine (smooth_move (right, 1f)); //Calling the coroutine.

		// Cancel Invoke
		Cancel_Interact();

	}  // end function CompleteGate()

	IEnumerator smooth_move(Vector3 direction, float speed){
         float startime = Time.time;
         Vector3 start_pos = transform.position; //Starting position.
         Vector3 end_pos = transform.position + direction; //Ending position.
 
         while (start_pos != end_pos && ((Time.time - startime)*speed) < 1f) { 
             float move = Mathf.Lerp (0,1, (Time.time - startime)*speed);
 
             Gate_Blocker.transform.position += direction*move;
 
             yield return null;
         }
     }
	private void Gate_Progressing() {
		if(RemainingGen == 0) {
			if(Gate_Complete != true) {
				
				// Set Gate Text
				Status_Text.text = Gate_Text.ToString();
				
				// Show Progress Bar
				Progress_Slider.gameObject.SetActive (true);
				
				// Increase Progress for Gen
				Gate_Progress++;

				// Slider Bar = Gen Progress
				Progress_Slider.GetComponent<Slider> ().value = Gate_Progress;

			} // end if Gen_Complete
		} // end if RemainingGen = 0
 	} // end function Generator_Progress()

	public void Gate_Interact() {	
		// If we're looking at object
		if(isGazingUpon){
			// If the Gen isn't complete
			if(Gate_Progress != 100f) {

				// Repeat Generator Progress
				InvokeRepeating("Gate_Progressing", 0f, Gate_Speed);

			} // end if Gate_Progress
		} // end if isGazingUpon
	} // end function Gate_Interaction()

	public void Cancel_Interact() {
		
		// Cancel Generator Progress
		CancelInvoke("Gate_Progressing");

		// Hide Progress Bar
		Progress_Slider.gameObject.SetActive(false);

	} // end function Cancel_Interact()

	// Raycast 
    public void GazingUpon() {
		// Set variable that we're looking at object
        isGazingUpon = true;
		// Allow Interaction of Generator
		Gate_Interact();
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