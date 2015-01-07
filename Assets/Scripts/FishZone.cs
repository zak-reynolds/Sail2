using UnityEngine;
using System.Collections;

public class FishZone : MonoBehaviour {

	public BoatController boatController;

	public float MaxFish;
	public float FishGiven;

	void Start()
	{
		boatController = GameManager.GetGM ().boatController;
	}

	public void TakeFish(float Amount)
	{
		FishGiven += Amount;
		if (FishGiven > MaxFish)
		{
			boatController.isFishZone = false;
			boatController.ContactedFishZone = null;
			GameObject.Destroy (this.gameObject);
		}
	}
}
