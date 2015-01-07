using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementDetector : MonoBehaviour {

	[SerializeField]
	private BoatController boatController;

	private bool squidFishTipEnabled = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "SquidArea")
		{
			boatController.isInSquidArea = true;
			if (squidFishTipEnabled)
				ControlTips.StartFish ();
		}
		else if (other.tag == "SquidTrigger")
		{
			GameManager.GetGM ().squidLocation = 
			((GameObject)
				 (GameObject.Instantiate (boatController.squid, other.transform.position, other.transform.rotation))
				 ).transform;
			GameObject.Destroy (other.gameObject);
			boatController.MoveToSquidDialogPos ();
			boatController.gameManager.charAnimator.SetTrigger ("Startled");

			GuiManager.ComponentAction endDialogueAction = boatController.OnSquidTalkEnd;
			endDialogueAction += PlayBasicTip;
			GuiManager.StartDialogue (
				"Squid hungry.\nSquid like fish. Bird like fish too.\nSquid give good thing for fish.\nBring squid 1 fish from under bird.",
				"Okay!",
				endDialogueAction);
			boatController.autoSail = false;
			boatController.gameManager.sfxSource.clip = boatController.gameManager.sfxSquid;
			boatController.gameManager.sfxSource.Play ();
			ControlTips.StartDialogue ();
		}
		else if (other.tag != "Detector" && other.tag != "Treasure" && other.tag != "FishZone")
		{
			if (boatController.Speed > 0) audio.Play ();
			boatController.isColliding = true;
			boatController.Speed = 0;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag != "FishZone")
		{
			if (other.tag == "SquidArea")
			{
				boatController.isInSquidArea = false;
				if (ControlTips.GetState () == ControlTips.State.Fish)
					ControlTips.EndTip ();
			}
			else
			{
				boatController.isColliding = false;
			}
		}
	}

	public void PlayBasicTip()
	{
		ControlTips.StartBasic ();
		squidFishTipEnabled = true;
	}

}
