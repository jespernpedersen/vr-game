using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Interactable : MonoBehaviour {
	// Controller
	// a reference to the action
	public SteamVR_Action_Boolean genRepairInput;
	// a reference to the hand
	public SteamVR_Input_Sources handType;
	public GameObject Generator;

	// Raycast
	public Camera viewCamera;
	private GameObject lastGazedUpon;
	private bool statusController;

	// UI
	public Slider UISlider;

	// Use this for initialization
	void Start () {
		// Input
		genRepairInput.AddOnStateDownListener(TriggerDown, handType);
		genRepairInput.AddOnStateUpListener(TriggerUp, handType);		
		statusController = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
    	CheckGaze();
	}  
	public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
		Generator.GetComponent<Generator>().Cancel_Interact();
	}
	private void CheckGaze() {
		if (lastGazedUpon){
			lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
			Generator.GetComponent<Generator>().Cancel_Interact();
		}
		Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
		RaycastHit hit;
		if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity)){
			Debug.Log("Gazing");
			hit.transform.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);
			lastGazedUpon = hit.transform.gameObject;
			Generator.GetComponent<Generator>().Generator_Interact();
		}
    }
}
