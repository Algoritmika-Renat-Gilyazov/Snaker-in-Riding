using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class OptionsManager : MonoBehaviour
{
    [SerializeField]
    private Slider mouseSensitivitySlider;

    [SerializeField]
    private TMP_Dropdown dropdown;

    public bool isChanging = false;

    public void OnDropdownChanged(int index)
    {
        if (isChanging) return;
        StartCoroutine(SetLocale(index));
    }

    IEnumerator SetLocale(int index)
    {
        isChanging = true;

        yield return LocalizationSettings.InitializationOperation;

        switch (index)
        {
            case 0:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
                break;
            case 1:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("ru");
                break;
        }

        isChanging = false;
    }
    void Start()
    {
        var currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;

        switch (currentLocale)
        {
            case "en":
                dropdown.value = 0;
                break;
            case "ru":
                dropdown.value = 1;
                break;
            default:
                dropdown.value = 0;
                break;
        }
        if (PlayerPrefs.HasKey("MouseSent"))
        {
            float mouseSent = PlayerPrefs.GetFloat("MouseSent");
            mouseSensitivitySlider.value = mouseSent;
        }
        else
        {
            mouseSensitivitySlider.value = 2f;
            PlayerPrefs.SetFloat("MouseSent", 2f);
            PlayerPrefs.Save();
        }
    }

    public void OnMouseSensitivityChanged()
    {
        int mouseSent = Mathf.RoundToInt(mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("MouseSent", mouseSent);
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
