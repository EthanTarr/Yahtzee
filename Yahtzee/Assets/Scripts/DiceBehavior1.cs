using UnityEngine;
using System.Collections;

public class DiceBehavior1 : MonoBehaviour {
	public int rolled;
	public AudioClip Dice;
	public AudioClip Table;
	private AudioSource audio;
	private float prevPos;
	private bool activated;

	// Use this for initialization
	void Start () {
		prevPos = -10000;
		activated = false;
		audio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Rigidbody> ().IsSleeping () && !activated) {
			foreach (Transform tran in this.GetComponentInChildren<Transform> ()) {
				if (prevPos < tran.position.y) {
					rolled = int.Parse (tran.gameObject.name);
					prevPos = tran.position.y;
				}
			}
			//GameObject.Find ("GameManager").GetComponent<GameManager> ().addDice (rolled);
			GameObject.Find ("GameManager").GetComponent<GameManager> ().addDice (rolled);
			activated = true;
		} else if (activated && !this.GetComponent<Rigidbody> ().IsSleeping ()) {
			activated = false;
			//GameObject.Find ("GameManager").GetComponent<GameManager> ().removeDice (rolled);
			GameObject.Find ("GameManager").GetComponent<GameManager> ().removeDice (rolled);
		}
	}

	void OnMouseDown() {
		if (this.GetComponent<Rigidbody> ().IsSleeping ()) {
			if (this.tag.Equals ("Highlight")) {
				this.tag = "Dice";
				foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>()) {
					sr.color = Color.white;
				}
			} else {
				this.tag = "Highlight";
				foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>()) {
					sr.color = Color.grey;
				}
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.relativeVelocity.magnitude > 1) {
			audio.pitch = 1 + Random.value;
			if (other.gameObject.name.Equals ("Dice(Clone)")) {
				audio.PlayOneShot (Dice);
			} else {
				audio.PlayOneShot (Table);
			}
		}
	}
}
