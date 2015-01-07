using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SailCamera : MonoBehaviour {

	public Transform Follow;
	public Transform Rig;

	public float rotationSpeed;
	public float xRotationSpeed;
	public bool zoomOnChar;
	public bool spyglassView;
	public int zoomIndex;
	public float zoomSpeed;
	public List<Vector3> zoomRotation;
	public List<Vector3> zoomLocalPos;
	public float moveSpeed;

	public Vector3 normalLocalPos;
	public float normalCamXRot;
	public Vector3 spyglassLocalPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rig.position = Follow.position;

		float targetAngle = Follow.rotation.eulerAngles.y;
		float targetAngleX = normalCamXRot;
		float actualRotationSpeed = rotationSpeed;
		if (zoomOnChar)
		{
			targetAngle += zoomRotation[zoomIndex].y;
			targetAngleX += zoomRotation[zoomIndex].x;

			actualRotationSpeed = zoomSpeed;
			
			transform.localPosition = Vector3.Lerp(transform.localPosition, zoomLocalPos[zoomIndex], Time.deltaTime * moveSpeed);
		}
		else if (spyglassView)
		{
			targetAngle = 0;
			targetAngleX = 90;
			transform.localPosition = Vector3.Lerp(transform.localPosition, spyglassLocalPos, Time.deltaTime * moveSpeed);
		}
		else
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, normalLocalPos, Time.deltaTime * moveSpeed);
			// Simple raycast technique to avoid terrain. Should never hit 10000 iterations, but set limit to be safe.
			int i = 10000;
			while (
				Physics.Raycast (transform.position + Vector3.down,
			                 Rig.position - transform.position,
			                 Vector3.Magnitude (Rig.position - transform.position) * 0.5f)
				&& i > 0)
			{
				i--;
				transform.Translate (0, 0.01f, 0);
				transform.LookAt (Rig.position + Vector3.up * 2);
			}
		}

		// Rotate rig (Y)
		Vector3 newRot = new Vector3(
			Rig.rotation.eulerAngles.x,
			Mathf.LerpAngle(Rig.rotation.eulerAngles.y, targetAngle, Time.deltaTime * actualRotationSpeed),
			Rig.rotation.eulerAngles.z);
		Quaternion newQuat = new Quaternion();
		newQuat.eulerAngles = newRot;
		Rig.rotation = newQuat;

		// Rotate camera (X)
		newRot = new Vector3(
			Mathf.LerpAngle (transform.localRotation.eulerAngles.x, targetAngleX, Time.deltaTime * xRotationSpeed),
			0, 0);
		newQuat = new Quaternion();
		newQuat.eulerAngles = newRot;
		transform.localRotation = newQuat;
	}
}
