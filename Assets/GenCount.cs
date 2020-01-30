using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenCount : MonoBehaviour {
	public GameObject genPrefab;
	public GameObject[] Gens;

	// Use this for initialization
	void Start () {
		Gens = GameObject.FindGameObjectsWithTag("gen");
		gameObject.GetComponent<Text>().text = Gens.Length.ToString(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
