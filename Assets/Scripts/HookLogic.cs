using UnityEngine;
using System.Collections;

public class HookLogic : MonoBehaviour {

	public GameManager gameManager;
	public GameObject hookBone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnHookGrab()
	{
		if (gameManager.boatController.ContactedTreasure != null)
		{
			gameManager.boatController.ContactedTreasure.transform.parent = hookBone.transform;
			gameManager.boatController.ContactedTreasure.transform.localPosition = Vector3.zero;
			gameManager.boatController.ContactedTreasure.collider.enabled = false;
			gameManager.sailCamera.zoomIndex = 1;
		}
		gameManager.charAnimator.SetTrigger ("Startled");
		gameManager.sfxSource.clip = gameManager.sfxHookUp;
		gameManager.sfxSource.Play ();
	}

	void OnHookFinished()
	{
		gameObject.GetComponent<Animator>().SetBool ("UseHook", false);
		if (gameManager.boatController.ContactedTreasure != null)
		{
			gameManager.charAnimator.SetTrigger ("Happy");
			gameManager.sfxSource.PlayOneShot (gameManager.sfxGetTreasure);
			GameObject.Destroy (gameManager.boatController.ContactedTreasure);
			gameManager.boatController.ContactedTreasure = null;
		}
		else
		{
			gameManager.charAnimator.SetTrigger ("Sad");
			gameManager.sfxSource.PlayOneShot (gameManager.sfxGetNothing);
		}
		gameManager.sfxSource.volume = 1;
		gameManager.sfxSource.loop = false;
	}
}
