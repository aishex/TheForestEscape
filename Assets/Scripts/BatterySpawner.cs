using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public GameObject batteryPrefab;
    public int batteryCount = 1;
    public Vector2 spawnArea = new Vector2(11, 9);

    void Start()
    {
        SpawnBatteries();
    }

    void SpawnBatteries()
    {
        for (int i = 0; i < batteryCount; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            
            if (randomPosition != Vector2.zero)
            {
                Instantiate(batteryPrefab, randomPosition, Quaternion.identity);
            }
        }
    }

    Vector2 GetRandomPosition()
    {
        for (int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(-spawnArea.x, spawnArea.x);
            float randomY = Random.Range(-spawnArea.y, spawnArea.y);
            Vector2 potentialPos = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

            if (Physics2D.OverlapCircle(potentialPos, 0.5f) == null)
            {
                return potentialPos;
            }
        }
        
        return Vector2.zero;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x * 2, spawnArea.y * 2, 0));
    }
}