using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class PowerBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
	private GameObject Power;
	private GameObject AngleBar;
	public bool canRoll;

	// Use this for initialization
	void Start () {
		Power = GameObject.Find ("Power");
		Power.SetActive (false);
		AngleBar = GameObject.Find ("AngleBar");
		AngleBar.SetActive (false);

		FirstTurn ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FirstTurn() {
		this.gameObject.SetActive (true);
		canRoll = true;
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Roll";
	}

	public void NotFirstTurn() {
		GameObject.Find ("RollButtonText").GetComponent<Text> ().text = "Re-Roll";
	}

	public void OnPointerDown(PointerEventData data) {
		if (canRoll) {
			Debug.Log ("!");
			Power.SetActive (true);
		}
		if (Power.GetComponent<Animator> ().speed == 0) {
			Debug.Log ("?");
			PowerDice();
		}
	}

	public void OnPointerUp(PointerEventData data) {
		if (canRoll) {
			AngleBar.SetActive (true);
			Power.GetComponent<Animator> ().speed = 0;
		}
	}

	public void PowerDice() {

		canRoll = false;
		Debug.Log ("Size = " + Power.GetComponent<RectTransform> ().sizeDelta.x / 100f);
		Debug.Log ("Angle = " + GameObject.Find("Angle").GetComponent<RectTransform> ().localPosition.x / 45f);
		GameObject.Find ("GameManager").GetComponent<GameManager> ().StartSummonDice (Power.GetComponent<RectTransform> ().sizeDelta.x / 100f, GameObject.Find("Angle").GetComponent<RectTransform> ().localPosition.x / 45f);
		Power.GetComponent<Animator> ().speed = 1;
		Power.SetActive (false);
		AngleBar.SetActive (false);
	}
}
