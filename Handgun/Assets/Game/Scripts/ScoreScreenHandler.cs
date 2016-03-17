using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScreenHandler : MonoBehaviour
{
    // Text objects
    public Text titleText;
    public Text rankText;

    // Buttons
    public Button retryButton;
    public Button nextButton;

    // Other objects
    public GameObject radialBar;
    public GameObject fireWorksPrefab;

    public float titleDelay = 0.5f;
    public float radialDelay = 0.2f;
    public float itemDelay = 0.2f;

    private bool startAnimation = true;
    private bool hasWon = false;
    private Camera cam;

    // audio
    private AudioSource audioSource;
    public AudioClip SFX_PUNCH;
    public AudioClip SFX_RADIAL;
    public AudioClip SFX_FIREWORKS;
    public AudioClip SFX_WIN;

    // Use this for initialization
    void Start()
    {
        titleText.gameObject.SetActive(false);
        rankText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        radialBar.SetActive(false);

        UnityEngine.Cursor.visible = true;
        cam = FindObjectOfType<Camera>();
        audioSource = FindObjectOfType<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startAnimation)
        {
            startAnimation = false;
            StartCoroutine(ShowTexts());
        }
    }

    private IEnumerator ShowTexts()
    {
        yield return new WaitForSeconds(titleDelay);
        audioSource.PlayOneShot(SFX_PUNCH, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        titleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(radialDelay);
        audioSource.PlayOneShot(SFX_PUNCH, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        radialBar.SetActive(true);
    }

    public IEnumerator ShowRank(string rank, Color color, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(SFX_PUNCH, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        rankText.color = color;
        rankText.text = rank;
        rankText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay * 1.5f);
        audioSource.PlayOneShot(SFX_WIN, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        audioSource.PlayOneShot(SFX_PUNCH, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        retryButton.gameObject.SetActive(true);
        if (hasWon)
        {
            nextButton.gameObject.SetActive(true);
            StartCoroutine(SpawnFireWorks(0.7f));
        }
    }

    public void SetWin(bool b)
    {
        hasWon = b;
    }

    private IEnumerator SpawnFireWorks(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Vector3 dim = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth,cam.pixelHeight,0));
            Vector3 pos = new Vector3(dim.x * Random.RandomRange(-1f, 1f), dim.y * Random.RandomRange(-1f, 1f), -1f);

            GameObject obj = Instantiate(fireWorksPrefab);
            obj.transform.position = pos;
            obj.GetComponent<ParticleSystem>().startColor = new Color(Random.value, Random.value, Random.value);
            audioSource.PlayOneShot(SFX_FIREWORKS, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        }
    }
}
