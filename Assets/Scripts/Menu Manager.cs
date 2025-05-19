using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject menu;
    public GameManager gameManager;


    private void Start()
    {
        menu.SetActive(false);
    }
    public void ContiuneGame()
    {
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 
        menu.SetActive(false);
        Time.timeScale = 1f;     
        gameManager.isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        menu.SetActive(true);
        gameManager.isPaused = true;
    }
    public void Start_Game()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

