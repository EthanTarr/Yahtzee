using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public bool firstTurn = true;
	public bool secondTurn = false;
	public bool thirdTurn = false;
	public ArrayList dices;
	private bool counted = false;


	void Start () {
		dices = new ArrayList();
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

}
