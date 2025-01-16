using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private float _createRadius = 2f;

    public override event Action CountUpdated;
    public event Action<Cube> BombCreating;

    private void Start()
    {
        StartCoroutine(CreateRepeating());
    }

    private IEnumerator CreateRepeating()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (true)
        {
            if (_pool.CountInactive > 0 || _pool.CountAll < _poolMaxSize)
            {
                GetObject<Cube>();
                CountCreated++;
                CountUpdated?.Invoke();
            }

            yield return wait;
        }
    }

    protected override void ConfigureOnGet<R>(Cube obj)
    {
        var pos = _originPoint.position;
        pos.x += Random.Range(-_createRadius, _createRadius);
        pos.z += Random.Range(-_createRadius, _createRadius);
        obj.transform.position = pos;
        obj.StopVelocity();
        obj.gameObject.SetActive(true);
        obj.Releasing += ReleaseObject<R>;
    }

    public override void ReleaseObject<R>(Cube obj)
    {
        BombCreating?.Invoke(obj);
        _pool.Release(obj);
        CountUpdated?.Invoke();
        obj.Releasing -= ReleaseObject<R>;
    }

    protected override void GetObject<R>(R obj = null)
    {
        _pool.Get();
    }
}
