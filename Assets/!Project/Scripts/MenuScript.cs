using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public TMP_Text soundText;
    public TMP_Text highscoreText;

    private void Start()
    {
        if (soundText != null)
        {
            SetSoundText();
        }

        if (highscoreText != null)
        {
            highscoreText.text = "Highscore " + PlayerPrefs.GetInt("Highscore").ToString();
        }
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

        SetSoundText();
    }

    private void SetSoundText()
    {
        soundText.text = PlayerPrefs.GetInt("Sound") == 1 ? "♪" : "X♪";
    }
}
