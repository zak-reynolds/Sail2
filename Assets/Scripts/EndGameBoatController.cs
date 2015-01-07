using UnityEngine;
using System.Collections;

public class EndGameBoatController : MonoBehaviour {
	
	public float Speed;

	public float sinkSpeed;
	
	public float MaxSpeed;
	public float Acceleration;
	public float Drag;
	public float MaxWaveEmitRate;
	
	public ParticleSystem leftWave;
	public ParticleSystem rightWave;
	
	public Animator boatAnimator;
	public Animator mastAnimator;
	public Animator charAnimator;
	public EndGameCam endCam;
	public AudioSource sfxPirate;

	public bool autoSail;
	public bool sinking;

	public bool dialogStarted;
	public bool dialogFinished;
	
	public float Dialog1Time;
	public float Dialog2Time;
	public float Dialog3Time;

	public GUISkin guiSkin;

	private float timer;
	private int stage = 0;
	
	// Use this for initialization
	void Start () {
		timer = Dialog1Time + Dialog2Time + Dialog3Time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (autoSail)
		{
			Speed += Acceleration * Time.deltaTime;
			mastAnimator.SetBool ("Moving",true);
			
			Speed = Mathf.Clamp (Speed, 0, MaxSpeed);
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

		if (sinking)
		{
			this.transform.position += Vector3.down * Time.deltaTime * sinkSpeed;
		}

		if (transform.position.z > -56 && !dialogStarted)
		{
			autoSail = false;
			dialogStarted = true;
			timer = Dialog1Time + Dialog2Time + Dialog3Time;
			charAnimator.SetTrigger ("Startled");
			sfxPirate.Play ();
		}

		if (dialogStarted)
		{
			timer -= Time.deltaTime;
			if (timer < Dialog2Time + Dialog3Time && stage == 0)
			{
				stage++;
				endCam.SwitchSides = true;
			}
			if (timer < Dialog3Time && stage == 1)
			{
				stage++;
				sfxPirate.Play ();
			}
			if (timer < 0 && stage == 2)
			{
				stage++;
				endCam.SwitchSides = false;
				charAnimator.SetTrigger ("StaySad");
				sinking = true;
				dialogFinished = true;
			}
		}
		if (dialogFinished && transform.position.y < -1.25)
			sinking = false;
	}

	void OnGUI()
	{
		if (dialogStarted)
		{
			switch (stage)
			{
			case 0:
				GUI.Label (new Rect(0, 0, Screen.width, Screen.height), "OY!", guiSkin.customStyles[1]);
				break;
			case 2:
				GUI.Label (new Rect(0, 0, Screen.width, Screen.height), "Yeh, we're taking that stuff brah.", guiSkin.customStyles[1]);
				break;
			default:
				break;
			}
			if (dialogFinished && transform.position.y < -1.25)
			{
				GUI.Label (new Rect(0, 0, Screen.width, Screen.height), "Thanks for playing!\n\nThis game was created for Ludum Dare 29 by Zak Reynolds.", guiSkin.customStyles[1]);
				if (GUI.Button (GuiManager.ConvertScreenToPixel(new Rect (0.25f, 0.8f, 0.5f, 0.1f)), "Sounds good, brah!", guiSkin.button))
					ScreenFadeEffect.StartFadeIn (0);
			}
		}
	}
}
