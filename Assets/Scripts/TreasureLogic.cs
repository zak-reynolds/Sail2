using UnityEngine;
using System.Collections;

public class TreasureLogic : MonoBehaviour {

	public int TreasureIndex;

	void OnDestroy()
	{
		if (GameObject.FindGameObjectWithTag ("GameManager") != null)
			GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager>().obtainedTreasure[TreasureIndex] = true;
	}
}
