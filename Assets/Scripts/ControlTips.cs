using UnityEngine;
using System.Collections;

public class ControlTips : MonoBehaviour {
	
	[SerializeField]
	private GameObject keyBasicsTip;
	[SerializeField]
	private GameObject keyFishTip;
	[SerializeField]
	private GameObject keyHookTip;
	[SerializeField]
	private GameObject keyMenuTip;
	[SerializeField]
	private GameObject keySpyglassTip;
	[SerializeField]
	private GameObject joyBasicsTip;
	[SerializeField]
	private GameObject joyFishTip;
	[SerializeField]
	private GameObject joyHookTip;
	[SerializeField]
	private GameObject joyMenuTip;
	[SerializeField]
	private GameObject joySpyglassTip;
	[SerializeField]
	private GameObject joyDialogueTip;

	private GameObject currentTip;

	public enum State { None, Basics, Fish, Hook, Menu, Spyglass, Dialogue }
	private State currentState;
	private bool joystick;
	[SerializeField]
	private float basicTipTimer;

	void Start()
	{
		joystick = GameManager.GetGM ().joystickEnabled;
	}

	void Update()
	{
		switch (currentState)
		{
		case State.None:
			break;
		case State.Basics:
			basicTipTimer -= Time.deltaTime;
			if (basicTipTimer < 0)
			{
				EndTip ();
				StartFish ();
			}
			break;
		case State.Fish:
			if (Input.GetButton ("UseNet"))
			    EndTip ();
			break;
		case State.Hook:
			if (Input.GetButton ("UseHook"))
			    EndTip ();
			break;
		case State.Menu:
			if (GuiManager.IsInventoryOpen ())
			    EndTip ();
			break;
		case State.Spyglass:
			if (Input.GetButton ("UseSpyglass"))
			    EndTip ();
			break;
		case State.Dialogue:
			if (Input.GetButton ("GUISelect"))
				EndTip ();
			break;
		}
	}

	private static ControlTips GetInstance()
	{
		return GameObject.Find ("_ControlTips").GetComponent<ControlTips>();
	}

	public static void StartBasic()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();

		Transform boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
		
		if (ct.joystick)
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joyBasicsTip, boatTransform.position, boatTransform.rotation);
		else
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.keyBasicsTip, boatTransform.position, boatTransform.rotation);
		
		ct.currentTip.transform.parent = boatTransform;
		ct.currentState = State.Basics;
	}

	public static void StartFish()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();

		Transform boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
		
		if (ct.joystick)
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joyFishTip, boatTransform.position, boatTransform.rotation);
		else
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.keyFishTip, boatTransform.position, boatTransform.rotation);
		
		ct.currentTip.transform.parent = boatTransform;
		ct.currentState = State.Fish;
	}

	public static void StartHook()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();

		Transform boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
		
		if (ct.joystick)
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joyHookTip, boatTransform.position, boatTransform.rotation);
		else
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.keyHookTip, boatTransform.position, boatTransform.rotation);
		
		ct.currentTip.transform.parent = boatTransform;
		ct.currentState = State.Hook;
	}

	public static void StartMenu()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();
		
		Transform targetTransform;
		
		if (ct.joystick)
		{
			targetTransform = GameObject.FindGameObjectWithTag("Boat").transform;
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joyMenuTip, targetTransform.position, targetTransform.rotation);
		}
		else
		{
			targetTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.keyMenuTip, targetTransform.position, targetTransform.rotation);
		}
		
		ct.currentTip.transform.parent = targetTransform;
		ct.currentState = State.Menu;
	}

	public static void StartSpyglass()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();
		
		Transform boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
		
		if (ct.joystick)
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joySpyglassTip, boatTransform.position, boatTransform.rotation);
		else
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.keySpyglassTip, boatTransform.position, boatTransform.rotation);
		
		ct.currentTip.transform.parent = boatTransform;
		ct.currentState = State.Spyglass;
	}
	
	public static void StartDialogue()
	{
		ControlTips ct = GetInstance ();
		if (ct.currentTip)
			EndTip ();
		
		Transform boatTransform = GameObject.FindGameObjectWithTag("Boat").transform;
		
		if (ct.joystick)
		{
			ct.currentTip = (GameObject) GameObject.Instantiate (ct.joyDialogueTip, boatTransform.position, boatTransform.rotation);
		
			ct.currentTip.transform.parent = boatTransform;
			ct.currentState = State.Dialogue;
		}
	}

	public static void EndTip()
	{
		if (GetInstance ().currentTip)
			Destroy (GetInstance ().currentTip);
		else
			Debug.Log ("Don't end a tip that doesn't exist!");
		GetInstance ().currentState = State.None;
	}

	public static State GetState()
	{
		return GetInstance().currentState;
	}

}
