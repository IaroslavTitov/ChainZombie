using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] zombieHurtSound;
    public AudioClip[] zombieYellSound;
    public AudioClip[] playerHurtSound;
    public AudioClip zombieCagedSound;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip gameOverMusic;

    public AudioSource musicSource;

    private void Start()
    {
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
        if (PlayerPrefs.GetInt("Sound") != 1)
        {
            return;
        }

        musicSource.clip = music;
        musicSource.Play();
    }

    public void playSoundEffect(AudioClip[] effects)
    {
        playSoundEffect(effects[Random.Range(0, effects.Length)]);
    }

    public void playSoundEffect(AudioClip effect)
    {
        if (PlayerPrefs.GetInt("Sound") != 1)
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
}
