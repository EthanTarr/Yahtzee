using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public bool firstTurn = true;
	public bool secondTurn = false;
	public bool thirdTurn = false;
	public ArrayList dices;
	private bool counted = false;
	private GameObject ScoreSheet;


	void Start () {
		dices = new ArrayList();
		ScoreSheet = GameObject.Find ("ScoreSheet");
		ScoreSheet.SetActive (false);
	}

	void Update () {
		if (dices != null && counted) {
			float prevnum = 0;
			float count = 0;
			foreach (float num in dices) {
				if (num == prevnum) {
					count++;
				} else {
					count = 0;
					prevnum = num;
				}
			}
			if (count == 4) {
				Debug.Log ("Yahtzee!");
			}
			counted = false;
		}

		if (Input.GetKeyDown ("g")) {
			dicesDebug ();
		} else if (Input.GetKeyDown ("q")) {
			sortArray ();
		} else if (Input.GetKeyDown ("p")) {
			toggleScoreSheet ();
		}
	}

	public void addDice(float die) {
		dices.Add (die);
		//Debug.Log ("added " + die);
		counted = true;
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

	private bool Yahtzee() {
		float value = (float)dices[0];
		foreach (float die in dices) {
			if (die != value) {
				return false;
			}
		}
		return true;
	}

	private bool OfAKind(int kind) {
		if (kind != 3 && kind != 4) {
			Debug.Log ("Wrong kind of kind");
			return false;
		}
		for (int i = 0; i < ((kind % 2) + 2); i++) {
			float value = (float)dices [i];
			int count = 0;
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

	private bool Straight(int kind) {
		if (kind != 4 && kind != 5) {
			Debug.Log ("Wrong kind of straight");
			return false;
		}
		float value = (float)dices [0];
		if (value != 1 && value != 2) {
			return false;
		}
		for (int i = 1; i < kind; i++) {
			if ((float)dices [i] != value + 1) {
				return false;
			}
			value = (float)dices [i];
		}
		return true;
	}

	private bool FullHouse() {
		float value = (float)dices [0];
		int count = 1;
		for (int i = 1; i < 5; i++) {
			if ((float)dices [i] == value) {
				count++;
			}
		}
		if (count == 2 || count == 3) {
			if (dices [1] == dices [0]) {
				if (dices [2] == dices [0]) {
					if (dices [3] == dices [4]) {
						return true;
					}
				} else if (dices [2] == dices [3] && dices [2] == dices [4]) {
					return true;
				}
			} else if(dices[1] == dices[2] || dices[1] == dices[3]){
				return true;
			}
		}
		return false;
	}
		
}
