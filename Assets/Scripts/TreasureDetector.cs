using UnityEngine;
using System.Collections;

public class TreasureDetector : MonoBehaviour {

	[SerializeField]
	private BoatController boatController;
	
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Treasure")
			boatController.ContactedTreasure = other.gameObject;
	}
	
	void OnTriggerExit (Collider other) {
		if (other.tag == "Treasure")
			boatController.ContactedTreasure = null;
	}
}
