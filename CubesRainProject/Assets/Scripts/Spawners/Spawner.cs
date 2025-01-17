using System;
using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : Creatable<T>
{
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 5;

    [SerializeField] private T _prefab;

    private PoolManager<T> _poolManager;

    public abstract event Action CountUpdated;

    public int CountSpawned => _poolManager.CountSpawned;
    public int CountActive => _poolManager.CountActive;
    public int CountCreated { get; protected set; }

    private void Awake()
    {
        _poolManager = new PoolManager<T>(_prefab, _poolCapacity, _poolMaxSize);
    }

    public T Spawn()
    {
        var obj = _poolManager.GetObject();
        ConfigureObject(obj);
        CountCreated++;

        return obj;
    }

    protected abstract void ConfigureObject(T obj);
}
