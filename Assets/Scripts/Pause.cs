using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject panelPause;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("esc");
            if (isPaused)
                Resume();
            else
                PauseOn();
        }
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void PauseOn()
    {
        panelPause.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
