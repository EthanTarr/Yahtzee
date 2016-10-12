using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreInputBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setActive(bool active) {
		this.GetComponent<Button> ().enabled = active;
	}
}
