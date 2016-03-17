using UnityEngine;
using System.Collections;

public class GLOBAL : MonoBehaviour {

    // A fake singleton
    public static bool IS_PAUSED = false;
    public static bool POWERUP_ACTIVE = false;
    public static string CURRENT_POWER;
    public static float TIME_SCALE = 1f;

    public static int TOTAL_SCORE = 0;
    public static int SCORE_MULT = 1;

    public static int DAMAGE_MULT = 1;
}
