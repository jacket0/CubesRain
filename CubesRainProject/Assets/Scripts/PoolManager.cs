using UnityEngine;
using UnityEngine.Pool;

public class PoolManager<T> where T : Creatable<T>
{
    private ObjectPool<T> _pool;

    public int CountSpawned => _pool.CountAll;
    public int CountActive => _pool.CountActive;

    public PoolManager(T prefab, int capacity, int maxSize)
    {
        _pool = new(
            createFunc: () => Object.Instantiate(prefab),
            actionOnGet: (obj) => ConfigureOnGet(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Object.Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: capacity,
            maxSize: maxSize
        );
    }

    public T GetObject(T obj = null)
    {
        return _pool.Get();
    }

    private void ConfigureOnGet(T obj)
    {
        obj.transform.position = obj.transform.position;
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Released += ReleaseObject;
    }

    private void ReleaseObject(T obj) 
    {
        _pool.Release(obj);
        obj.Released -= ReleaseObject;
    }
}
