using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TotalBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PseudoUpperSubTotal() {
		this.gameObject.GetComponentInChildren<Text> ().text = "" + (int.Parse (GameObject.Find("AcesInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("TwosInput").GetComponentInChildren<Text>().text) + int.Parse (GameObject.Find("ThreesInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("FoursInput").GetComponentInChildren<Text>().text) + int.Parse (GameObject.Find("FivesInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("SixesInput").GetComponentInChildren<Text>().text));
		PseudoUpperTotal ();
		Bonus ();
		UpperTotal ();
		GrandTotal ();
	}

	public void PseudoUpperTotal() {
		GameObject.Find("PseudoUpperTotal").GetComponentInChildren<Text> ().text = "" + (int.Parse (GameObject.Find("PseudoUpperSubTotal").GetComponentInChildren<Text>().text) +
			int.Parse (GameObject.Find("Bonus").GetComponentInChildren<Text>().text));
	}

	public void Bonus() {
		if (int.Parse (GameObject.Find ("AcesInput").GetComponentInChildren<Text> ().text) +
		    int.Parse (GameObject.Find ("TwosInput").GetComponentInChildren<Text> ().text) + int.Parse (GameObject.Find ("ThreesInput").GetComponentInChildren<Text> ().text) +
		    int.Parse (GameObject.Find ("FoursInput").GetComponentInChildren<Text> ().text) + int.Parse (GameObject.Find ("FivesInput").GetComponentInChildren<Text> ().text) +
		    int.Parse (GameObject.Find ("SixesInput").GetComponentInChildren<Text> ().text) >= 63) {
			GameObject.Find("Bonus").GetComponentInChildren<Text> ().text = "35";
		}
	}

	public void UpperTotal() {
		GameObject.Find ("UpperTotal").GetComponentInChildren<Text> ().text = this.gameObject.GetComponentInChildren<Text> ().text;
	}

	public void LowerTotal() {
		this.gameObject.GetComponentInChildren<Text> ().text = "" + (int.Parse (GameObject.Find("ThreeOfAKindInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("FourOfAKindInput").GetComponentInChildren<Text>().text) + int.Parse (GameObject.Find("FullHouseInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("SmallStraightInput").GetComponentInChildren<Text>().text) + int.Parse (GameObject.Find("LargeStraightInput").GetComponentInChildren<Text>().text) + 
			int.Parse (GameObject.Find("YahtzeeInput").GetComponentInChildren<Text>().text) + int.Parse (GameObject.Find("ChanceInput").GetComponentInChildren<Text>().text));
		GrandTotal ();
	}

	public void GrandTotal() {
		GameObject.Find("GrandTotal").GetComponentInChildren<Text> ().text = "" + (int.Parse (GameObject.Find("UpperTotal").GetComponentInChildren<Text>().text) +
			int.Parse (GameObject.Find("LowerTotal").GetComponentInChildren<Text>().text));
	}
}
