using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : Creatable<T>
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected Transform _originPoint;
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 5;

    protected ObjectPool<T> _pool;

    public abstract event Action CountUpdated;

    public int CountCreated { get; protected set; }
    public int CountSpawned => _pool.CountAll;
    public int CountActive => _pool.CountActive;


    private void Awake()
    {
        _pool = new(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ConfigureOnGet<T>(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    public abstract void ReleaseObject<R>(T obj) where R : Creatable<R>;

    protected abstract void ConfigureOnGet<R>(T obj) where R : Creatable<R>;

    protected abstract void GetObject<R>(R obj = null) where R : Creatable<R>;
}
