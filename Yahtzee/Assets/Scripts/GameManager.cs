using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool firstTurn = true;
	public bool secondTurn = false;
	public bool thirdTurn = false;
	public ArrayList dices;
	private GameObject ScoreSheet;
	private GameObject Aces;
	private GameObject Twos;
	private GameObject Threes;
	private GameObject Fours;
	private GameObject Fives;
	private GameObject Sixes;
	private GameObject ThreeOfAKind;
	private GameObject FourOfAKind;
	private GameObject FullHouse;
	private GameObject SmallStraight;
	private GameObject LargeStraight;
	private GameObject Chance;
	private GameObject Yahtzee;
	private Dictionary<string, bool> Scored;
	private Color HighlightedButtonColor = new Color (210/255f, 187/255f, 118/255f, 138/255f);
	private Color LockedColor = new Color(200/255f, 200/255f, 200/255f, 128/255f);

	void Start () {
		dices = new ArrayList();
		populateGameObjects ();
		ScoreSheet.SetActive (false);
		populateDictionary();
	}

	void Update () {
		
		if (Input.GetKeyDown ("g")) {
			dicesDebug ();
		} else if (Input.GetKeyDown ("q")) {
			sortArray ();
		} else if (Input.GetKeyDown ("p")) {
			toggleScoreSheet ();
		} else if (Input.GetKeyDown ("t")) {
			dices = new ArrayList ();
			dices.Add (6f);
			dices.Add (6f);
			dices.Add (5f);
			dices.Add (5f);
			dices.Add (5f);
			Debug.Log ("FH" + FullHouseScore ());
			Debug.Log ("SMS" + StraightScore (4));
			Debug.Log ("LGS" + StraightScore (5));
			Debug.Log ("3K" + OfAKindScore (3));
			Debug.Log ("4K" + OfAKindScore (4));
			Debug.Log ("YTZ" + YahtzeeScore ());
		}
	}

	private void populateDictionary() {
		Scored = new Dictionary<string, bool> ();
		Scored.Add ("AcesInput", false);
		Scored.Add ("TwosInput", false);
		Scored.Add ("ThreesInput", false);
		Scored.Add ("FoursInput", false);
		Scored.Add ("FivesInput", false);
		Scored.Add ("SixesInput", false);
		Scored.Add ("YahtzeeInput", false);
		Scored.Add ("ThreeOfAKindInput", false);
		Scored.Add ("FourOfAKindInput", false);
		Scored.Add ("SmallStraightInput", false);
		Scored.Add ("LargeStraightInput", false);
		Scored.Add ("FullHouseInput", false);
		Scored.Add ("ChanceInput", false);
	}

	private void populateGameObjects() {
		Aces = GameObject.Find ("AcesInput");
		Twos = GameObject.Find ("TwosInput");
		Threes = GameObject.Find ("ThreesInput");
		Fours = GameObject.Find ("FoursInput");
		Fives = GameObject.Find ("FivesInput");
		Sixes = GameObject.Find ("SixesInput");
		ThreeOfAKind = GameObject.Find ("ThreeOfAKindInput");
		FourOfAKind = GameObject.Find ("FourOfAKindInput");
		FullHouse = GameObject.Find ("FullHouseInput");
		SmallStraight = GameObject.Find ("SmallStraightInput");
		LargeStraight = GameObject.Find ("LargeStraightInput");
		Chance = GameObject.Find ("ChanceInput");
		Yahtzee = GameObject.Find ("YahtzeeInput");
		ScoreSheet = GameObject.Find ("ScoreSheet");
	}

	public void addDice(float die) {
		//Debug.Log (die);
		if (dices.Count >= 5) {
		} else {
			
			dices.Add (die);
			//Debug.Log ("added " + die);
			if (dices.Count == 5) {
				if (GameObject.Find ("RollButton")) {
					GameObject.Find ("RollButton").GetComponent<Button> ().enabled = true;
				}
				checkSpecialCases ();
			}
		}
	}

	public void removeDice(float die) {
		//Debug.Log ("removing " + die);
		if (dices.Contains (die)) {
			dices.Remove (die);
		} else {
			Debug.Log ("ERROR: die is not in list");
		}
	}

	public void toggleScoreSheet() {
		if (ScoreSheet.activeSelf) {
			ScoreSheet.SetActive (false);
		} else {
			ScoreSheet.SetActive (true);
		}
	}

	public void dicesDebug() {
		Debug.Log ("-------");
		foreach (float die in dices) {
			Debug.Log (die);
		}
		Debug.Log ("-------");
	}

	private void sortArray() {
		for (int i = 1; i < dices.Count; i++) {
			int j = i;
			while(j > 0 && (float)dices[j-1] > (float)dices[j]) {
				float temp = (float)dices [j];
				dices [j] = dices [j - 1];
				dices [j - 1] = temp;
				j = j - 1;
			}
		}
	}

	public void ScoreObject(GameObject Go) {
		Scored [Go.name] = true;
		foreach (GameObject input in GameObject.FindGameObjectsWithTag("Input")) {
			input.GetComponent<Button> ().enabled = false;
			input.GetComponent<Image> ().color = Color.white;
		}
		Go.GetComponent<Image> ().color = LockedColor;
		GameObject.Find ("CameraPivot").GetComponent<Spins> ().reset ();
		toggleScoreSheet ();
	}

	private void checkSpecialCases() {
		if (!Scored["YahtzeeInput"]) {
			effectChoice(Yahtzee);
		}
		if (!Scored["ThreeOfAKindInput"]) {
			effectChoice(ThreeOfAKind);
		}
		if (!Scored["FourOfAKindInput"]) {
			effectChoice(FourOfAKind);
		}
		if (!Scored["SmallStraightInput"]) {
			effectChoice(SmallStraight);
		}
		if (!Scored["LargeStraightInput"]) {
			effectChoice(LargeStraight);
		}
		if (!Scored["FullHouseInput"]) {
			effectChoice(FullHouse);
		}
		if (!Scored["ChanceInput"]) {
			effectChoice(Chance);
		}
		if (!Scored["AcesInput"]) {
			effectChoice(Aces);
		}
		if (!Scored["TwosInput"]) {
			effectChoice(Twos);
		}
		if (!Scored["ThreesInput"]) {
			effectChoice(Threes);
		}
		if (!Scored["FoursInput"]) {
			effectChoice(Fours);
		}
		if (!Scored["FivesInput"]) {
			effectChoice(Fives);
		}
		if (!Scored["SixesInput"]) {
			effectChoice(Sixes);
		}
	}

	private void effectChoice(GameObject Go) {
		Go.GetComponent<Button> ().enabled = true;
		//Go.GetComponent<Image> ().color = HighlightedButtonColor;
	}

	public bool YahtzeeScore() {
		float value = (float)dices[0];
		foreach (float die in dices) {
			if (die != value) {
				return false;
			}
		}
		return true;
	}

	public bool OfAKindScore(int kind) {
		if (kind != 3 && kind != 4) {
			Debug.Log ("Wrong kind of kind");
			return false;
		}
		for (int i = 0; i < ((kind % 2) + 2); i++) {
			float value = (float)dices [i];
			int count = 1;
			for (int j = i + 1; j < 5; j++) {
				if ((float)dices [j] == value) {
					count++;
				}
				if (count == kind) {
					return true;
				}
			}
		}
		return false;
	}

	public bool StraightScore(int kind) {
		sortArray ();
		if (kind != 4 && kind != 5) {
			Debug.Log ("Wrong kind of straight");
			return false;
		}
		float value = (float)dices [0];
		if (value != 1 && value != 2 && value != 3) {
			return false;
		}
		int count = 1;
		for (int i = 1; i < 5; i++) {
			if ((float)dices [i] != value + 1 && (float)dices [i] != value) {
				return false;
			}
			if ((float)dices [i] == value + 1) {
				count++;
			}
			value = (float)dices [i];
		}
		if (count >= kind) {
			return true;
		}
		return false;
	}

	public bool FullHouseScore() {
		sortArray ();
		float value = (float)dices [0];
		int count = 1;
		for (int i = 1; i < 5; i++) {
			if ((float)dices [i] == value) {
				count++;
			}
		}
		if (count == 2 || count == 3) {
			if ((float)dices [1] == (float)dices [0]) {
				if ((float)dices [2] == (float)dices [0]) {
					if ((float)dices [3] == (float)dices [4]) {
						return true;
					}
				} else if ((float)dices [2] == (float)dices [3] && (float)dices [2] == (float)dices [4]) {
					return true;
				}
			} else if((float)dices[1] == (float)dices[2] || (float)dices[1] == (float)dices[3]){
				return true;
			}
		}
		return false;
	}
		
}
