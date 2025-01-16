using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Exploder))]
public class Bomb : Creatable <Bomb>
{
    private Exploder _exploder;

    public override event Action<Bomb> Released;

    private new void Awake()
    {
        base.Awake();
        _exploder = GetComponent<Exploder>();
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(WaitBeforeDeath());
    }

    public override void StopVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    protected override IEnumerator WaitBeforeDeath()
    {
        float lifeTime = Random.Range(_minLifeTime, _maxLifeTime);
        float time = 0;
        Color color = _material.color;

        while (time != lifeTime)
        {
            float alpha = Mathf.Lerp(1, 0, time / lifeTime);
            color.a = alpha;
            _material.color = color;

            time = Mathf.MoveTowards(time, lifeTime, Time.deltaTime);

            yield return null;
        }

        _exploder.Explode();
        Released?.Invoke(this);
    }
}
