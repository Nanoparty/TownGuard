using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject OptionsMenu;

    public TransitionIn Transition;

    private Slider musicSlider;
    private Slider soundSlider;

    void Start()
    {
        EnableMainMenu();
        AudioManager.am.StartMenu();
    }

    private void EnableMainMenu()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);

        GameObject buttons = MainMenu.transform.GetChild(0).transform.GetChild(1).gameObject;
        Button playButton = buttons.transform.GetChild(0).GetComponent<Button>();
        Button optionsButton = buttons.transform.GetChild(1).GetComponent<Button>();
        Button quitButton = buttons.transform.GetChild(2).GetComponent<Button>();

        playButton.onClick.AddListener(PlayListener);
        optionsButton.onClick.AddListener(OptionsListener);
        quitButton.onClick.AddListener(QuitListener);
    }

    private void EnableOptionsMenu()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);

        GameObject sliders = OptionsMenu.transform.GetChild(0).transform.GetChild(1).gameObject;
        GameObject buttons = OptionsMenu.transform.GetChild(0).transform.GetChild(2).gameObject;

        musicSlider = sliders.transform.GetChild(0).GetComponentInChildren<Slider>();
        soundSlider = sliders.transform.GetChild(1).GetComponentInChildren<Slider>();

        musicSlider.onValueChanged.AddListener(delegate { MusicListener(); });
        soundSlider.onValueChanged.AddListener(delegate { SoundListener(); });

        Button backButton = buttons.transform.GetChild(0).GetComponent<Button>();

        backButton.onClick.AddListener(BackListener);
    }

    private void PlayListener()
    {
        AudioManager.am.Click();
        AudioManager.am.Whoosh();
        AudioManager.am.EndMenu();
        Transition.transOutStart = true;
        //SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    private void OptionsListener()
    {
        AudioManager.am.Click();
        EnableOptionsMenu();
    }

    private void QuitListener()
    {
        AudioManager.am.Click();
        Application.Quit();
    }

    private void MusicListener()
    {
        Data.musicVolume = musicSlider.value;
    }

    private void SoundListener()
    {
        Data.soundVolume = soundSlider.value;
    }

    private void BackListener()
    {
        AudioManager.am.Click();
        EnableMainMenu();
    }
}
