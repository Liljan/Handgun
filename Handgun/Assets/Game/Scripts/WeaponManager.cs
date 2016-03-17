using UnityEngine;
using System.Collections;
using Leap;

public class WeaponManager : MonoBehaviour
{
    public float shootCooldown = 0.1f;

    public HandController hc;
    public bool right;
    private int currentHand;

    public int ammo;
    public int clipSize;
    public int currentClip;
    public bool canFire = false;

    private bool reloading = false;
    public float reloadTime = 0.1f;
    private float length;
    public int damage = 1;
    private bool firstBullet = true;

    // Levelhandler
    private LevelHandler levelHandler;
    private Transform aim;
    private Leap.Controller controller;

    // SFX
    public AudioClip[] SFX_DEFAULT_FIRE;
    public AudioClip SFX_RELOAD;
    private AudioSource audioSource;

    // INTERACTION CONSTANTS
    private const float firstPinchDist = 90;
    private const float minPinchDist = 78;
    private const float maxPinchDist = 82;

    // Use this for initialization
    void Start()
    {
        audioSource = GameObject.FindWithTag("SFX_Source").GetComponent<AudioSource>();
        levelHandler = FindObjectOfType<LevelHandler>();
        UpdateAmmoText();
        aim = GetComponent<Aim>().transform;
        controller = new Controller();
        controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
        controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
        controller.Config.SetFloat("Gesture.Circle.MinRadius", 50.0f);
        controller.Config.Save();
        length = 15f;
        canFire = false;
        currentHand = right ? 0 : 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (!GLOBAL.IS_PAUSED)
        {
            Frame frame = controller.Frame();
            if (hc.IsConnected())
            {
                if (!Aim.USE_MOUSE) //leap code
                {
                    FingerList fl = hc.GetFrame().Hands[currentHand].Fingers.FingerType(Finger.FingerType.TYPE_INDEX);
                    Finger indexfinger = fl[0];
                    FingerList f2 = hc.GetFrame().Hands[currentHand].Fingers.FingerType(Finger.FingerType.TYPE_THUMB);
                    Finger thumb = f2[0];

                    Leap.Vector vectorBetweenFingers = indexfinger.StabilizedTipPosition - thumb.StabilizedTipPosition;
                    length = vectorBetweenFingers.Magnitude;

                    if(firstBullet) //Runs one time
                    {
                        if (length > firstPinchDist)
                        {
                            canFire = true;
                        }
                        if (length < minPinchDist && canFire == true && currentClip > 0 && !reloading)
                        {
                            Fire();
                            canFire = false;
                            firstBullet = false;
                        }
                    }
                    else if(!firstBullet)
                    {
                        if (length < minPinchDist && length > 1 && canFire == true && currentClip > 0 && !reloading)
                        {
                            Fire();
                            canFire = false;
                        }
                        if (length > maxPinchDist)
                            canFire = true;
                    }
                }
            }
            else //mouse
            {
                // Left click
                if (Input.GetButtonDown("Fire") && currentClip > 0 && !reloading)
                {
                    Fire();
                }
            }
            foreach (Gesture gesture in frame.Gestures())
            {
                switch (gesture.Type)
                {
                    case (Gesture.GestureType.TYPECIRCLE):
                        {
                            if (ammo > 0 && !reloading)
                                StartCoroutine(Reload());
                            break;
                        }
                        /* Implementation for another powerup (Sword)
                    case (Gesture.GestureType.TYPESWIPE):
                        {
                            /*
                            RaycastHit2D hit;
                            hit = Physics2D.Raycast(aim.position, Vector2.zero);

                            if (GLOBAL.POWERUP_ACTIVE == true)
                            {
                                if (hit)
                                {
                                    if (hit.collider.tag == "Target")
                                    {
                                        Target t = hit.transform.gameObject.GetComponent<Target>();
                                        t.TakeDamage(damage);
                                    }
                                    if (hit.collider.tag == "Powerup")
                                    {
                                        Powerup pu = hit.transform.gameObject.GetComponent<Powerup>();
                                        pu.EnablePowerup();
                                    }
                                }
                            }
                            break;
                        }*/
                    default:
                        {
                            break;
                        }
                }
            }
            // right click
            if (Input.GetButtonDown("Reload") && ammo > 0 && !reloading)
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Fire()
    {
        --currentClip;
        UpdateAmmoText();
        int rndIndex = Random.Range(0, SFX_DEFAULT_FIRE.Length);
        audioSource.PlayOneShot(SFX_DEFAULT_FIRE[rndIndex], SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);

        RaycastHit2D hit;

        //if mouse
        if (Aim.USE_MOUSE)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
        else //leap
        {
            hit = Physics2D.Raycast(aim.position, Vector2.zero);
        }
        if (hit)
        {
            // if a hit was registered at all...
            if (hit.collider.tag == "Target")
            {
                Target t = hit.transform.gameObject.GetComponent<Target>();
                t.TakeDamage(damage);
            }
            if (hit.collider.tag == "Powerup" && GLOBAL.POWERUP_ACTIVE == false)
            {
                Powerup pu = hit.transform.gameObject.GetComponent<Powerup>();
                pu.EnablePowerup();
            }
        }
    }

    public IEnumerator Reload()
    {
        bool visible = transform.gameObject.GetComponent<SpriteRenderer>().enabled;
        reloading = true;
        visible = false;

        audioSource.PlayOneShot(SFX_RELOAD, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        yield return new WaitForSeconds(reloadTime);

        reloading = false;
        visible = true;
        ammo -= (clipSize - currentClip);
        ammo = Mathf.Max(0, ammo);

        if (ammo > clipSize)
        {
            currentClip = clipSize;
        }
        else
        {
            currentClip = ammo;
        }
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (right)
        {
            levelHandler.SetRightAmmoText(currentClip, ammo);
        }
        else
        {
            levelHandler.SetLeftAmmoText(currentClip, ammo);
        }
    }
}
