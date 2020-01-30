using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenCount : MonoBehaviour {
	public GameObject genPrefab;
	public GameObject[] Gens;
	public int RemainingGenerators;

	// Use this for initialization
	void Start () {
		Gens = GameObject.FindGameObjectsWithTag("gen");
		foreach(var Gen in Gens) {
			if(Gen.GetComponent<Generator>().Gen_Complete != true) {
				RemainingGenerators++;
			}
		}
	}
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Text>().text = RemainingGenerators.ToString(); 
	}
	public void CompleteGenerator() {
		Debug.Log("Completed the Generator");
		RemainingGenerators -= 1;
	}
}
