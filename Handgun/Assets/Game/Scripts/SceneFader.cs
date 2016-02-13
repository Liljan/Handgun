using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour {

    public Texture2D fadeoutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1f;
    private int fadeDir = FADE_DIRECTION.IN;       // direction -1 -> in, direction 1 -> out
	// Use this for initialization
	void Start () {

        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeoutTexture);
	}

    public float BeginFade(int dir)
    {
        fadeDir = dir;
        return fadeSpeed;
    }

    public void OnLevelWasLoaded()
    {
        BeginFade(FADE_DIRECTION.IN);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
