using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string sceneName;
    public string spawnPointName;
    public static string targetSpawn; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetSpawn = spawnPointName;
            SceneManager.LoadScene(sceneName);
        }
    }
}