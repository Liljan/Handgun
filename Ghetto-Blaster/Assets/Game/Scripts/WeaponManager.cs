using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour {

    private bool canShoot = true;
    public float shootCooldown = 0.1f;

    public int ammo = 100;
    public int clipSize = 10;
    private int currentClip;

	// Use this for initialization
	void Start () {
        currentClip = clipSize;
	}
	
	// Update is called once per frame
	void Update () {

        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // right click
        if (Input.GetMouseButtonDown(1))
        {
            Reload();
        }

	}

    public void Fire()
    {
        // TODO
        Debug.Log("Break sum foo!");
    }

    public void Reload()
    {
        // TODO
        Debug.Log("Reloading!");
    }
}
