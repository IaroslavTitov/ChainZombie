﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public TMP_Text soundText;
    public TMP_Text rightVersionText;
    public TMP_Text highscoreText;

    private SoundManager soundManager;
    private void Start()
    {
        if (soundText != null)
        {
            SetSoundText();
        }

        if (rightVersionText != null)
        {
            SetRightVersionText();
        }

        if (highscoreText != null)
        {
            highscoreText.text = "Highscore " + PlayerPrefs.GetInt("Highscore").ToString();
        }

        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ToggleSound()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound") == 1? 0 : 1);

        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            soundManager.playMusic(soundManager.menuMusic);
        }
        else
        {
            soundManager.musicSource.Stop();
        }

        SetSoundText();
    }

    private void SetSoundText()
    {
        soundText.text = PlayerPrefs.GetInt("Sound") == 0 ? "on" : "off";
    }

    public void ToggleRightVersion()
    {
        PlayerPrefs.SetInt("RightVersion", PlayerPrefs.GetInt("RightVersion") == 1 ? 0 : 1);

        soundManager.SetGachi();
        soundManager.playMusic(soundManager.menuMusic);

        SetRightVersionText();
        FindObjectsOfType<ZombieScript>().ToList().ForEach(x => x.SetGachi());
    }

    private void SetRightVersionText()
    {
        rightVersionText.text = PlayerPrefs.GetInt("RightVersion") == 1 ? "on" : "off";
    }
}
