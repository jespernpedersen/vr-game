using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Interactable : MonoBehaviour {
	// Controller
	// a reference to the action
	public SteamVR_Action_Boolean InteractionInput;
	public SteamVR_Action_Boolean GameObjectInteract;
	// a reference to the hand
	public SteamVR_Input_Sources handType;
	public GameObject Generator;

	// Raycast
	public Camera viewCamera;
	private GameObject lastGazedUpon;

	// UI
	public Slider UISlider;

	// Use this for initialization
	void Start () {
		// Input
		InteractionInput.AddOnStateDownListener(TriggerDown, handType);
		InteractionInput.AddOnStateUpListener(TriggerUp, handType);	
		GameObjectInteract.AddOnStateUpListener(Interact, handType);
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
		// When triggering down, check if looking at any interactable
    	CheckGaze();
	}  
	public void Interact(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
		Generator.GetComponent<Generator>().SkillCheck_Interact();
	}
	public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
		// When Releasing Trigger, we should cancel interaction
		Generator.GetComponent<Generator>().Cancel_Interact();
	}
	private void CheckGaze() {
		if (lastGazedUpon){
			lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
		}

		Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
		RaycastHit hit;

		if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity)){
			hit.transform.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);
			lastGazedUpon = hit.transform.gameObject;
		}
    }
}
