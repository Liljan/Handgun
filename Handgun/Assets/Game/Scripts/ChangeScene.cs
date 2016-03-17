using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

    public void NextLevel()
    {
        SetScene(LEVEL_DATA.NEXT_LVL);
    }

    public void RestartLevel()
    {
        SetScene(LEVEL_DATA.CURRENT_LVL);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetScene(string sceneIndex)
    {
        try
        {
            Application.LoadLevel(sceneIndex);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public void SetScene(int sceneIndex)
    {
        try
        {
            Application.LoadLevel(sceneIndex);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
