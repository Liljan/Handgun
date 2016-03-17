using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CameraEffects : MonoBehaviour
{

    // Use this for initialization

    private BloomOptimized bo;
    private VignetteAndChromaticAberration vaca;
    private ScreenOverlay so;

    void Start()
    {
        bo = GetComponent<BloomOptimized>();
        bo.enabled = false;
        vaca = GetComponent<VignetteAndChromaticAberration>();
        vaca.enabled = false;
        so = GetComponent<ScreenOverlay>();
        so.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (GLOBAL.POWERUP_ACTIVE)
        {
            if (GLOBAL.CURRENT_POWER == POWERS.BULLET_TIME)
            {
                bo.enabled = true;
                vaca.enabled = true;
            }
            else if (GLOBAL.CURRENT_POWER == POWERS.DOUBLE_SCORE)
            {
                bo.enabled = true;
                so.enabled = true;
            }
        }
        else
        {
            bo.enabled = false;
            vaca.enabled = false;
            so.enabled = false;
        }
    }
}
