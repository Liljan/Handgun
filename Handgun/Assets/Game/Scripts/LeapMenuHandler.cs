using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Leap;

public class LeapMenuHandler : MonoBehaviour
{
    public Selectable selectedButton;
    private EventSystem es;
    private int current = 0;

    public HandController hc;
    public bool right;
    private int currentHand;

    // Levelhandler
    private Transform aim;
    private Leap.Controller controller;
    private Frame frame;
    private SwipeGesture swipe;

    private bool isSwiping = false;

    public float delay = 2f;
    public float tapSpeed = 30.0f;

    // Audio
    private AudioSource audioSource;

    // SFX
    public AudioClip SFX_HOVER;
    public AudioClip SFX_SELECT;

    // Music
    public AudioClip MSC_BACKGROUND_LOOP;
    
    // Use this for initialization
    void Start()
    {
        hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPESCREENTAP);
        hc.GetLeapController().EnableGesture(Gesture.GestureType.TYPESWIPE);
        hc.GetLeapController().Config.SetFloat("Gesture.ScreenTap.MinForwardVelocity", tapSpeed);
        hc.GetLeapController().Config.SetFloat("Gesture.ScreenTap.MinDistance", 2.0f);
        hc.GetLeapController().Config.Save();

        currentHand = right ? 0 : 1;

        audioSource = FindObjectOfType<AudioSource>();
       // StartCoroutine(LoopMusic(MSC_BACKGROUND_LOOP));
        audioSource.PlayOneShot(MSC_BACKGROUND_LOOP, SETTINGS.MASTER_VOLUME * SETTINGS.MUSIC_VOLUME);
    }

    // Update is called once per frame
    void Update()
    {
        frame = hc.GetFrame();

        foreach (Gesture gesture in frame.Gestures())
        {

            switch (gesture.Type)
            {
                case (Gesture.GestureType.TYPESCREENTAP):
                    {
                        audioSource.PlayOneShot(SFX_SELECT, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
                        StartCoroutine(PressSelectedButton());
                        break;
                    }
                case (Gesture.GestureType.TYPESWIPE):
                    {
                        swipe = new SwipeGesture(gesture);

                        if (!isSwiping && swipe.Direction.y > 0)
                        {
                            StartCoroutine(CycleUp(delay));
                        }

                        else if (!isSwiping && swipe.Direction.y < 0)
                        {
                            StartCoroutine(CycleDown(delay));
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }


    private IEnumerator CycleUp(float t)
    {
        selectedButton = selectedButton.navigation.selectOnUp;
        EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
        audioSource.PlayOneShot(SFX_HOVER, SETTINGS.MASTER_VOLUME * SETTINGS.MUSIC_VOLUME);

        isSwiping = true;
        yield return new WaitForSeconds(t);
        isSwiping = false;

    }

    private IEnumerator CycleDown(float t)
    {
        selectedButton = selectedButton.navigation.selectOnDown;
        EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
        audioSource.PlayOneShot(SFX_HOVER, SETTINGS.MASTER_VOLUME * SETTINGS.MUSIC_VOLUME);

        isSwiping = true;
        yield return new WaitForSeconds(t);
        isSwiping = false;
    }

    private IEnumerator PressSelectedButton()
    {
        audioSource.PlayOneShot(SFX_SELECT, SETTINGS.MASTER_VOLUME * SETTINGS.SFX_VOLUME);
        yield return new WaitForSeconds(0f); // bad, to be changed later
        Debug.Log("Ready to change scene");

        Button b = selectedButton.gameObject.GetComponent<Button>();
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(b.gameObject, pointer, ExecuteEvents.submitHandler);
    }

    private void EnableSwipe()
    {
        isSwiping = false;
    }

    private IEnumerator LoopMusic(AudioClip ac)
    {
        while (true)
        {
            audioSource.PlayOneShot(ac, SETTINGS.MASTER_VOLUME * SETTINGS.MUSIC_VOLUME);
            yield return new WaitForSeconds(ac.length);
        }
    }
}
