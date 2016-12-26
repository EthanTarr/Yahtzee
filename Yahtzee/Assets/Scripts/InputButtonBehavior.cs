using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputButtonBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private string prevValue;
	private GameManager GO;
	private Color OnHighlight = new Color(200/255f, 200/255f, 200/255f, 180/255f);

	void Start () {
		prevValue = "0";
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	//checks what input this GameObject is and updates its score value accordingly
	public void OnPointerEnter(PointerEventData eventData) {
		if (this.gameObject.GetComponent<Button> ().enabled) { //check to see if this has been scored already
			prevValue = this.gameObject.GetComponentInChildren<Text> ().text;
			if (this.gameObject.name.Equals ("AcesInput")) {
				ChangeUpperNumber (1);
			} else if (this.gameObject.name.Equals ("TwosInput")) {
				ChangeUpperNumber (2);
			} else if (this.gameObject.name.Equals ("ThreesInput")) {
				ChangeUpperNumber (3);
			} else if (this.gameObject.name.Equals ("FoursInput")) {
				ChangeUpperNumber (4);
			} else if (this.gameObject.name.Equals ("FivesInput")) {
				ChangeUpperNumber (5);
			} else if (this.gameObject.name.Equals ("SixesInput")) {
				ChangeUpperNumber (6);
			} else if (this.gameObject.name.Equals ("ThreeOfAKindInput")) {
				if (GO.OfAKindScore (3)) {
					ChangeTotalNumber ();
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if(this.gameObject.name.Equals ("FourOfAKindInput")) {
				if (GO.OfAKindScore (4)) {
					ChangeTotalNumber ();
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if(this.gameObject.name.Equals ("ChanceInput")) {
				ChangeTotalNumber ();
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if (this.gameObject.name.Equals ("SmallStraightInput")) {
				if(GO.StraightScore(4)) {
				this.gameObject.GetComponentInChildren<Text> ().text = "30";
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if (this.gameObject.name.Equals ("LargeStraightInput")) {
				if(GO.StraightScore(5)) {
					this.gameObject.GetComponentInChildren<Text> ().text = "40";
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if (this.gameObject.name.Equals ("FullHouseInput")) {
				if(GO.FullHouseScore()) {
					this.gameObject.GetComponentInChildren<Text> ().text = "25";
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			} else if (this.gameObject.name.Equals ("YahtzeeInput")) {
				if(GO.YahtzeeScore()) {
					this.gameObject.GetComponentInChildren<Text> ().text = "50";
				}
				this.gameObject.GetComponent<Image> ().color = OnHighlight;
			}
		}
	}

	//restores GameObject to previous state
	public void OnPointerExit(PointerEventData eventData) {
		if (this.gameObject.GetComponent<Button> ().enabled) {
			this.gameObject.GetComponentInChildren<Text> ().text = prevValue;
			this.gameObject.GetComponent<Image> ().color = Color.white;
		}
	}

	//changes value to appropriate number
	private void ChangeTotalNumber() {
		int sum = 0;
		foreach (int num in GameObject.Find("GameManager").GetComponent<GameManager>().dices) {
			sum += num;
		}
		this.gameObject.GetComponentInChildren<Text> ().text = "" + sum;
	}

	//changes balue to appropriate number
	private void ChangeUpperNumber(int value) {
		int sum = 0;
		foreach (int num in GameObject.Find("GameManager").GetComponent<GameManager>().dices) {
			if (num == value) {
				sum += num;
			}
		}
		this.gameObject.GetComponentInChildren<Text> ().text = "" + sum;
		this.gameObject.GetComponent<Image> ().color = OnHighlight;
	}
}
