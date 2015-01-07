using UnityEngine;
using System.Collections;

public class EndGameCam : MonoBehaviour {

	public Transform rigTransform;
	public Transform follow;

	public bool SwitchSides;

	public float frontAngle;
	public float backAngle;

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rigTransform.position = follow.position;

		float targetAngle;

		if (!SwitchSides)
		{
			targetAngle = frontAngle;
		}
		else
		{
			targetAngle = backAngle;
		}
		
		Vector3 newRot = new Vector3(
			rigTransform.rotation.eulerAngles.x,
			Mathf.LerpAngle(rigTransform.rotation.eulerAngles.y, targetAngle, Time.deltaTime * rotationSpeed),
			rigTransform.rotation.eulerAngles.z);
		Quaternion newQuat = new Quaternion();
		newQuat.eulerAngles = newRot;
		rigTransform.rotation = newQuat;
	}
}
