using UnityEngine;
using System.Collections;

public class AnimatedUVs : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public float sinModifier;
	
	Vector2 uvOffset = Vector2.zero;

	void Update()
	{
		Vector3 pos = transform.position;
		pos.y = sinModifier * Mathf.Sin (Time.timeSinceLevelLoad);
		transform.position = pos;
	}

	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.deltaTime );
		if( renderer.enabled )
		{
			renderer.materials[ materialIndex ].SetTextureOffset( "_MainTex", uvOffset );
		}
	}
}