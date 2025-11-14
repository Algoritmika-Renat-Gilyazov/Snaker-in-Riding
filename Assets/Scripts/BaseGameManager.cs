using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseScreen;

    public bool paused = false;

    public float mouseSent;

    public Player player;

    public void Crash(string code)
    {
        PlayerPrefs.SetString("ErrorCode" ,code);
        SceneManager.LoadScene("Error");
    }

    void Awake()
    {
        try
        {
            if (PlayerPrefs.HasKey("MouseSent"))
            {
                mouseSent = PlayerPrefs.GetFloat("MouseSent");
            }
            else
            {
                mouseSent = 2f;
                PlayerPrefs.SetFloat("MouseSent", 2f);
                PlayerPrefs.Save();
            }
            Debug.Log("BaseManager started!");
        } catch
        {
            Crash("BASE_AWAKE_ERROR");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }

    public void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ScalePlayer(float scale)
    {
        player.transform.localScale = new Vector3(scale, scale, scale);
    }
    public void ScaleCharacter(Character ch, float scale)
    {
        ch.transform.localScale = new Vector3(scale, scale, scale);
    }
}
