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
    [SerializeField] float loadGameOverDelay = 3f;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] AudioClip musicClip;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] AudioClip bonusLivesClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] float bonusLivesThresholdMultiplier = 1.5f;

    int coins;
    int bonusLivesThreshold = 10;

    Coroutine gameOverRoutine;

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
        PlayMusic(musicClip);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.Stop();
        musicSource.clip = music;
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

    public void GameOver()
    {
        if (gameOverRoutine != null)
        {
            StopCoroutine(gameOverRoutine);
        }

        gameOverRoutine = StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadGameOver()
    {
        musicSource.Stop();
        PlaySFXClip(gameOverClip);
        yield return new WaitForSeconds(loadGameOverDelay);
        SceneManager.LoadScene("Gameover");
        Destroy(gameObject);
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

        if (coins == bonusLivesThreshold)
        {
            coins = 0;
            playerLives++;
            bonusLivesThreshold = Convert.ToInt32(bonusLivesThreshold * bonusLivesThresholdMultiplier);
            PlaySFXClip(bonusLivesClip);
            UpdateLivesText();
        }

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
