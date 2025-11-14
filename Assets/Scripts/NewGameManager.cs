using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class NewGameManager : MonoBehaviour
{
    public int cl = 0;

    public TMP_Dropdown gmselector;
    void Update()
    {
        if(gmselector.value == 0)
        {
            PlayerPrefs.SetString("Gamemode", "Survival");
        }
        else if(gmselector.value == 1)
        {
            PlayerPrefs.SetString("Gamemode", "Creative");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Game");
    }
    public void DSSFSGFGHJG()
    {
        if(cl < 3)
        {
            cl++;
            return;
        }
        SceneManager.LoadScene("a");
    }
}