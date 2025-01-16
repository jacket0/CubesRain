using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Creatable<T>
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 5;

    private PoolManager<T> _poolManager;

    public abstract event Action CountUpdated;

    public int CountSpawned => _poolManager.CountSpawned;
    public int CountActive => _poolManager.CountActive;
    public int CountCreated { get; protected set; }

    private void Awake()
    {
        _poolManager = new PoolManager<T>(_prefab, _poolCapacity, _poolMaxSize);
    }

    protected abstract void ConfigureObject<R>(T obj) where R : Creatable<R>;

    public T Spawn<R>() where R : Creatable<R>
    {
        var obj = _poolManager.GetObject<T>();
        ConfigureObject<T>(obj);
        CountCreated++;

        return obj;
    }
}
