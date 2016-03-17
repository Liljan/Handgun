using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuScript : MonoBehaviour
{
    public Button startText;
    public Button exitText;

    void Start()
    {
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
    }
    public void StartGame() //this function will be used on our Play button
    {
        Application.LoadLevel(1); //this will load our first level from our build settings. "1" is the second scene in our game
    }
    public void ExitGame() //This function will be used on our "Yes" button in our Quit menu
    {
        //Only works on building the game
        Application.Quit(); 
    }
}