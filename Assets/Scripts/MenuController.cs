using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pausePanel;

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void OnPause()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void LoadLevelScene()
    {
        SceneManager.LoadScene("Level_00");
        Debug.Log("Loading game scene now");
    }

    public void LoadGameOverScene()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("GameOver");
    }

    public void Resume()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        LoadLevelScene();
    }
}
