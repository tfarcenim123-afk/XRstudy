using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // 생성할 방울 프리팹
    public float spawnInterval = 0.5f; // 생성 간격

    [Header("생성 범위 설정")]
    public float xRange = 0.5f; // 좌우 랜덤 범위 (-0.5 ~ 0.5)
    public float zRange = 0.5f; // 앞뒤 랜덤 범위 (-0.5 ~ 0.5)

    void Start()
    {
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
    }

    void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            // 1. 현재 위치를 기준으로 X와 Z값에만 랜덤 더하기
            float randomX = Random.Range(-xRange, xRange);
            float randomZ = Random.Range(-zRange, zRange);
            
            // 2. 새로운 생성 위치 계산 (Y는 부모 위치 그대로 사용)
            Vector3 spawnPos = new Vector3(
                transform.position.x + randomX, 
                transform.position.y, 
                transform.position.z + randomZ
            );

            // 3. 계산된 위치에 방울 생성
            Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
        }
    }
}