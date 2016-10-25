using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Spins : MonoBehaviour {
	public float speed = 2;
	public int numOfDice = 1;
	private int savedDice = 0;
	private float xLimit;
	private float zLimit;
	private GameObject dice;
	private GameObject roll;
	private GameManager GO;
	private RollButtonBehavior RBB;
	private Vector3 dicePosition;
	private GameObject[] saves;

	// Use this for initialization
	void Start () {
		dicePosition = new Vector3 (0, 2, -5);
		GO = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		RBB = GameObject.Find ("RollButton").GetComponent<RollButtonBehavior> ();
		xLimit = GameObject.Find ("Floor").transform.localScale.x * 5;
		zLimit = GameObject.Find ("Floor").transform.localScale.z * 5;
		dice = (GameObject.Find ("Dice"));
		dice.SetActive (false);
		saves = new GameObject [5];
		for (int i = 1; i <= 5; i++) {
			saves[i - 1] = (GameObject.Find ("Saved" + i));
			saves [i - 1].SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W) && transform.position.z <= zLimit) {
			transform.Translate (0, 0, speed);
		} else if (Input.GetKey (KeyCode.S) && transform.position.z >= -zLimit) {
			transform.Translate (0, 0, -speed);
		} 
		if (Input.GetKey (KeyCode.A) && transform.position.x >= -xLimit) {
			transform.Translate (-speed, 0, 0);
		} else if (Input.GetKey (KeyCode.D) && transform.position.x <= xLimit) {
			transform.Translate (speed, 0, 0);
		} 
		if (Input.GetKeyDown (KeyCode.H)) {
			Dice (numOfDice);
		}
		if (Input.GetKey (KeyCode.Space)) {
			Application.LoadLevel (Application.loadedLevel);
		} 
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	IEnumerator SummonDice(float num) {
		GameObject.Find ("RollButton").GetComponent<Button> ().enabled = false;
		int i = 0;
		while (i < num) {
			roll = (GameObject)Instantiate (dice, dicePosition, Quaternion.identity);
			roll.SetActive (true);
			roll.GetComponent<Rigidbody> ().AddForce (new Vector3 (Random.Range (-28, 28), 1, Random.Range (25, 50)));
			roll.GetComponent<Rigidbody> ().AddTorque (new Vector3 (Random.Range (-20, 20), Random.Range (-20, 20), Random.Range (-20, 20)));
			i++;
			yield return new WaitForSeconds (.5f);
		}
	}

	void Dice(float num) {
		if (GO.firstTurn || GO.secondTurn || GO.thirdTurn) {
			foreach (GameObject die in GameObject.FindGameObjectsWithTag("Dice")) {
				GO.removeDice (die.GetComponent<DiceBehavior1> ().rolled);
				Destroy (die);
			}
			foreach (GameObject image in GameObject.FindGameObjectsWithTag("ImageHighlight")) {
				saves [(int.Parse (image.name.Substring (5))) - 1].GetComponent<ButtonBehavior> ().Highlight ();
				saves [(int.Parse (image.name.Substring (5))) - 1].SetActive (false);
				GO.removeDice (float.Parse(image.GetComponent<Image>().sprite.name.Substring(7)));
				savedDice--;
			}
			for (int i = 1; i < 5; i++) {
				if(saves[i].activeSelf) {
					int j = i;
					while(j > 0 && !saves[j - 1].activeSelf) {
						GameObject curtemp = saves[j];
						GameObject prevtemp = saves[j - 1];
						prevtemp.SetActive(true);
						prevtemp.GetComponent<Image> ().sprite = curtemp.GetComponent<Image> ().sprite;
						curtemp.SetActive (false);
						j--;
					}
				}
			}
			foreach (GameObject die in GameObject.FindGameObjectsWithTag("Highlight")) {
				saves [savedDice].SetActive(true);
				saves[savedDice].GetComponent<Image> ().sprite = 
				Resources.Load<Sprite> ("Images/DieSide" + (int)die.GetComponent<DiceBehavior1> ().rolled);
				Destroy (die);
				savedDice++;
			}
			StartCoroutine (SummonDice (num - savedDice));
		}
		if (GO.firstTurn) {
			GO.firstTurn = false;
			GO.secondTurn = true;
			RBB.NotFirstTurn ();

		} else if (GO.secondTurn) {
			GO.secondTurn = false;
			GO.thirdTurn = true;
		} else {
			GO.thirdTurn = false;
			GameObject.Find ("RollButton").SetActive(false);
		}
	}

	public void StartSummonDice() {
		Dice (numOfDice);
	}

	public void reset() {
		GO.firstTurn = true;
		GO.secondTurn = false;
		GO.thirdTurn = false;
		for (savedDice = savedDice; savedDice >= 0; savedDice--) {
			saves [savedDice].SetActive (false);
		}
		savedDice = 0;
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Dice")) {
			Destroy (die);
		}
		foreach (GameObject die in GameObject.FindGameObjectsWithTag("Highlight")) {
			Destroy (die);
		}
		GO.dices = new ArrayList ();
		RBB.FirstTurn ();
	}

	public void unsave() {
		float xLocation = Random.Range (-xLimit, xLimit);
		float zLocation = Random.Range (-zLimit, zLimit);
		foreach (GameObject die in GO.dices) {
			if (xLocation <= die.transform.position.x + .5 && xLocation >= die.transform.position.x - .5) {

			}
		}
		Vector3 pos = new Vector3 (xLocation, 1,zLocation);
		Instantiate (dice, dicePosition, Quaternion.identity);
	}
}
