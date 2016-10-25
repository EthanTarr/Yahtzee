using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RollButtonBehavior : MonoBehaviour {
	private GameManager GO;
	private GameObject RB;

	// Use this for initialization
	void Start () {
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		RB = GameObject.Find ("RollButton");
		FirstTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FirstTurn() {
		RB.SetActive (true);
		RB.GetComponent<Button> ().enabled = true;
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Roll";
	}

	public void NotFirstTurn() {
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Re-Roll";
	}
}
