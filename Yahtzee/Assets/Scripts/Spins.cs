using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spins : MonoBehaviour {
	public float speed = 2;
	public bool spinControls = false;
	private float xLimit;
	private float zLimit;
	private GameManager GO;
	private RollButtonBehavior RBB;


	// Use this for initialization
	void Start () {
		
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		RBB = GameObject.Find ("RollButton").GetComponent<RollButtonBehavior> ();
		if (GameObject.Find ("Floor")) {
			xLimit = GameObject.Find ("Floor").transform.localScale.x * 5;
			zLimit = GameObject.Find ("Floor").transform.localScale.z * 5;
		}


	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (spinControls) {
			if (Input.GetKey (KeyCode.A)) {
				transform.Rotate (0, speed, 0);
			} else if (Input.GetKey (KeyCode.D)) {
				transform.Rotate (0, -speed, 0);
			}
		} else {
			if (Input.GetKey (KeyCode.W) && transform.position.z <= zLimit) {
				transform.Translate (0, 0, speed);
			} else if (Input.GetKey (KeyCode.S) && transform.position.z >= -zLimit) {
				transform.Translate (0, 0, -speed);
			} 
			if (Input.GetKey (KeyCode.A) && transform.position.x >= -xLimit) {
				transform.Translate (-speed, 0, 0);
			} else if (Input.GetKey (KeyCode.D) && transform.position.x <= xLimit) {
				transform.Translate (speed, 0, 0);
			} 
		}
		*/
		if (Input.GetKey (KeyCode.Space)) {
			Application.LoadLevel (Application.loadedLevel);
		} 
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}
	}



	/*
	public void unsave() {
		float xLocation = Random.Range (-xLimit, xLimit);
		float zLocation = Random.Range (-zLimit, zLimit);
		foreach (GameObject die in GO.dices) {
			if (xLocation <= die.transform.position.x + .5 && xLocation >= die.transform.position.x - .5) {

			}
		}
		Vector3 pos = new Vector3 (xLocation, 1,zLocation);
		Instantiate (dice, dicePosition, Quaternion.identity);
	}
	*/
}
