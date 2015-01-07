using UnityEngine;
using System.Collections;

public class FishDetector : MonoBehaviour {

	[SerializeField]
	private BoatController boatController;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "FishZone")
		{
			boatController.isFishZone = true;
			boatController.ContactedFishZone = (FishZone)other.GetComponent<FishZone>();
		}
	}
	
	public void OnTriggerExit(Collider other)
	{
		if (other.tag == "FishZone")
		{
			boatController.isFishZone = false;
			boatController.ContactedFishZone = null;
		}
	}
}
