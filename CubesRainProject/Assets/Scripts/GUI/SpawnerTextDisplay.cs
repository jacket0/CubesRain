using TMPro;
using UnityEngine;

public abstract class SpawnerTextDisplay<T> : MonoBehaviour where T : Creatable<T>
{
    [SerializeField] protected TextMeshProUGUI _spawnCountText;
    [SerializeField] protected TextMeshProUGUI _createdCountText;
    [SerializeField] protected TextMeshProUGUI _activeCountText;
    [SerializeField] protected Spawner<T> _spawner;

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
