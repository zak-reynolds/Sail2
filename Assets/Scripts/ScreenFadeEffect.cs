using UnityEngine;
using System.Collections;

public class ScreenFadeEffect : MonoBehaviour {

	public Texture fadeTexture;

	public float fadeTime;

	private float fadeTimer;
	private bool fadeTextureIn;
	private float textureAlpha;
	private int levelToLoad;

	// Use this for initialization
	void Start () {
		fadeTextureIn = false;
		textureAlpha = 1;
		fadeTimer = fadeTime;
		levelToLoad = -1;
	}

	void Update() {
		if (fadeTimer > 0)
		{
			fadeTimer -= Time.deltaTime;
			if (fadeTextureIn)
				textureAlpha = Mathf.Lerp (1, 0, fadeTimer / fadeTime);
			else
				textureAlpha = Mathf.Lerp (0, 1, fadeTimer / fadeTime);
		}
		else if (fadeTextureIn && levelToLoad > -1)
		{
			Application.LoadLevel (levelToLoad);
		}
	}

	void OnGUI () {
		if (textureAlpha > 0)
		{
			Color originalColor = GUI.color;
			GUI.color = new Color(1, 1, 1, textureAlpha);
			GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
			GUI.color = originalColor;
		}
	}

	public static void StartFadeIn(int LevelToLoad)
	{
		ScreenFadeEffect sfe = GameObject.Find ("_ScreenFadeEffect").GetComponent<ScreenFadeEffect>();
		if (!sfe.fadeTextureIn)
		{
			sfe.fadeTextureIn = true;
			sfe.fadeTimer = sfe.fadeTime;
			sfe.levelToLoad = LevelToLoad;
		}
	}
}
