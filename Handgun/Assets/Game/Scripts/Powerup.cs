using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    public GameObject destroyParticle;
    public float duration = 10f;
    public AudioClip[] powerupaudio;

    public string power;

    private Renderer renderer;
    private LevelHandler levelHandler;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        levelHandler = FindObjectOfType<LevelHandler>();
    }

    // Update is called once per frame

    private void Blink()
    {
        renderer.enabled = false;
    }
    public void EnablePowerup()
    {
        levelHandler.EnablePowerup(power);
        Debug.Log(power);
        Instantiate(destroyParticle, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
