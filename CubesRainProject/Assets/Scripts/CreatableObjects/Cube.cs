using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : Creatable<Cube>
{
    private bool _isTouchedPlatform = false;

    public override event Action<Cube> Released;
    public event Action<Vector3> PositionSetted;

    private new void Awake() =>
        base.Awake();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform))
        {
            TouchPlatform();
        }
    }

    private void OnDisable()
    {
        _isTouchedPlatform = false;
        Material.color = Color.white;

        if (Coroutine != null)
            StopCoroutine(Coroutine);
    }

    public override void StopVelocity()
    {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    protected override IEnumerator WaitBeforeDeath()
    {
        var time = new WaitForSeconds(Random.Range(MinLifeTime, MaxLifeTime));

        yield return time;

        Released?.Invoke(this);
    }

    private void TouchPlatform()
    {
        if (_isTouchedPlatform == false)
        {
            _isTouchedPlatform = true;
            Material.color = Random.ColorHSV();
            Coroutine = StartCoroutine(WaitBeforeDeath());
            PositionSetted?.Invoke(transform.position);
        }
    }
}