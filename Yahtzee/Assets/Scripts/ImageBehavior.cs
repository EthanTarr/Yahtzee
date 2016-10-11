using UnityEngine;
using System.Collections;

public class ImageBehavior : MonoBehaviour {
	private bool check = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		// This section gets the RectTransform information from this object. Height and width are stored in variables. The borders of the object are also defined
		RectTransform objectRectTransform = gameObject.GetComponent<RectTransform> ();     
		float width = objectRectTransform.rect.width;
		float height = objectRectTransform.rect.height;
		float rightOuterBorder = (width * .5f);
		float leftOuterBorder = (width * -.5f);
		float topOuterBorder = (height * .5f);
		float bottomOuterBorder = (height * -.5f);
		// The following line determines if the cursor is on the object
		if (Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder)
		   && Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder)
		   && !check) {
			Debug.Log ("worked");
			check = true;
		} else if(!(Input.mousePosition.x <= (transform.position.x + rightOuterBorder) && Input.mousePosition.x >= (transform.position.x + leftOuterBorder)
			&& Input.mousePosition.y <= (transform.position.y + topOuterBorder) && Input.mousePosition.y >= (transform.position.y + bottomOuterBorder)) && check) {
			check = false;
		}
	}
}
