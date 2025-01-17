using TMPro;
using UnityEngine;

public abstract class SpawnerTextDisplay<T> : MonoBehaviour where T : Creatable<T>
{
    [SerializeField] private TextMeshProUGUI _spawnCountText;
    [SerializeField] private TextMeshProUGUI _createdCountText;
    [SerializeField] private TextMeshProUGUI _activeCountText;
    [SerializeField] private Spawner<T> _spawner;

    private void OnEnable()
    {
        _spawner.CountUpdated += UpdateValue;
    }

    private void OnDisable()
    {
        _spawner.CountUpdated -= UpdateValue;
    }

    protected void UpdateValue()
    {
        _spawnCountText.text = $"Заспавнено: {_spawner.CountSpawned}";
        _createdCountText.text = $"Создано: {_spawner.CountCreated}";
        _activeCountText.text = $"Активных: {_spawner.CountActive}";
    }
}
