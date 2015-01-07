using UnityEngine;
using System.Collections;

public class WobbleZRotation : MonoBehaviour {

	public float zRotationAmount;
	public float zTranslateAmount;


	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward, Mathf.Sin ((Time.time + 1)) * zRotationAmount);
		transform.Translate (0, 0, Mathf.Sin ((Time.time + 1)) * zTranslateAmount);
	}
}
