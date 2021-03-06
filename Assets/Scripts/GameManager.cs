using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public BoatController boatController;
	public Animator netAnimator;
	public Animator hookAnimator;
	public Animator charAnimator;
	public GUIText fishCount;
	public SailCamera sailCamera;
	public Transform squidLocation;

	public float NetSpeedModifier;
	public float FishCatchSpeed;
	public float FishGiveSpeed;

	public float NumFish;
	public float NumSquidFish;
	public bool CanUseHook;
	public bool CanUseSpyglass;
	public int SelectedMap;
	public List<bool> obtainedMaps;
	public List<bool> obtainedTreasure;
	public List<bool> obtainedItems;

	private float originalMaxSpeed;
	private Quaternion originalCharRotation;
	private int lastNumFish;

	[HideInInspector]
	public AudioSource sfxSource;

	public AudioClip sfxSquid;
	public AudioClip sfxHookUp;
	public AudioClip sfxHookDown;
	public AudioClip sfxGetNothing;
	public AudioClip sfxGetTreasure;
	public AudioClip sfxGetFish;
	public AudioClip sfxThrowFish;
	public GameObject fishPrefab;

	public float hookSfxVolume;

	public bool joystickEnabled;

	private bool inventoryFlag = false;
	private bool endGameFlag = false;

	public static GameManager GetGM()
	{
		return GameObject.Find ("_GameManager").GetComponent<GameManager>();
	}

	// Use this for initialization
	void Start () {
		originalMaxSpeed = boatController.MaxSpeed;
		originalCharRotation = charAnimator.transform.localRotation;
		if (PlayerPrefs.HasKey ("Joystick"))
			joystickEnabled = PlayerPrefs.GetInt ("Joystick") == 1;
		sfxSource = gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		if (GuiManager.IsInventoryOpen () && !boatController.controlsLocked)
		{
			boatController.controlsLocked = true;
			inventoryFlag = true;
		}
		if (!GuiManager.IsInventoryOpen () && inventoryFlag)
		{
			boatController.controlsLocked = false;
			inventoryFlag = false;
		}


		if (Input.GetButton ("UseNet"))
		{
			if (!boatController.isUsingHook && !boatController.controlsLocked)
			{
				if (!boatController.isInSquidArea)
				{
					boatController.MaxSpeed = originalMaxSpeed * NetSpeedModifier;
					netAnimator.SetBool ("Open", true);
					if (boatController.isFishZone && boatController.Speed > 0)
					{
						lastNumFish = Mathf.FloorToInt (NumFish);
						NumFish += Time.deltaTime * FishCatchSpeed;
						boatController.ContactedFishZone.TakeFish (Time.deltaTime * FishCatchSpeed);
						if (lastNumFish < Mathf.FloorToInt (NumFish))
						{
							sfxSource.PlayOneShot (sfxGetFish, Random.Range (0.8f, 1.2f));
						}
					}
				}
				else
				{
					// Cancel net to be safe
					boatController.MaxSpeed = originalMaxSpeed;
					netAnimator.SetBool ("Open", false);
					charAnimator.transform.localRotation = originalCharRotation;

					if (NumFish - Time.deltaTime * FishGiveSpeed > 0)
					{
						charAnimator.gameObject.transform.LookAt (squidLocation);
						lastNumFish = Mathf.FloorToInt (NumFish);
						NumFish -= Time.deltaTime * FishGiveSpeed;
						NumSquidFish += Time.deltaTime * FishGiveSpeed;
						if (lastNumFish > Mathf.FloorToInt (NumFish))
						{
							Transform charTransform = charAnimator.gameObject.transform;
							GameObject.Instantiate (fishPrefab, charTransform.position + charTransform.up + charTransform.forward, charTransform.rotation);
							sfxSource.PlayOneShot (sfxThrowFish);
						}
						charAnimator.SetBool ("ThrowFish", true);
					}
					else
					{
						charAnimator.SetBool ("ThrowFish", false);
						charAnimator.transform.localRotation = originalCharRotation;
					}
				}
			}
		}
		else
		{
			boatController.MaxSpeed = originalMaxSpeed;
			netAnimator.SetBool ("Open", false);
			charAnimator.SetBool ("ThrowFish", false);
			charAnimator.transform.localRotation = originalCharRotation;
		}

		if (boatController.isInSquidArea)
		{
			if (!boatController.controlsLocked)
			{
				if (NumSquidFish > 1 && !obtainedItems[0])
				{
					CanUseHook = true;
					obtainedItems[0] = true;

					string control = "E";
					if (joystickEnabled)
						control = "X";
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartHook;
					GuiManager.StartDialogue (
						"MNOMNOMNOMNOMNOM.\nYou do good. Have this useless thing.\nMaybe less useless if you press " + control + ".\n50 more fish and squid give really good thing. ",
						"Sure...!",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 50 && !obtainedMaps[0])
				{
					obtainedMaps[0] = true;
					string control = "bottom left";
					if (joystickEnabled)
						control = "press start";
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"SHHHHLLLPTHPTHPTHPTHTP.\nGood good! Have thing now. But want 20 more.\nOpen inventory " + control + ".\nMaybe find better thing with that.",
						"Well I'll try!",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 70 && !obtainedItems[1])
				{
					CanUseSpyglass = true;
					obtainedItems[1] = true;
					string control = "spacebar";
					if (joystickEnabled)
						control = "left bumper";
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartSpyglass;
					GuiManager.StartDialogue (
						"OBLIBLEBELBLETHPTHP.\nThis good good good! Want 30 more!\nTake. Maybe help with other thing.\nMaybe " + control + " help too.",
						"All right!",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 100 && !obtainedMaps[1])
				{
					obtainedMaps[1] = true;
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"MUNCHMUNCHMUNCHGROBLE!\nGOOOOOD! 30 MOAR!\nTake! They say this place shaped\nlike square.",
						"Okaaay...",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 130 && !obtainedMaps[2])
				{
					obtainedMaps[2] = true;
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"CHOMPALOMPALOMP!\nMORE MORE 20 MORE!\nThis place where sky light sleeps.\nBring more fish from there!",
						"Sounds good, yo.",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 150 && !obtainedMaps[3])
				{
					obtainedMaps[3] = true;
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"nom.\nSquid would like more. 20 more.\nSquid cousin get lost in this place.\nSquid no go there.",
						"I will!",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 170 && !obtainedMaps[4])
				{
					obtainedMaps[4] = true;
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"THPNOMGRUBSNTHPHHHPHHTHP!\nBRING FISH GODLY FISH 40!!\nPower of gods rest at this place.\nIt called Tree Horse. It big horse.",
						"I have to go alone?",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
				if (NumSquidFish > 200 && !obtainedMaps[5])
				{
					obtainedMaps[5] = true;
					GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
					endDialogueAction += ControlTips.StartMenu;
					GuiManager.StartDialogue (
						"RRRUHMNUMSTHPNUMNUMNUM!\nSquid full. You best.\nThis last place. It tall.\nWatch out after.",
						"All righty then.",
						endDialogueAction);
					boatController.MoveToSquidDialogPos ();
					sfxSource.PlayOneShot (sfxSquid);
				}
			}
		}
		else
			charAnimator.SetBool ("ThrowFish", false);

		if (Input.GetButtonDown ("UseHook") && CanUseHook && !boatController.controlsLocked)
		{
			boatController.isUsingHook = true;
			hookAnimator.SetBool ("UseHook", true);
			sailCamera.zoomOnChar = true;
			sailCamera.zoomIndex = 0;
			sfxSource.clip = sfxHookDown;
			sfxSource.loop = true;
			sfxSource.volume = hookSfxVolume;
			sfxSource.Play ();
		}

		if (Input.GetButton ("UseSpyglass") && CanUseSpyglass && !boatController.controlsLocked && !boatController.isUsingHook)
			sailCamera.spyglassView = true;
		else
			sailCamera.spyglassView = false;

		if (fishCount != null) fishCount.text = "Fish: " + Mathf.FloorToInt (NumFish).ToString ();

		if (!endGameFlag)
		{
			bool haveEverything = true;
			foreach (bool i in obtainedTreasure)
			{
				if (!i) haveEverything = false;
			}
			if (haveEverything)
			{
				endGameFlag = true;
				ScreenFadeEffect.StartFadeIn (2);
			}
		}
	}

	public void OnInventoryClose()
	{
		//viewManager.RequestOverlayCloseAll ();
	}
	
	public void OpenMap1()
	{
		SelectedMap = 1;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
	
	public void OpenMap2()
	{
		SelectedMap = 2;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
	
	public void OpenMap3()
	{
		SelectedMap = 3;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
	
	public void OpenMap4()
	{
		SelectedMap = 4;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
	
	public void OpenMap5()
	{
		SelectedMap = 5;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
	
	public void OpenMap6()
	{
		SelectedMap = 6;
		//viewManager.RequestOverlayOpen (typeof(MapOverlay));
	}
}
