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

    private void OnDisable()
    {
        _isTouchedPlatform = false;
        _material.color = Color.white;

        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public override void StopVelocity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void TouchPlatform()
    {
        if (_isTouchedPlatform == false)
        {
            _isTouchedPlatform = true;
            _material.color = Random.ColorHSV();
            _coroutine = StartCoroutine(WaitBeforeDeath());
            PositionSetted?.Invoke(transform.position);
        }
    }

    protected override IEnumerator WaitBeforeDeath()
    {
        var time = new WaitForSeconds(Random.Range(_minLifeTime, _maxLifeTime));

        yield return time;

        Released?.Invoke(this);
    }
}