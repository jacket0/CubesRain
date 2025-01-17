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
        Coroutine = StartCoroutine(WaitBeforeDeath());
    }

    public override void StopVelocity()
    {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    protected override IEnumerator WaitBeforeDeath()
    {
        float lifeTime = Random.Range(MinLifeTime, MaxLifeTime);
        float time = 0;
        Color color = Material.color;

        while (time != lifeTime)
        {
            float alpha = Mathf.Lerp(1, 0, time / lifeTime);
            color.a = alpha;
            Material.color = color;

            time = Mathf.MoveTowards(time, lifeTime, Time.deltaTime);

            yield return null;
        }

        _exploder.Explode();
        Released?.Invoke(this);
    }
}
