using UnityEngine;
using System.Collections;

public class SmallFishToss : MonoBehaviour {

	public Vector3 startForce;
	public Vector3 startTorque;
	public Vector3 randomForce;
	public Vector3 randomTorque;

	public GameObject splashEffect;

	private float timer;
	private GameObject splashEffectInstance;
	private bool createdSplash;
	private Transform waterLevel;

	// Use this for initialization
	void Start () {
		Vector3 actualForce;
		actualForce.x = Random.Range (startForce.x - randomForce.x, startForce.x + randomForce.x);
		actualForce.y = Random.Range (startForce.y - randomForce.y, startForce.y + randomForce.y);
		actualForce.z = Random.Range (startForce.z - randomForce.z, startForce.z + randomForce.z);
		Vector3 actualTorque;
		actualTorque.x = Random.Range (startTorque.x - randomForce.x, startTorque.x + randomForce.x);
		actualTorque.y = Random.Range (startTorque.y - randomForce.y, startTorque.y + randomForce.y);
		actualTorque.z = Random.Range (startTorque.z - randomForce.z, startTorque.z + randomForce.z);


		rigidbody.AddRelativeForce (actualForce, ForceMode.Impulse);
		rigidbody.AddRelativeTorque (actualTorque, ForceMode.Impulse);

		waterLevel = GameObject.Find ("Water").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < waterLevel.position.y + 0.1f && !createdSplash)
		{
			createdSplash = true;
			splashEffectInstance = (GameObject) GameObject.Instantiate (splashEffect, transform.position, Quaternion.identity);
			splashEffectInstance.transform.parent = waterLevel;
			audio.Play ();
		}
		timer += Time.deltaTime;
		if (timer > 5)
		{
			GameObject.Destroy (gameObject);
			if (splashEffectInstance)
				GameObject.Destroy (splashEffectInstance);
		}
	}
}
