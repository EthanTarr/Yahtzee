using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreInputBehavior : MonoBehaviour {

	public void setActive(bool active) {
		this.GetComponent<Button> ().enabled = active;
	}
}
