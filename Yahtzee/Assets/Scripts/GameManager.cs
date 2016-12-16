using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public bool firstTurn = true;
	public bool secondTurn = false;
	public bool thirdTurn = false;
	public int DiceIconSpeed = 1;
	public int numOfDice = 1;
	public ArrayList dices;
	private int savedDice = 0;
	private Vector3 dicePosition;
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
	private GameObject dice;
	private GameObject roll;
	private GameObject[] Saveds;
	private Dictionary<string, bool> Scored;
	private RollButtonBehavior RBB;
	private Color HighlightedButtonColor = new Color (210/255f, 187/255f, 118/255f, 138/255f);
	private Color LockedColor = new Color(200/255f, 200/255f, 200/255f, 128/255f);

	void Start () {
		dices = new ArrayList();
		populateGameObjects ();
		ScoreSheet.SetActive (false);
		populateDictionary();
		RBB = GameObject.Find ("RollButton").GetComponent<RollButtonBehavior> ();
		dice.SetActive (false);
		//dicePosition = new Vector3 (0, 2, -5);
		expandCarpet ();
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
			dices.Add (5);
			dices.Add (5);
			dices.Add (5);
			dices.Add (5);
			dices.Add (5);
			Debug.Log ("FH " + FullHouseScore ());
			Debug.Log ("SMS " + StraightScore (4));
			Debug.Log ("LGS " + StraightScore (5));
			Debug.Log ("3K " + OfAKindScore (3));
			Debug.Log ("4K " + OfAKindScore (4));
			Debug.Log ("YTZ " + YahtzeeScore ());
		}
	}

	private void expandCarpet() {
		GameObject cp = GameObject.Find ("Carpet");
		GameObject cps = GameObject.Find ("Carpets");
		for (int i = -35; i < 35; i++) {
			for (int j = -20; j < 20; j++) {
				GameObject temp = (GameObject)Instantiate (cp, cp.transform.position + new Vector3 ((float)(i * 1.795619), 0, (float)(j * 7.683)), cp.transform.rotation);
				temp.transform.parent = cps.transform;
			}
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
		dice = GameObject.Find ("Dice");
		Saveds = new GameObject[5];
		for (int i = 1; i <= 5; i++) {
			Saveds[i - 1] = (GameObject.Find ("Saved" + i));
			Saveds [i - 1].SetActive (false);
		}
	}

	public void addDice(int die) {
		dices.Add (die);
		if (dices.Count == 5) {
			if (GameObject.Find ("RollButton")) {
				GameObject.Find ("RollButton").GetComponent<Button> ().enabled = true;
			}
			sortArray ();
			checkSpecialCases ();
			addDiceToIcons ();
		}
	}

	public void removeDice(int die) {
		//Debug.Log ("removing " + die);
		if (dices.Contains (die)) {
			dices.Remove (die);
		} else {
			Debug.Log ("ERROR: die is not in list");
		}
	}

	public void addDiceToIcons() {
		ArrayList Collection = new ArrayList (); //GameObjects of rolled dice

		//find how many dice were saved from last throw
		foreach (GameObject save in GameObject.FindGameObjectsWithTag("Image")) {
			if (save.activeSelf) {
				Collection.Add(new GameObject()); // temp gameobject to keep indexing the same
			}
		}

		//find all the dice that have been rolled and save their values
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Dice")) {
			Collection.Add (die);
		}

		//place every rolled dice into the saveds UI
		for (int i = 0; i < dices.Count; i++) {
			Saveds [i].GetComponent<Image> ().sprite =
				Resources.Load<Sprite> ("Images/DieSide" + (int)dices [i]);
			Saveds [i].SetActive(true);
			Destroy ((UnityEngine.Object)Collection [i]);
		}

		if (!firstTurn && !secondTurn && !thirdTurn) {
			toggleScoreSheet();
		}
	}

	public void toggleScoreSheet() {
		if (ScoreSheet.activeSelf) {
			ScoreSheet.GetComponent<Animator> ().SetTrigger ("Close");
		} else {
			ScoreSheet.SetActive (true);
			ScoreSheet.GetComponent<Animator> ().SetTrigger ("Open");
		}
	}

	public void dicesDebug() {
		Debug.Log ("-------");
		foreach (int die in dices) {
			Debug.Log (die);
		}
		Debug.Log ("-------");
	}

	private void sortArray() {
		for (int i = 1; i < dices.Count; i++) {
			int j = i;
			while(j > 0 && (int)dices[j-1] > (int)dices[j]) {
				int temp = (int)dices [j];
				dices [j] = dices [j - 1];
				dices [j - 1] = temp;
				j = j - 1;
			}
		}
	}

	//Gives the given UI scoring object a permanent score
	public void ScoreObject(GameObject Go) {
		Scored [Go.name] = true; // Marks the given object as scored

		//deactivates all the other Input objects
		foreach (GameObject input in GameObject.FindGameObjectsWithTag("Input")) {
			input.GetComponent<Button> ().enabled = false;
			input.GetComponent<Image> ().color = Color.white;
		}
			
		Go.GetComponent<Image> ().color = LockedColor;
		reset (); 
		toggleScoreSheet ();
	}

	//resets the round of the game
	public void reset() {

		// set gamestate to first turn
		firstTurn = true;
		secondTurn = false;
		thirdTurn = false;

		for (int i = 4; i >= 0; i--) {
			Saveds [i].SetActive (false);
		}
		savedDice = 0;
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Dice")) {
			Destroy (die);
		}
		/*
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Highlight")) {
			Destroy (die);
		}*/
		dices = new ArrayList ();
		RBB.FirstTurn ();
	}

	IEnumerator SummonDice(int num) {
		GameObject.Find ("RollButton").GetComponent<Button> ().enabled = false;
		int i = 0;
		while (i < num) {
			roll = (GameObject)Instantiate (dice, new Vector3(9f, 3.5f, 1f), Quaternion.identity);
			roll.SetActive (true);
			/*
			roll = (GameObject)Instantiate (dice, GameObject.Find ("Main Camera").transform.position + new Vector3(0, -10, 0), Quaternion.identity);
			roll.SetActive (true);
			Vector3 camera = GameObject.Find ("CameraPivot").transform.rotation.eulerAngles;
			if (camera.y < 45 || camera.y > 135 && camera.y < 225 || camera.y > 315) {
				roll.GetComponent<Rigidbody> ().AddForce (new Vector3 ((float)(Random.Range (-28, 28) * (Mathf.Cos (Mathf.Deg2Rad * camera.y))), Random.Range (3, 10), (float)(Random.Range (50, 75) * (Mathf.Cos (Mathf.Deg2Rad * camera.y)))));
			} else {
				roll.GetComponent<Rigidbody> ().AddForce (new Vector3 ((float)(Random.Range (50, 75) * (Mathf.Sin (Mathf.Deg2Rad * camera.y))), Random.Range (3, 10), (float)(Random.Range (-28, 28) * (Mathf.Sin (Mathf.Deg2Rad * camera.y)))));
			}
			*/
			roll.GetComponent<Rigidbody> ().AddForce (new Vector3 ((float)(Random.Range (-50, -75)), Random.Range (3, 10), (float)(Random.Range (-28, 28))));
			roll.GetComponent<Rigidbody> ().AddTorque (new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-30, 30)));
			i++;
			yield return new WaitForSeconds (.5f);
		}
	}

	void Dice(int num) {
		if (secondTurn || thirdTurn) {
			savedDice = 5;
			foreach (GameObject image in GameObject.FindGameObjectsWithTag("ImageHighlight")) {
				Saveds [(int.Parse (image.name.Substring (5))) - 1].GetComponent<ButtonBehavior> ().Highlight ();
				Saveds [(int.Parse (image.name.Substring (5))) - 1].SetActive (false);
				removeDice (int.Parse (image.GetComponent<Image> ().sprite.name.Substring (7)));
				savedDice--;
			}
			for (int i = 1; i < 5; i++) {
				if (Saveds [i].activeSelf) {
					int j = i;
					while (j > 0 && !Saveds [j - 1].activeSelf) {
						GameObject curtemp = Saveds [j];
						GameObject prevtemp = Saveds [j - 1];
						prevtemp.SetActive (true);
						prevtemp.GetComponent<Image> ().sprite = curtemp.GetComponent<Image> ().sprite;
						curtemp.SetActive (false);
						j--;
					}
				}
			}
			/*foreach (GameObject die in GameObject.FindGameObjectsWithTag("Highlight")) {
				saves [savedDice].SetActive(true);
				saves[savedDice].GetComponent<Image> ().sprite = 
				Resources.Load<Sprite> ("Images/DieSide" + (int)die.GetComponent<DiceBehavior1> ().rolled);
				Destroy (die);
				savedDice++;
			}*/
		}
		StartCoroutine (SummonDice (num - savedDice));
		if (firstTurn) {
			firstTurn = false;
			secondTurn = true;
			RBB.NotFirstTurn ();

		} else if (secondTurn) {
			secondTurn = false;
			thirdTurn = true;
		} else {
			thirdTurn = false;
			GameObject.Find ("RollButton").SetActive(false);
		}
	}

	public void StartSummonDice() {
		Dice (numOfDice);
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
		int value = (int)dices[0];
		foreach (int die in dices) {
			Debug.Log (die);
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
			int value = (int)dices [i];
			int count = 1;
			for (int j = i + 1; j < 5; j++) {
				if ((int)dices [j] == value) {
					count++;
				}
				if (count == kind) {
					return true;
				}
			}
		}
		return false;
	}

	//boolean test to see if rolled values are in a straight formation of the given kind
	public bool StraightScore(int kind) {
		//debug check to see if given int is a valid staight
		if (kind != 4 && kind != 5) {
			Debug.Log ("Wrong kind of straight");
			return false;
		}

		int startIndex = 1; 
		int value = (int)dices [0];

		//initial check to reduce computation
		if (value != 1 && value != 2 && value != 3) {
			return false;
		} else if (kind == 4 && (int)dices [1] == value + 2) { //for the special case of [1,3,4,5,6]
			value = (int)dices [1];
			startIndex = 2;
		}
		int count = 1;
		for (int i = startIndex; i < 5; i++) {
			if ((int)dices [i] != value + 1 && (int)dices [i] != value) { //checks sequential ordering or duplicates
				return false;
			}
			if ((int)dices [i] == value + 1) { //if order is sequential
				count++;
			} else if(kind != 4) { //if duplicates exists
				return false;
			}
			value = (int)dices [i];
		}
		if (count >= kind) {
			return true;
		}
		return false;
	}

	public bool FullHouseScore() {
		int value = (int)dices [0];
		int count = 1;
		for (int i = 1; i < 5; i++) {
			if ((int)dices [i] == value) {
				count++;
			}
		}
		if (count == 2 || count == 3) {
			if ((int)dices [1] == (int)dices [0]) {
				if ((int)dices [2] == (int)dices [0]) {
					if ((int)dices [3] == (int)dices [4]) {
						return true;
					}
				} else if ((int)dices [2] == (int)dices [3] && (int)dices [2] == (int)dices [4]) {
					return true;
				}
			} else if((int)dices[1] == (int)dices[2] || (int)dices[1] == (int)dices[3]){
				return true;
			}
		}
		return false;
	}
		
}
