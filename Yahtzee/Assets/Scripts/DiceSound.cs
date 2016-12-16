using UnityEngine;
using System.Collections;

public class DiceSound : MonoBehaviour {

	public AudioClip Dice;
	public AudioClip Table;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other) {
		Debug.Log (other.relativeVelocity.magnitude);
		if (other.relativeVelocity.magnitude > 1) {
			audio.pitch = 1 + Random.value;
			if (other.gameObject.name.Equals ("1") || other.gameObject.name.Equals ("2") || other.gameObject.name.Equals ("3") || other.gameObject.name.Equals ("4") || other.gameObject.name.Equals ("5") || other.gameObject.name.Equals ("6")) {
				audio.PlayOneShot (Dice);
			} else {
				audio.PlayOneShot (Table);
			}
		}
	}
}
