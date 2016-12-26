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
	private GameObject PowerGuage;
	private GameObject[] Saveds;
	private Dictionary<string, bool> Scored;
	private Color HighlightedButtonColor = new Color (210/255f, 187/255f, 118/255f, 138/255f);
	private Color LockedColor = new Color(200/255f, 200/255f, 200/255f, 128/255f);

	void Start () {
		dices = new ArrayList();
		populateGameObjects ();
		ScoreSheet.SetActive (false);
		populateDictionary();
		dice.SetActive (false);
		//dicePosition = new Vector3 (0, 2, -5);
		expandCarpet ();
	}

	void Update () {

		//------- debug controls --------
		if (Input.GetKeyDown ("g")) {
			dicesDebug ();
		} else if (Input.GetKeyDown ("q")) {
			sortArray ();
		} else if (Input.GetKeyDown ("p")) {
			toggleScoreSheet ();
		} else if (Input.GetKeyDown ("t")) {
			dices = new ArrayList ();
			dices.Add (1);
			dices.Add (3);
			dices.Add (4);
			dices.Add (5);
			dices.Add (6);
			Debug.Log ("FH " + FullHouseScore ());
			Debug.Log ("SMS " + StraightScore (4));
			Debug.Log ("LGS " + StraightScore (5));
			Debug.Log ("3K " + OfAKindScore (3));
			Debug.Log ("4K " + OfAKindScore (4));
			Debug.Log ("YTZ " + YahtzeeScore ());
		}
		//-------------------------------

		if (GetComponent<AudioSource> ().volume < 1) {
			GetComponent<AudioSource> ().volume += .1f * Time.deltaTime;
		}
	}

	//takes the carpet object and duplicates it in both x and z directions
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

	//populates the scored field dictionary
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

	//populates the field GameObjects
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
		PowerGuage = GameObject.Find ("PowerGuage");
		Saveds = new GameObject[5];
		for (int i = 1; i <= 5; i++) {
			Saveds[i - 1] = (GameObject.Find ("Saved" + i));
			Saveds [i - 1].SetActive (false);
		}
	}

	//adds the given int die to the dices array
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

	//removes the given int die from the dices array
	public void removeDice(int die) {
		//Debug.Log ("removing " + die);
		if (dices.Contains (die)) {
			dices.Remove (die);
		} else {
			Debug.Log ("ERROR: die is not in list");
		}
	}

	//takes all rolled dice and converts them into UI icons
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

		//enables roll button
		PowerGuage.GetComponent<PowerBehavior> ().canRoll = true;

		//if all rolls have been used toggle Score Sheet
		if (!firstTurn && !secondTurn && !thirdTurn) {
			toggleScoreSheet();
		}
	}

	//Toggles the activation of the scoresheet
	public void toggleScoreSheet() {
		if (ScoreSheet.activeSelf) {
			ScoreSheet.GetComponent<Animator> ().SetTrigger ("Close");
		} else {
			ScoreSheet.SetActive (true);
			ScoreSheet.GetComponent<Animator> ().SetTrigger ("Open");
		}
	}

	//debug prints the contents of the dices array
	public void dicesDebug() {
		Debug.Log ("-------");
		foreach (int die in dices) {
			Debug.Log (die);
		}
		Debug.Log ("-------");
	}

	//insert sorts the dices array
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
		Go.tag = "Untagged";
		reset (); 
		toggleScoreSheet ();
	}

	//resets the round of the game
	public void reset() {

		// set gamestate to first turn
		firstTurn = true;
		secondTurn = false;
		thirdTurn = false;

		// deactivate dice UI icons
		for (int i = 4; i >= 0; i--) {
			Saveds [i].SetActive (false);
		}
		savedDice = 0;

		// Remove all dice in play
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Dice")) {
			Destroy (die);
		}

		dices = new ArrayList ();
		PowerGuage.GetComponent<PowerBehavior>().FirstTurn ();
	}

	//instantiates the given int num dice and gives them force depending on the given float power
	IEnumerator SummonDice(int num, float power, float angle) {
		int i = 0;
		while (i < num) {
			roll = (GameObject)Instantiate (dice, new Vector3(9f, 3.5f, 1f), Quaternion.Euler(new Vector3(Random.Range (-180, 180), Random.Range (-180, 180), Random.Range (-180, 180))));
			roll.SetActive (true);

			// code is for when camera rotation is implemented

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

			if (power == 0 && angle == 0) { // if power is not specified
				roll.GetComponent<Rigidbody> ().AddForce (new Vector3 ((float)(Random.Range (-50, -75)), Random.Range (3, 10), (float)(Random.Range (-28, 28))));
			} else {
				Debug.Log ("power = " + ((power * -60) - 15));
				Debug.Log ("power = " + (angle * 28));
				roll.GetComponent<Rigidbody> ().AddForce (new Vector3 ((power * -70) - 5, Random.Range (3, 10), (angle * 20) + (float)(Random.Range(-10, 10))));
			}

			roll.GetComponent<Rigidbody> ().AddTorque (new Vector3 (Random.Range (-30, 30), Random.Range (-30, 30), Random.Range (-30, 30)));
			i++;
			yield return new WaitForSeconds (.5f);
		}
	}

	// summons dice depending on what is indicated by UI and updates turn counter
	void Dice(float power, float angle) {

		//checks UI for dice to reroll and updates it accordingly
		if (secondTurn || thirdTurn) {
			savedDice = 5;
			//deactivates highlighted UI dice and removes them from the dices array
			foreach (GameObject image in GameObject.FindGameObjectsWithTag("ImageHighlight")) {
				Saveds [(int.Parse (image.name.Substring (5))) - 1].GetComponent<ButtonBehavior> ().Highlight ();
				Saveds [(int.Parse (image.name.Substring (5))) - 1].SetActive (false);
				removeDice (int.Parse (image.GetComponent<Image> ().sprite.name.Substring (7)));
				savedDice--;
			}
			//relocates all remaining UI dice to priority UI spots
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
		}

		//instatiate appropriate number of dice
		StartCoroutine (SummonDice (numOfDice - savedDice, power, angle));

		//update turn state
		if (firstTurn) {
			firstTurn = false;
			secondTurn = true;
			PowerGuage.GetComponent<PowerBehavior> ().NotFirstTurn ();

		} else if (secondTurn) {
			secondTurn = false;
			thirdTurn = true;
		} else {
			thirdTurn = false;
			PowerGuage.SetActive (false);
		}
	}

	public void StartSummonDice() {
		Dice (0f, 0f);
	}

	public void StartSummonDice(float power, float angle) {
		Dice (power, angle);
	}

	//intermediate function to check if a value has been scored already and to display its value if it hasn't
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

	//-------------------------  tests for values in array 'dices' ----------------------------
	// works under the assumption that array 'dices' is sorted and contains five values that range from 1 - 6

	//boolean test for a yahtzee (all values are the same in the array)
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

	//boolean test for a four or three of a kind (contains at least that many of the same value in the array)
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
	//(sequential increase of values for 4 values for the small or all 5 for the large)
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
			if (count >= kind) {
				return true;
			}
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

	//boolean test for a full house (three values are the same and the other two are the same but different to the value of the first three)
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
