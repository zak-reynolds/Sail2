using UnityEngine;
using System.Collections;

public class GullLogic : MonoBehaviour {

	public float rotateSpeed;
	public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0, Mathf.Sin (Time.time) * 0.05f, moveSpeed * Time.deltaTime);
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
}
