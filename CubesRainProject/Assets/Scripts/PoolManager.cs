using UnityEngine;
using UnityEngine.Pool;

public class PoolManager<T> where T : Creatable<T>
{
    protected ObjectPool<T> _pool;

    public int CountSpawned => _pool.CountAll;
    public int CountActive => _pool.CountActive;

    public PoolManager(T prefab, int capacity, int maxSize)
    {
        _pool = new(
            createFunc: () => Object.Instantiate(prefab),
            actionOnGet: (obj) => ConfigureOnGet<T>(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Object.Destroy(obj),
            collectionCheck: true,
            defaultCapacity: capacity,
            maxSize: maxSize
        );
    }

    public T GetObject<R>(R obj = null) where R : Creatable<R>
    {
        return _pool.Get();
    }

    private void ConfigureOnGet<R>(T obj) where R : Creatable<R>
    {
        obj.transform.position = obj.transform.position;
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Released += ReleaseObject<R>;
    }

    private void ReleaseObject<R>(T obj) where R : Creatable<R>
    {
        _pool.Release(obj);
        obj.Released -= ReleaseObject<R>;
    }
}
