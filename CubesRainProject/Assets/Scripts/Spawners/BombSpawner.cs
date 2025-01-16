using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    public override event Action CountUpdated;

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += SpawnBomb;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= SpawnBomb;
    }

    protected override void ConfigureObject<R>(Bomb obj)
    {
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Released += ReleaseObject<R>;
    }

    private void ReleaseObject<R>(Bomb obj)
    {
        obj.Released -= ReleaseObject<R>;
        CountUpdated?.Invoke();
    }

    private void SpawnBomb<Cube>(Cube cube) where Cube : Creatable<Cube>
    {
        Bomb bomb = Spawn<Bomb>();
        bomb.transform.position = cube.transform.position;
        CountUpdated?.Invoke();
    }
}
