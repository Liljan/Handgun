using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelHandler : MonoBehaviour {

    public static SceneFader sceneFader;

    //UI
    public Text LeftAmmoText;
    public Text RightAmmoText;

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

    public void SetLeftAmmoText(int clip, int total)
    {
        LeftAmmoText.text = clip + "/" + total;
    }

    public void SetRightAmmoText(int clip, int total)
    {
        LeftAmmoText.text = clip + "/" + total;
    }
}
