using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    private bool hasSpawned = false;
    public int spawnAmount = 0;
    private float elapsedTime = 0f;
    public float time = 5f;

    // Audio
    private AudioSource source;
    public AudioClip SFX_DESPAWN;

    // child
    private Transform child;
    private LevelHandler levelHandler;

	// Use this for initialization
	void Start () {
        levelHandler = FindObjectOfType<LevelHandler>();
        source = FindObjectOfType<AudioSource>();
        child = this.transform.FindChild("Target");
        child.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if(!GLOBAL.IS_PAUSED)
        elapsedTime += Time.deltaTime;

        if (levelHandler.GetDestroyedTargets() >= spawnAmount  && !hasSpawned)
        {
            child.gameObject.SetActive(true);
            hasSpawned = true;
            // Does not support the pause function
            StartCoroutine(Kill(time));
        }
	}

    public IEnumerator Kill(float t)
    {
        yield return new WaitForSeconds(t);
        source.PlayOneShot(SFX_DESPAWN, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        levelHandler.DestroyTarget();
        Destroy(this.gameObject);
    }
}
