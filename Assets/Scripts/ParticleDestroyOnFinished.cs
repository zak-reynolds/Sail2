using UnityEngine;
using System.Collections;

public class ParticleDestroyOnFinished : MonoBehaviour {

	private ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = (ParticleSystem)gameObject.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!ps.IsAlive ())
						Destroy (gameObject);
	}
}
