using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    private bool canShoot = true;
    public float shootCooldown = 0.1f;

    public int ammo;
    public int clipSize;
    private int currentClip;

    private bool reloading = false;
    public float reloadTime = .1f;

    public AudioClip[] SFX_SHOOT;

    // Levelhandler
    private LevelHandler levelHandler;

	// Use this for initialization
	void Start () {
        currentClip = clipSize;
        levelHandler = FindObjectOfType<LevelHandler>();
        levelHandler.SetLeftAmmoText(currentClip, ammo);
    }
	
	// Update is called once per frame
	void Update () {

        // Left click
        if (Input.GetMouseButtonDown(0) && currentClip > 0 && !reloading)
        {
            Fire();   
        }

        // right click
        if (Input.GetMouseButtonDown(1) && ammo > 0 && !reloading)
        {
            StartCoroutine(Reload());
        }
	}

    public void Fire()
    {
        --currentClip;
        levelHandler.SetLeftAmmoText(currentClip, ammo);
    }

    public IEnumerator Reload()
    {
        Debug.Log("Current clip before reload: " + currentClip);
        bool visible = transform.gameObject.GetComponent<SpriteRenderer>().enabled;
        reloading = true;
        visible = false;

        yield return new WaitForSeconds(reloadTime);

        reloading = false;
        visible = true;
        ammo -= (clipSize - currentClip);
        ammo = Mathf.Max(0, ammo);

        if(ammo > clipSize)
        {
            currentClip = clipSize;
        }
        else
        {
            currentClip = ammo;
        }
        
        Debug.Log("Current clip after reload: " + currentClip);
        levelHandler.SetLeftAmmoText(currentClip, ammo);
    }
}
