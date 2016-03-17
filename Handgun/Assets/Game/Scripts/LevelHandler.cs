using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour
{
    private string countdownString;
    private bool isShowingCountdown = false;

    public static SceneFader sceneFader;

    // Level data
    public string nextLvl;

    // Weapon managers
    public GameObject leftWeapon;
    public GameObject rightWeapon;

    //UI
    public Text LeftAmmoText;
    public Text RightAmmoText;
    public Text TimerText;
    public Text ScoreText;

    public Font guiFont;
    private GUIStyle guiStyle;
    private string messageString;
    private bool showMessage = false;

    // timer
    public float timeLimit = 30f;
    private float elapsedTime;
    private bool isGameOver = false;

    // Level win conditions
    private int amountOfTargets;
    private int killedTargets = 0;
    private string gameOverString;

    // AUDIO
    private AudioSource sfxSource;
    private AudioSource musicSource;
    // SFX

    // MUSIC
    public AudioClip music;
    //public AudioClip musicTimeUp;
    public AudioClip musicWin;

    // Use this for initialization
    void Start()
    {
        // Cursor.visible = false;
        sceneFader = GetComponent<SceneFader>();
        InitLevelData();

        // Initiate all fonts and styles for GUI objects
        guiStyle = new GUIStyle();
        guiStyle.font = guiFont;
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.yellow;
        guiStyle.alignment = TextAnchor.MiddleCenter;

        TimerText.text = FormatTimeString((timeLimit - elapsedTime));
        ScoreText.text = LEVEL_DATA.SCORE.ToString();

        amountOfTargets = FindObjectsOfType<Spawner>().Length;

        StartCoroutine(CountdownSequence(0.5f));

        sfxSource = GameObject.FindWithTag("SFX_Source").GetComponent<AudioSource>();
        musicSource = GameObject.FindWithTag("Music_Source").GetComponent<AudioSource>();

        // Time.timeScale = 0.5f;
        // audioSource.pitch = Time.timeScale;

        DisablePowerup();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GLOBAL.IS_PAUSED = !GLOBAL.IS_PAUSED;

            if (GLOBAL.IS_PAUSED)
            {
                GLOBAL.IS_PAUSED = true;
                Time.timeScale = 0f;
                sfxSource.Pause();
                musicSource.Pause();
            }
            else
            {
                GLOBAL.IS_PAUSED = false;
                Time.timeScale = 1f;
                sfxSource.UnPause();
                musicSource.UnPause();
            }
        }

        if (!GLOBAL.IS_PAUSED && !isShowingCountdown && !isGameOver)
        {
            // update everything
            elapsedTime += Time.deltaTime;

            if (killedTargets >= amountOfTargets)
            {
                StartCoroutine(GameOver(2f, "You win!"));
            }

            //
            if (elapsedTime < timeLimit)
                TimerText.text = FormatTimeString((timeLimit - elapsedTime));
            else
            {
                StartCoroutine(GameOver(2f, "Time up!"));
            }
        }
    }

    public void DestroyTarget()
    {
        ++killedTargets;
    }

    public int GetDestroyedTargets()
    {
        return killedTargets;
    }

    public void AddScore(int score)
    {
        LEVEL_DATA.SCORE += score * GLOBAL.SCORE_MULT;
        ScoreText.text = LEVEL_DATA.SCORE.ToString();
    }

    public void OnGUI()
    {
        if (isShowingCountdown)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), countdownString, guiStyle);
            // GUILayout.Label(new Rect(0f,0f, 20f,20f) , countdownString, guiStyle);
        }

        if (isGameOver)
        {
            GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), gameOverString, guiStyle);
        }
        else if (GLOBAL.IS_PAUSED)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), "Paused", guiStyle);
        }
        else if (showMessage)
        {
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 0, 0), messageString, guiStyle);
        }
    }

    public IEnumerator ChangeScreen(int index)
    {
        float fadeTime = sceneFader.BeginFade(FADE_DIRECTION.IN);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(index);
    }

    public void SetLeftAmmoText(int clip, int total)
    {
        LeftAmmoText.text = clip + "/" + total;
    }

    public void SetRightAmmoText(int clip, int total)
    {
        RightAmmoText.text = clip + "/" + total;
    }

    public IEnumerator CountdownSequence(float time)
    {
        isShowingCountdown = true;
        rightWeapon.SetActive(false);
        leftWeapon.SetActive(false);

        countdownString = "3";
        yield return new WaitForSeconds(time);

        countdownString = "2";
        yield return new WaitForSeconds(time);

        countdownString = "1";
        yield return new WaitForSeconds(time);

        countdownString = "KILL 'EM ALL!";
        yield return new WaitForSeconds(time);

        isShowingCountdown = false;
        rightWeapon.SetActive(true);
        leftWeapon.SetActive(true);

        // sound
        musicSource.PlayOneShot(music);
        musicSource.loop = true;
    }

    public IEnumerator GameOver(float time, string gameOver)
    {
        rightWeapon.SetActive(false);
        leftWeapon.SetActive(false);
        DisablePowerup();
        gameOverString = gameOver;
        isGameOver = true;
        
        musicSource.Stop();
        musicSource.PlayOneShot(musicWin);
        yield return new WaitForSeconds(time);
        UpdateLevelData();
        Application.LoadLevel("Game_Over_Screen");
    }

    private string FormatTimeString(float e)
    {
        int d = (int)(e * 100.0f);
        int minutes = d / (60 * 100);
        int seconds = (d % (60 * 100)) / 100;
        int hundredths = d % 100;

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
    }

    public void EnablePowerup(string thePower)
    {
        GLOBAL.POWERUP_ACTIVE = true;
        GLOBAL.CURRENT_POWER = thePower;

        switch (thePower)
        {
            case POWERS.BULLET_TIME:
                StartCoroutine(BulletTime(2f));
                break;
            case POWERS.DOUBLE_SCORE:
                StartCoroutine(DoubleScore(10f));
                break;
            default:
                break;
        }
    }
    public void DisablePowerup()
    {
        GLOBAL.POWERUP_ACTIVE = false;
        guiStyle.normal.textColor = Color.yellow;
    }

    private IEnumerator BulletTime(float t)
    {
        Time.timeScale = 0.5f;
        sfxSource.pitch = Time.timeScale;
        musicSource.pitch = Time.timeScale;
        guiStyle.normal.textColor = Color.magenta;
        StartCoroutine(ShowGUIMessage("BULLET TIME!", t));
        yield return new WaitForSeconds(t);
        Time.timeScale = 1.0f;
        sfxSource.pitch = Time.timeScale;
        musicSource.pitch = Time.timeScale;
        DisablePowerup();
    }

    private IEnumerator DoubleScore(float t)
    {
        GLOBAL.SCORE_MULT = 2;
        StartCoroutine(ShowGUIMessage("DOUBLE POINTS!", t));
        yield return new WaitForSeconds(t);
        GLOBAL.SCORE_MULT = 1;
        DisablePowerup();
    }

    private IEnumerator ShowGUIMessage(string msg, float t)
    {
        messageString = msg;
        showMessage = true;
        yield return new WaitForSeconds(t);
        showMessage = false;
    }

    private void InitLevelData()
    {
        LEVEL_DATA.CURRENT_LVL = Application.loadedLevel;
        LEVEL_DATA.NEXT_LVL = nextLvl;
        LEVEL_DATA.HIT_TARGETS = 0;
        LEVEL_DATA.TOTAL_TARGETS = 0;
        LEVEL_DATA.SCORE = 0;
    }

    private void UpdateLevelData()
    {
        LEVEL_DATA.HIT_TARGETS = killedTargets;
        LEVEL_DATA.TOTAL_TARGETS = amountOfTargets;
    }
}
