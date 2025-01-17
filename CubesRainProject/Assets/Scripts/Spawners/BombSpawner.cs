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

    protected override void ConfigureObject(Bomb obj)
    {
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Released += ReleaseObject;
    }

    private void ReleaseObject(Bomb obj)
    {
        obj.Released -= ReleaseObject;
        CountUpdated?.Invoke();
    }

    private void SpawnBomb(Cube cube)
    {
        Bomb bomb = Spawn();
        bomb.transform.position = cube.transform.position;
        CountUpdated?.Invoke();
    }
}
