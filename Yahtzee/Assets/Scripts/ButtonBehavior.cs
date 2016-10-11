using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Highlight() {
		if (!this.gameObject.GetComponent<Image> ().color.Equals (Color.gray)) {
			this.gameObject.tag = "ImageHighlight";
			this.gameObject.GetComponent<Image> ().color = Color.gray;
		} else {
			this.gameObject.tag = "Image";
			this.gameObject.GetComponent<Image> ().color = Color.white;
		}
	}
}
