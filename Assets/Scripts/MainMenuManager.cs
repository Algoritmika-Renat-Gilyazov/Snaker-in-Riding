using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public void Crash(string code)
    {
        PlayerPrefs.SetString("ErrorCode", code);
        SceneManager.LoadScene("Error");
    }
    void Start()
    {
        try
        {
            if (!PlayerPrefs.HasKey("MouseSent"))
            {
                PlayerPrefs.SetFloat("MouseSent", 2f);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("SawSecret"))
            {
            }
            else
            {
                PlayerPrefs.SetInt("SawSecret", 0);
                PlayerPrefs.Save();
            }
        } catch
        {
            Crash("APP_INIT_ERROR");
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("NewGame");
    }

    public void ToOptions()
    {
        SceneManager.LoadScene("Options");
    }
    public void QuitGame()
    {
        #region QUIT
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        #endregion
    }
}