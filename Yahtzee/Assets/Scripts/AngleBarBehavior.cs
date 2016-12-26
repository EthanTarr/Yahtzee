using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class AngleBarBehavior : MonoBehaviour {
	private GameObject Angle;

	// Use this for initialization
	void Start () {
		Angle = GameObject.Find ("Angle");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPointerDown(PointerEventData data) {
		GameObject.Find ("PowerGuage").GetComponent<PowerBehavior> ().PowerDice();
	}
}
