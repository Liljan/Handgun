using UnityEngine;
using System.Collections;
using Leap;

public class Aim : MonoBehaviour
{
    private Vector3 pos;

    private Leap.Vector fingerpos;

    public HandController hc;
    private int currentHand;
    public bool right;

    public float zDistance = -10f;

    // FOR DEBUGING
    public const bool USE_MOUSE = false;

    // Use this for initialization
    void Start()
    {
        currentHand = right ? 0 : 1;
         Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GLOBAL.IS_PAUSED)
        {
            if (USE_MOUSE)
            {
                // MOUSE CODE, code to move crosshair
                pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
                pos = Camera.main.ScreenToWorldPoint(pos);
                transform.position = pos;
            }
            else
            {
                // LEAP MOTION, code to move crosshair
                FingerList fl = hc.GetFrame().Hands[currentHand].Fingers.FingerType(Finger.FingerType.TYPE_INDEX);
                Finger indexfinger = fl[0];
                fingerpos = indexfinger.StabilizedTipPosition;

                Vector3 indexPostion = fingerpos.ToUnityScaled(false);

                transform.position = hc.transform.TransformPoint(indexPostion.x, indexPostion.y, 1f);
            }
        }
    }
}
