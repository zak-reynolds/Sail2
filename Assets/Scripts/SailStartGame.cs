using UnityEngine;
using System.Collections;

public class SailStartGame : MonoBehaviour {

	public Animator sailAnimator;
	public GameObject keyboardSelectEffect;
	public GameObject joystickSelectEffect;

	public AudioClip menuMusic;
	public AudioClip menuChime;
	public float startDelayTime;
	public float optionsMenuCamY;

	private AudioSource musicSource;
	private AudioSource sfxSource;
	private float nextLevelTimer = -1;
	private Transform cameraTransform;

	void Start()
	{
		sailAnimator.SetBool ("Moving", true);

		if (!PlayerPrefs.HasKey ("MusicVolume"))
			PlayerPrefs.SetFloat ("MusicVolume", 0.37f);
		musicSource = gameObject.AddComponent<AudioSource>();
		musicSource.clip = menuMusic;
		musicSource.volume = PlayerPrefs.GetFloat ("MusicVolume");
		musicSource.loop = true;
		musicSource.Play ();

		sfxSource = gameObject.AddComponent<AudioSource>();
		sfxSource.clip = menuChime;

		cameraTransform = GameObject.FindWithTag ("MainCamera").GetComponent<Transform>();
	}

	void Update()
	{
		if (nextLevelTimer == -1)
		{
			if (Input.GetButtonDown ("UseSpyglass"))
			{
				PlayerPrefs.SetInt ("Joystick", 0);
				PlayerPrefs.Save ();
				nextLevelTimer = startDelayTime;
				sfxSource.Play ();
				GameObject.Instantiate (
					keyboardSelectEffect,
					GameObject.Find ("KeyboardMenuIcon").transform.position,
					keyboardSelectEffect.transform.rotation);
				ScreenFadeEffect.StartFadeIn (1);
			}
			if (Input.GetButtonDown ("GUISelect"))
			{
				PlayerPrefs.SetInt ("Joystick", 1);
				PlayerPrefs.Save ();
				nextLevelTimer = startDelayTime;
				sfxSource.Play ();
				GameObject.Instantiate (
					joystickSelectEffect,
					GameObject.Find ("JoystickMenuIcon").transform.position,
					joystickSelectEffect.transform.rotation);
				ScreenFadeEffect.StartFadeIn (1);
			}
		}
		else
		{
			if (nextLevelTimer > 0)
			{
				nextLevelTimer -= Time.deltaTime;
				musicSource.volume = (nextLevelTimer / startDelayTime) * (1.0f / PlayerPrefs.GetFloat ("MusicVolume"));


				//cameraTransform.Rotate (Vector3.up, nextLevelTimer / startDelayTime * optionsMenuCamY, Space.World);
			}
		}
	}
}
