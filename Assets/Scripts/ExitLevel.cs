using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] float loadLevelDelay = 0.75f;
    [SerializeField] AudioClip exitClip;

    PlayerMovement player;

    void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isAlive) return;

        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        FindAnyObjectByType<GameManager>().PlaySFXClip(exitClip);
        yield return new WaitForSecondsRealtime(loadLevelDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindAnyObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
