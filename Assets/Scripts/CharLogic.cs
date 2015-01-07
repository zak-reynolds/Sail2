using UnityEngine;
using System.Collections;

public class CharLogic : MonoBehaviour {
	
	void OnReactionEnd()
	{
		if (GameManager.GetGM ().boatController.isUsingHook)
		{
			GameManager.GetGM ().sailCamera.zoomOnChar = false;
			GameManager.GetGM ().boatController.isUsingHook = false;
		}
	}
}
