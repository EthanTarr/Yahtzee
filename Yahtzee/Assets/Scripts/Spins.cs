using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spins : MonoBehaviour {
	public float speed = 2;
	public bool spinControls = false;
	private float xLimit;
	private float zLimit;
	private GameManager GO;


	// Use this for initialization
	void Start () {
		
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		if (GameObject.Find ("Floor")) {
			xLimit = GameObject.Find ("Floor").transform.localScale.x * 5;
			zLimit = GameObject.Find ("Floor").transform.localScale.z * 5;
		}


	}

	void FixedUpdate() {
		if (spinControls) {
			int cameraRotation = (int) transform.localRotation.eulerAngles.y;
			if(Input.GetKey(KeyCode.V)) {
				Debug.Log(cameraRotation);
			}
			if (cameraRotation <= 90 || cameraRotation >= 270) { // boundaries
				transform.Rotate (0, -speed * Input.GetAxis("Horizontal"), 0);
			} else if (cameraRotation > 90 && cameraRotation < 180) {
				transform.eulerAngles = new Vector3 (0, 90, 0);
			} else { // (cameraRotation < 270 && cameraRotation > 180)
				transform.eulerAngles = new Vector3 (0, 270, 0);
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
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey (KeyCode.Space)) {
			Application.LoadLevel (Application.loadedLevel);
		} 
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}
			
	}
		
}
