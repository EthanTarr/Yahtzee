using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour {

	private Color Normal;
	private Color Highlighted;

	// Use this for initialization
	void Start () {
		Normal = this.GetComponent<Image> ().color;
		Highlighted = new Color(255, 255, 255, 100/255f);
	}

	public void Highlight() {
		if (!this.gameObject.GetComponent<Image> ().color.Equals (Highlighted)) {
			this.gameObject.tag = "ImageHighlight";
			this.gameObject.GetComponent<Image> ().color = Highlighted;
		} else {
			this.gameObject.tag = "Image";
			this.gameObject.GetComponent<Image> ().color = Normal;
		}
	}
}
