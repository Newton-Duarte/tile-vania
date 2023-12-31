using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numOfScenePersists = FindObjectsByType<ScenePersist>(FindObjectsSortMode.None).Length;
        if (numOfScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
