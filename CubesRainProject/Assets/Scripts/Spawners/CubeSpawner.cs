using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _createRadius = 2f;
    [SerializeField] private Transform _originPoint;

    public override event Action CountUpdated;
    public event Action<Cube> CubeReleased;

    private void Start()
    {
        StartCoroutine(CreateRepeating());
    }

    protected override void ConfigureObject(Cube obj)
    {
        Vector3 pos = _originPoint.position;
        pos.x += Random.Range(-_createRadius, _createRadius);
        pos.z += Random.Range(-_createRadius, _createRadius);
        obj.transform.position = pos;
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Released += ReleaseObject;
    }

    private IEnumerator CreateRepeating()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (true)
        {
            if (CountSpawned - CountActive > 0 || CountSpawned < _poolMaxSize)
            {
                Spawn();
                CountUpdated?.Invoke();
            }

            yield return wait;
        }
    }

    private void ReleaseObject(Cube obj)
    {
        CubeReleased?.Invoke(obj);
       obj.Released -= ReleaseObject;
        CountUpdated?.Invoke();
    }
}
