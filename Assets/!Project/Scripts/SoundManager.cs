using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [Header("Normal Sounds")]
    public AudioClip[] normalZombieHurtSound;
    public AudioClip[] normalZombieYellSound;
    public AudioClip[] normalPlayerHurtSound;
    public AudioClip normalZombieCagedSound;

    public AudioClip normalMenuMusic;
    public AudioClip normalGameMusic;
    public AudioClip normalGameOverMusic;

    [Header("Right Sounds")]
    public AudioClip[] gachizombieHurtSound;
    public AudioClip[] gachizombieYellSound;
    public AudioClip[] gachiplayerHurtSound;
    public AudioClip gachizombieCagedSound;

    public AudioClip gachimenuMusic;
    public AudioClip gachigameMusic;
    public AudioClip gachigameOverMusic;

    public AudioSource musicSource;

    [NonSerialized]
    public AudioClip[] zombieHurtSound;
    [NonSerialized]
    public AudioClip[] zombieYellSound;
    [NonSerialized]
    public AudioClip[] playerHurtSound;
    [NonSerialized]
    public AudioClip zombieCagedSound;

    [NonSerialized]
    public AudioClip menuMusic;
    [NonSerialized]
    public AudioClip gameMusic;
    [NonSerialized]
    public AudioClip gameOverMusic;
    private void Start()
    {
        SetGachi();
        if (SceneManager.GetActiveScene().name == "Game")
        {
            playMusic(gameMusic);
        }
        else
        {
            playMusic(menuMusic);
        }
    }

    public void playMusic(AudioClip music)
    {
        if (PlayerPrefs.GetInt("Sound") != 0)
        {
            return;
        }

        musicSource.clip = music;
        musicSource.Play();
    }

    public void playSoundEffect(AudioClip[] effects)
    {
        playSoundEffect(effects[UnityEngine.Random.Range(0, effects.Length)]);
    }

    public void playSoundEffect(AudioClip effect)
    {
        if (PlayerPrefs.GetInt("Sound") != 0)
        {
            return;
        }

        AudioSource source = (AudioSource)gameObject.AddComponent(typeof(AudioSource));

        source.clip = effect;
        source.Play();
        StartCoroutine(RemoveAudioSource(source, effect.length));
    }

    IEnumerator RemoveAudioSource(AudioSource source, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(source);
    }

    public void SetGachi()
    {
        if (PlayerPrefs.GetInt("RightVersion") == 1)
        {
            zombieHurtSound = gachizombieHurtSound;
            zombieYellSound = gachizombieYellSound;
            playerHurtSound = gachiplayerHurtSound;
            zombieCagedSound = gachizombieCagedSound;
            menuMusic = gachimenuMusic;
            gameMusic = gachigameMusic;
            gameOverMusic = gachigameOverMusic;
        }
        else
        {
            zombieHurtSound = normalZombieHurtSound;
            zombieYellSound = normalZombieYellSound;
            playerHurtSound = normalPlayerHurtSound;
            zombieCagedSound = normalZombieCagedSound;
            menuMusic = normalMenuMusic;
            gameMusic = normalGameMusic;
            gameOverMusic = normalGameOverMusic;
        }
    }
}
