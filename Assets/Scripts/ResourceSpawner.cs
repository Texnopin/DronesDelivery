using UnityEngine;
using UnityEngine.UI;  // Для работы с UI элементами

public class ResourceSpawner : MonoBehaviour
{
    public GameObject resourcePrefab;
    public Vector2 spawnArea;
    public Slider spawnIntervalSlider;  // Ссылка на слайдер из UI
    private float spawnInterval;

    private int currentResourceCount = 0;

    private void Start()
    {
        if (spawnIntervalSlider != null)
        {
            spawnIntervalSlider.onValueChanged.AddListener(OnSliderValueChanged);
            spawnInterval = spawnIntervalSlider.value;
        }
        else
        {
            spawnInterval = 5f;  // Значение по умолчанию, если слайдер не задан
        }

        InvokeRepeating(nameof(SpawnResource), spawnInterval, spawnInterval);
    }

    private void SpawnResource()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            0.5f,
            Random.Range(-spawnArea.y, spawnArea.y)
        );

        Instantiate(resourcePrefab, randomPosition, Quaternion.identity);
        currentResourceCount++;
    }

    private void OnSliderValueChanged(float value)
    {
        // Обновляем spawnInterval и перезапускаем InvokeRepeating
        CancelInvoke(nameof(SpawnResource));
        spawnInterval = value;
        InvokeRepeating(nameof(SpawnResource), spawnInterval, spawnInterval);
    }
}
