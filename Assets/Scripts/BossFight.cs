using System;
using System.Collections;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject[] minionsWave1 = new GameObject[5];
    [SerializeField] GameObject[] minionsWave2 = new GameObject[5];
    [SerializeField] float minionSpawnTime = 2.5f;
    [SerializeField] float timeBetweenWaves = 3f;
    [SerializeField] float bossEnrageTime = 10f;
    [SerializeField] float moveSpeedMultiplerWhileEnraged = 1.25f;
    [SerializeField] AudioClip bossClip;
    [SerializeField] AudioClip bossSpeakClip;

    GameManager gameManager;

    Coroutine checkBossAliveRoutine;
    Coroutine bossRoutine;
    Coroutine minionsRoutine;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        gameManager.PlayMusic(bossClip);

        CallMinions();
    }

    void CheckBossAlive()
    {
        if (checkBossAliveRoutine != null)
        {
            StopCoroutine(checkBossAliveRoutine);
        }

        checkBossAliveRoutine = StartCoroutine(BossAlive());
    }

    IEnumerator BossAlive()
    {
        yield return new WaitForSeconds(0.5f);
        
        if (boss == null)
        {
            StopCoroutine(bossRoutine);
            gameManager.GameOver();
        }
        else
        {
            CheckBossAlive();
        }
    }

    void CallMinions()
    {
        if (minionsRoutine != null)
        {
            StopCoroutine(minionsRoutine);
        }

        minionsRoutine = StartCoroutine(Minions());
    }

    IEnumerator Minions()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        foreach (var minion in minionsWave1)
        {
            minion.SetActive(true);
            yield return new WaitForSeconds(minionSpawnTime);
        }

        yield return new WaitForSeconds(timeBetweenWaves);

        foreach (var minion in minionsWave2)
        {
            minion.SetActive(true);
            yield return new WaitForSeconds(minionSpawnTime);
        }

        StopCoroutine(minionsRoutine);

        CallBoss();
    }

    void CallBoss()
    {
        if (bossRoutine != null)
        {
            StopCoroutine(bossRoutine);
        }

        bossRoutine = StartCoroutine(Boss());
        CheckBossAlive();
    }

    IEnumerator Boss()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        boss.SetActive(true);
        gameManager.PlaySFXClip(bossSpeakClip);

        yield return new WaitForSeconds(bossEnrageTime);

        var enemyScript = boss.GetComponent<EnemyMovement>();
        enemyScript.SetMoveSpeed(enemyScript.MoveSpeed * moveSpeedMultiplerWhileEnraged);

        StopCoroutine(bossRoutine);
    }
}
