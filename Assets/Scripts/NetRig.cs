using UnityEngine;
using System.Collections;

public class NetRig : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.GetComponent<Animator>().SetBool ("Open", GameManager.GetGM ().netAnimator.GetBool ("Open"));
	}
}
