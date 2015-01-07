using UnityEngine;
using System.Collections;

public class ReplaceNewline : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<TextMesh>().text = gameObject.GetComponent<TextMesh>().text.Replace ("\\n", "\n");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
