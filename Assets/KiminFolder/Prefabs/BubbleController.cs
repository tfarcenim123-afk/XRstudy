using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [Header("크기 및 수명")]
    public float minStartSize = 0.05f;
    public float maxStartSize = 0.12f;
    public float growthRate = 0.05f; // 초당 커지는 크기 (0.1은 생각보다 빠를 수 있어 조절)
    public float lifeTime = 3f;

    [Header("움직임 설정")]
    public float riseSpeed = 0.5f;      // 상승 속도
    public float wiggleStrength = 0.8f; // 흔들림 강도
    public float wiggleSpeed = 3.0f;    // 흔들림 속도 (값이 클수록 파르르 떪)

    private float _randomOffset;

    void Start()
    {
        // 1. 시작 사이즈 랜덤 설정
        float randomSize = Random.Range(minStartSize, maxStartSize);
        transform.localScale = Vector3.one * randomSize;

        // 2. 랜덤 오프셋 (모든 방울이 다 다르게 흔들리도록 함)
        _randomOffset = Random.Range(0f, 1000f);

        // 3. 수명 다하면 삭제
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 1. 크기 성장
        transform.localScale += Vector3.one * growthRate * Time.deltaTime;

        // 2. 위로 상승
        Vector3 movement = Vector3.up * riseSpeed * Time.deltaTime;

        // 3. 랜덤 흔들림 계산 (Perlin Noise 활용)
        // 시간에 랜덤 오프셋을 더해 각 방울마다 고유한 움직임 부여
        float noiseX = Mathf.PerlinNoise(Time.time * wiggleSpeed, _randomOffset) * 2.0f - 1.0f;
        float noiseZ = Mathf.PerlinNoise(_randomOffset, Time.time * wiggleSpeed) * 2.0f - 1.0f;

        movement.x += noiseX * wiggleStrength * Time.deltaTime;
        movement.z += noiseZ * wiggleStrength * Time.deltaTime;

        // 최종 이동 적용
        transform.Translate(movement, Space.World);
    }
}