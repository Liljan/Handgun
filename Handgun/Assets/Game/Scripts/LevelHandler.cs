using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

    public static SceneFader sceneFader;

	// Use this for initialization
	void Start () {
        sceneFader = GetComponent<SceneFader>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator ChangeScreen(int index)
    {
        float fadeTime = sceneFader.BeginFade(FADE_DIRECTION.IN);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(index);
    }
}
