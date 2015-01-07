using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// GUI manager.
/// "Dialogue" refers to a conversational text box, containing text and a close button.
/// </summary>
public class GuiManager : MonoBehaviour {
	
	public delegate void ComponentAction();

	[SerializeField]
	private GUISkin guiSkin;

	[SerializeField]
	private float dialogueBackgroundYScreen;
	[SerializeField]
	private Texture dialogueBackgroundTex;
	[SerializeField]
	private Rect dialogueButtonRectScreen;

	[SerializeField]
	private float labelFontLinearM;
	[SerializeField]
	private float labelFontLinearB;
	[SerializeField]
	private float buttonFontRelativeSize;

	[SerializeField]
	private Rect inventoryButtonRectScreen;

	[SerializeField]
	private Texture inventoryBackgroundTex;
	[SerializeField]
	private Texture inventorySpaceTex;
	[SerializeField]
	private Rect inventoryBackgroundRectScreen;
	[SerializeField]
	private Rect inventoryXButtonRectScreen;
	[SerializeField]
	private List<string> inventoryGroupLabelTexts;
	[SerializeField]
	private List<Rect> inventoryGroupLabelRectsScreen;
	[SerializeField]
	private List<Rect> inventorySpaceRectsScreen;
	[SerializeField]
	private Texture inventoryMapTex;
	[SerializeField]
	private Texture inventoryTreasureTex;
	[SerializeField]
	private List<Texture> inventoryItemTex;

	[SerializeField]
	private Rect mapBackgroundRectScreen;
	[SerializeField]
	private Texture mapBackgroundTex;
	[SerializeField]
	private Rect mapXButtonRectScreen;
	[SerializeField]
	private List<Texture> mapTextures;

	private float dialogueBackgroundY;
	private Rect dialogueButtonRect;
	private Rect inventoryButtonRect;
	private Rect inventoryBackgroundRect;
	private Rect inventoryXButtonRect;
	private List<Rect> inventoryGroupLabelRects = new List<Rect>();
	private List<Rect> inventorySpaceRects = new List<Rect>();
	private Rect mapBackgroundRect;
	private Rect mapXButtonRect;

	public bool hudEnabled;

	private bool joystickEnabled;
	public int joystickSelectIndex;
	[SerializeField]
	private Texture joystickSelectTex;
	[SerializeField]
	private float joystickDelayTime;

	/// <summary>
	/// This contains the dialogue (0) and button text (0). Set internally.
	/// </summary>
	private string[] dialogue = { "", "" };

	/// <summary>
	/// The end dialogue actions. Set internally.
	/// </summary>
	private ComponentAction endDialogueAction;

	/// <summary>
	/// Whether a dialogue is open or not.
	/// </summary>
	private bool dialogueOpen;

	private enum HudState { Closed, InventoryScreen, MapOverlay }

	private HudState hudState = HudState.Closed;

	public static bool IsInventoryOpen() {
		return GameObject.Find ("_GuiManager").GetComponent<GuiManager>().hudState == HudState.InventoryScreen;
	}

	private int selectedMap;

	private bool guiOpenInvPressed;
	private bool guiSelectPressed;
	private float joystickLastMovementTime = 0;


	public static void StartDialogue(string DialogueText, string ButtonText, ComponentAction EndDialogueAction)
	{
		// Find the GUIManager instance, set the dialogue parameters, and open the dialogue
		GuiManager guiManager = GameObject.Find ("_GuiManager").GetComponent<GuiManager>();
		guiManager.dialogue[0] = DialogueText;
		guiManager.dialogue[1] = ButtonText;
		guiManager.endDialogueAction = EndDialogueAction;
		guiManager.dialogueOpen = true;
	}

	public static void StartDialogue(string DialogueText, string ButtonText)
	{
		StartDialogue (DialogueText, ButtonText, null);
	}

	// Use this for initialization
	void Start () {
		joystickEnabled = PlayerPrefs.GetInt ("Joystick") == 1;
	}

	void Update () {
		if (joystickEnabled)
		{
			// This is needed so we can read joystick input in OnGUI()
			guiOpenInvPressed = Input.GetButtonDown ("GUIOpenInventory");
			guiSelectPressed = Input.GetButtonDown ("GUISelect");

			if (hudState == HudState.InventoryScreen)
			{
				// Change selection based on input. This is arbitrary, based on the way
				// the default tiles are set up in the inventory. This would need to be
				// rewritten whenever a change is made to the inventory layout.
				if (Time.time > joystickLastMovementTime + joystickDelayTime)
				{
					// Joystick right
					if (Input.GetAxis ("Horizontal") > 0)
					{
						// Most odd numbered indices are on the edge of a group
						if (joystickSelectIndex % 2 == 1)
						{
							if (joystickSelectIndex == 13)
								joystickSelectIndex = 2;
							else
								joystickSelectIndex += 5;
						}
						else
						{
							if (joystickSelectIndex == 12)
								joystickSelectIndex = 0;
							else
								joystickSelectIndex += 1;
						}
						
						joystickLastMovementTime = Time.time;
					}
					// Joystick left
					if (Input.GetAxis ("Horizontal") < 0)
					{
						// Most even numbered indices are on the edge of a group
						if (joystickSelectIndex % 2 == 0)
						{
							if (joystickSelectIndex == 0)
								joystickSelectIndex = 12;
							else if (joystickSelectIndex == 2)
								joystickSelectIndex = 13;
							else if (joystickSelectIndex == 4)
								joystickSelectIndex = 11;
							else
								joystickSelectIndex -= 5;
						}
						else
						{
							if (joystickSelectIndex == 13)
								joystickSelectIndex = 9;
							else
								joystickSelectIndex -= 1;
						}
						
						joystickLastMovementTime = Time.time;
					}
					// Joystick up
					if (Input.GetAxis ("GUIVertical") > 0)
					{
						List<int> specialIndices = new List<int>() { 0, 1, 6, 7 };
						if (joystickSelectIndex == 12)
							joystickSelectIndex = 13;
						else if (joystickSelectIndex == 13)
							joystickSelectIndex = 12;
						else if (specialIndices.Contains (joystickSelectIndex))
							joystickSelectIndex += 4;
						else
							joystickSelectIndex -= 2;
						
						joystickLastMovementTime = Time.time;
					}
					// Joystick down
					if (Input.GetAxis ("GUIVertical") < 0)
					{
						List<int> specialIndices = new List<int>() { 4, 5, 10, 11 };
						if (joystickSelectIndex == 12)
							joystickSelectIndex = 13;
						else if (joystickSelectIndex == 13)
							joystickSelectIndex = 12;
						else if (specialIndices.Contains (joystickSelectIndex))
							joystickSelectIndex -= 4;
						else
							joystickSelectIndex += 2;
						
						joystickLastMovementTime = Time.time;
					}
				}
			}
		}
	}

	void OnGUI() {

		// Start by updating rects from screen to pixel coordinates
		// (this is done per frame to account for screen size changes)
		dialogueBackgroundY = dialogueBackgroundYScreen * Screen.height;
		dialogueButtonRect = GuiManager.ConvertScreenToPixel (dialogueButtonRectScreen);
		inventoryButtonRect = GuiManager.ConvertScreenToPixel (inventoryButtonRectScreen);
		inventoryBackgroundRect = GuiManager.ConvertScreenToPixel (inventoryBackgroundRectScreen);
		inventoryXButtonRect = GuiManager.ConvertScreenToPixel (inventoryXButtonRectScreen);
		inventoryGroupLabelRects.Clear ();
		foreach (Rect rect in inventoryGroupLabelRectsScreen)
		{
			inventoryGroupLabelRects.Add (GuiManager.ConvertScreenToPixel (rect));
		}
		inventorySpaceRects.Clear ();
		foreach (Rect rect in inventorySpaceRectsScreen)
		{
			inventorySpaceRects.Add (GuiManager.ConvertScreenToPixel (rect));
		}
		mapBackgroundRect = GuiManager.ConvertScreenToPixel (mapBackgroundRectScreen);
		mapXButtonRect = GuiManager.ConvertScreenToPixel (mapXButtonRectScreen);

		// Update font sizes (the default slope of the model was found by doing linear regression
		// on sampled screen sizes and proper font sizes for those screen sizes)
		guiSkin.label.fontSize = Mathf.FloorToInt (labelFontLinearM * Screen.width + labelFontLinearB);
		guiSkin.customStyles[0].fontSize = Mathf.FloorToInt (labelFontLinearM * Screen.width + labelFontLinearB);
		// Make the button font slightly smaller than the label font
		guiSkin.button.fontSize = Mathf.FloorToInt ((labelFontLinearM * Screen.width - labelFontLinearB) * buttonFontRelativeSize);

		if (dialogueOpen)
		{
			if (DrawDialogue())
			{
				// Close the dialogue and broadcast the actions
				dialogueOpen = false;
				if (endDialogueAction != null)
				{
					endDialogueAction();
				}
			}
		}
		else
		{
			// We want to disable the HUD during dialog
			if (hudEnabled)
			{
				// Draw fish counter

				// Draw other elements based on state
				switch (hudState)
				{
				case HudState.Closed:
					if (GUI.Button (inventoryButtonRect, "---", guiSkin.button) ||
					    (joystickEnabled && guiOpenInvPressed))
					{
						hudState = HudState.InventoryScreen;
						guiOpenInvPressed = false;
					}
					break;
				case HudState.InventoryScreen:
					// Draw background
					GUI.DrawTexture (inventoryBackgroundRect, inventoryBackgroundTex);
					// Draw X button and check for input
					if (GUI.Button (inventoryXButtonRect, "X", guiSkin.button) ||
					    (joystickEnabled && guiOpenInvPressed))
					{
						hudState = HudState.Closed;
						guiOpenInvPressed = false;
					}
					// Draw group labels
					int i = 0;
					foreach (Rect rect in inventoryGroupLabelRects)
					{
						GUI.Label (rect, inventoryGroupLabelTexts[i], guiSkin.label);
						i++;
					}
					// Draw spaces
					i = 0;
					foreach (Rect space in inventorySpaceRects)
					{
						GUI.DrawTexture (space, inventorySpaceTex);

						if (i < 6)
						{
							// If we're in the maps section
							if (GameManager.GetGM ().obtainedMaps[i])
							{
								if (GUI.Button (space, inventoryMapTex, guiSkin.button) ||
								    (joystickEnabled && guiSelectPressed && i == joystickSelectIndex))
								{
									selectedMap = i;
									hudState = HudState.MapOverlay;
								}
							}
						}
						else if (i < 12)
						{
							// If we're in the treasure section
							if (GameManager.GetGM ().obtainedTreasure[i - 6])
							{
								if (GUI.Button (space, inventoryTreasureTex, guiSkin.button) ||
								    (joystickEnabled && guiSelectPressed && i == joystickSelectIndex))
								{
									// Show treasure overlay or something
								}
							}
						}
						else
						{
							// If we're in the items section
							if (GameManager.GetGM ().obtainedItems[i - 12])
							{
								if (GUI.Button (space, inventoryItemTex[i - 12], guiSkin.button) ||
								    (joystickEnabled && guiSelectPressed && i == joystickSelectIndex))
								{
									// Show description of item or something
								}
							}
						}
						// Draw selection reticle (if joystick control is enabled)
						if (i == joystickSelectIndex && joystickEnabled)
						{
							GUI.DrawTexture (space, joystickSelectTex);
						}
						i++;
					}
					break;
				case HudState.MapOverlay:
					// Draw background
					GUI.DrawTexture (mapBackgroundRect, mapBackgroundTex);
					// Draw X button and check for input
					if (GUI.Button (mapXButtonRect, "X", guiSkin.button) ||
					    (joystickEnabled && guiOpenInvPressed))
					{
						hudState = HudState.Closed;
						guiOpenInvPressed = false;
					}
					// Draw map
					GUI.DrawTexture (mapBackgroundRect, mapTextures[selectedMap]);
					break;
				}
			}
		}
	}

	private bool DrawDialogue()
	{
		Vector2 boxSize = guiSkin.customStyles[0].CalcSize(new GUIContent(dialogue[0]));
		Rect drawBox = new Rect(Screen.width - boxSize.x, dialogueBackgroundY, boxSize.x, boxSize.y);
		GUI.DrawTexture (drawBox, dialogueBackgroundTex);
		GUI.Label (drawBox, dialogue[0], guiSkin.customStyles[0]);
		return (GUI.Button (dialogueButtonRect, dialogue[1], guiSkin.button) ||
		        (joystickEnabled && guiSelectPressed));
	}

	public static Rect ConvertScreenToPixel(Rect ScreenRect)
	{
		return new Rect(
			ScreenRect.x * Screen.width,
			ScreenRect.y * Screen.height,
			ScreenRect.width * Screen.width,
			ScreenRect.height * Screen.height);
	}
}
