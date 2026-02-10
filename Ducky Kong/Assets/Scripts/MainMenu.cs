using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevelName = "Level 1";
    public void Play()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
