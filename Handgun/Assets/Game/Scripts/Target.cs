using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    // Points
    public int pointsOuter = 100;
    public int pointsInner = 200;
    public int pointsBullseye = 300;

    // temporary
    public int points = 100;

    public int health = 1;

    public GameObject destroyParticle;
    public GameObject displayText;

    private LevelHandler levelHandler;

    // public audio
    private AudioSource audioSource;
    public AudioClip[] SFX_HIT;

    // Use this for initialization
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        levelHandler = FindObjectOfType<LevelHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            this.Kill();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        int rnd = Random.Range(0, SFX_HIT.Length);
        audioSource.PlayOneShot(SFX_HIT[rnd], SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        Instantiate(destroyParticle, transform.position, transform.rotation);
    }

    public void Kill()
    {
        levelHandler.AddScore(points);
        levelHandler.DestroyTarget();

        Destroy(this.gameObject);
    }
}
