using UnityEngine;
using System.Collections;

public class DiceBehavior1 : MonoBehaviour {
	public float rolled;
	private float prevPos;
	private bool activated;

	// Use this for initialization
	void Start () {
		prevPos = -10000;
		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Rigidbody> ().IsSleeping () && !activated) {
			foreach (Transform tran in this.GetComponentInChildren<Transform> ()) {
				if (prevPos < tran.position.y) {
					rolled = float.Parse (tran.gameObject.name);
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

	/*void OnMouseDrag() {
		Vector3 camPos = GameObject.Find("Main Camera").transform.position;
		RectTransform screen = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		this.transform.position = new Vector3 (camPos.x + (Input.mousePosition.x / screen.rect.width), 
			camPos.y - 4 + (Input.mousePosition.y / screen.rect.height), camPos.z + 5);
	}*/
}
