using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public string mainMenuScene = "MainMenu";
    public GameObject pausePanel;
    public GameObject deathPanel;
    public GameObject winPanel;
    private bool paused;
    private bool lost = false;
    private bool won = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        instance = this;
        ResumeGame();
        lost = false;
        won = false;
    }

    public void LoseGame()
    {

        ResumeGame();
        lost = true;
        paused = true;
        Time.timeScale = 0f;
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void WinGame()
    {

        ResumeGame();
        won = true;
        paused = true;
        Time.timeScale = 0f;
        winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Retry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !lost && !won)
        {
            if (IsPaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        if (!paused && !lost && !won)
        {
            paused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ResumeGame()
    {
        if (paused && !lost && !won)
        {
            paused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    public void NextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public bool IsPaused()
    {
        return paused;
    }
    public void Play()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game quit");
    }
}
