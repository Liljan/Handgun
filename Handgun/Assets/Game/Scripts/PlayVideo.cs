using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayVideo : MonoBehaviour
{
    public MovieTexture movie;
    private AudioSource audio;

    // Use this for initialization
    void Start()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        movie.Play();
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        Application.LoadLevel("Start_Menu_Screen");
    }
}
