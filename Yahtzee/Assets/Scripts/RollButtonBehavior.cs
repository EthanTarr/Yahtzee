using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RollButtonBehavior : MonoBehaviour {
	private GameManager GO;

	// Use this for initialization
	void Start () {
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		FirstTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FirstTurn() {
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Roll";
	}

	public void NotFirstTurn() {
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Re-Roll";
	}
}
