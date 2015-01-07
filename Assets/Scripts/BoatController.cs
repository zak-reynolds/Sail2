using UnityEngine;
using System.Collections;

public class BoatController : MonoBehaviour {

	public float Speed;
	public Vector3 Direction;

	public float MaxSpeed;
	public float Acceleration;
	public float Drag;
	public float TurnSpeed;
	public float MaxWaveEmitRate;

	public ParticleSystem leftWave;
	public ParticleSystem rightWave;
	public GameObject squid;

	public Animator boatAnimator;
	public Animator mastAnimator;
	public GameManager gameManager;
	public Transform globalWater;

	public bool isColliding;
	public bool isFishZone;
	public bool isInSquidArea;
	public bool isUsingHook;
	public bool controlsLocked;
	public bool autoSail;
	
	public GameObject ContactedTreasure;
	public FishZone ContactedFishZone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!isUsingHook && !controlsLocked)
		{
			if (Input.GetAxis("Vertical") >= 0 || autoSail)
			{
				if (Input.GetAxis ("Vertical") > 0 || autoSail)
				{
					Speed += Acceleration * Time.deltaTime;
					mastAnimator.SetBool ("Moving",true);
				}
				else
				{
					Speed -= Drag * Time.deltaTime;
					mastAnimator.SetBool ("Moving", false);
				}
			}
			else
			{
				Speed -= Acceleration * Time.deltaTime;
			}
			
			Speed = Mathf.Clamp (Speed, 0, MaxSpeed);
			if (Input.GetAxis ("Horizontal") < 0 && !autoSail)
			{
				this.transform.Rotate (Vector3.up, -TurnSpeed * Time.deltaTime);
			}
			if (Input.GetAxis ("Horizontal") > 0 && !autoSail)
			{
				this.transform.Rotate (Vector3.up, TurnSpeed * Time.deltaTime);
			}

			if (!isColliding)
				this.transform.position += this.transform.forward * Speed * Time.deltaTime;
		}
		else
		{
			Speed = 0;
			mastAnimator.SetBool ("Moving", false);
		}

		// Effects
		
		leftWave.emissionRate = MaxWaveEmitRate * (Speed / MaxSpeed);
		rightWave.emissionRate = MaxWaveEmitRate * (Speed / MaxSpeed);
		leftWave.startSpeed = leftWave.emissionRate / 2;
		rightWave.startSpeed = rightWave.emissionRate / 2;

		if (Speed > 0)
		{
			boatAnimator.SetBool ("Moving", true);
		}
		else
		{
			boatAnimator.SetBool ("Moving", false);
		}

		this.transform.position = new Vector3(this.transform.position.x, globalWater.position.y, this.transform.position.z);
	}

	public void OnSquidTalkEnd()
	{
		gameManager.sfxSource.PlayOneShot (gameManager.sfxSquid);
		gameManager.sailCamera.zoomOnChar = false;
		controlsLocked = false;
	}

	public void MoveToSquidDialogPos()
	{
		controlsLocked = true;
		gameManager.sailCamera.zoomOnChar = true;
		gameManager.sailCamera.zoomIndex = 2;
		Vector3 newPos = transform.position;
		newPos.x = 0;
		newPos.z = 12;
		transform.position = newPos;
		Vector3 newRot = new Vector3(
			transform.rotation.eulerAngles.x,
			310,
			transform.rotation.eulerAngles.z);
		Quaternion newQuat = new Quaternion();
		newQuat.eulerAngles = newRot;
		transform.rotation = newQuat;
	}
}
