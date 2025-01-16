using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    public override event Action CountUpdated;

    private void Awake()
    {
        _pool = new(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ConfigureOnGet<Bomb>(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    private void OnEnable()
    {
        _cubeSpawner.BombCreating += GetObject;
    }

    private void OnDisable()
    {
        _cubeSpawner.BombCreating -= GetObject;
    }

    public override void ReleaseObject<R>(Bomb obj)
    {
        _pool.Release(obj);
        obj.Releasing -= ReleaseObject<R>;
        CountUpdated?.Invoke();
    }

    protected override void ConfigureOnGet<R>(Bomb obj)
    {
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Releasing += ReleaseObject<R>;
    }

    protected override void GetObject<Cube>(Cube cube = null)
    {
        Bomb bomb = _pool.Get();
        bomb.transform.position = cube.transform.position;
        CountUpdated?.Invoke();
        CountCreated++;
    }
}
