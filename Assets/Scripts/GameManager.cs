using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] float loadDelay = 0.75f;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip musicClip;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;

    int coins;

    void Awake()
    {
        int numOfGameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length;
        if (numOfGameManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        UpdateLivesText();
        UpdateCoinsText();
        PlayMusic();
    }

    void PlayMusic()
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            Invoke(nameof(TakeLife), loadDelay);
        }
        else
        {
            Invoke(nameof(ResetGame), loadDelay);
        }
    }

    void ResetGame()
    {
        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    void TakeLife()
    {
        playerLives--;
        UpdateLivesText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCoin(int coin)
    {
        coins += coin;
        UpdateCoinsText();
    }

    public void PlaySFXClip(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    void UpdateLivesText()
    {
        livesText.text = playerLives.ToString();
    }

    void UpdateCoinsText()
    {
        coinsText.text = coins.ToString();
    }
}
