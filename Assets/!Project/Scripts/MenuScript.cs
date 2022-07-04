using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public TMP_Text soundText;

    private void Start()
    {
        SetSoundText();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void ToggleSound()
    {
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound") == 1? 0 : 1);

        SetSoundText();
    }

    private void SetSoundText()
    {
        soundText.text = PlayerPrefs.GetInt("Sound") == 1 ? "♪" : "X";
    }
}
