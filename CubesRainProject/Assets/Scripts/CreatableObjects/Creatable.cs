using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public abstract class Creatable <T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected float _minLifeTime = 2f;
    [SerializeField] protected float _maxLifeTime = 5f;

    protected Coroutine _coroutine;
    protected Rigidbody _rigidbody;
    protected Material _material;

    public abstract event Action<T> Released;

    protected void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _material = GetComponent<Renderer>().material;
    }

    public abstract void StopVelocity();

    protected abstract IEnumerator WaitBeforeDeath();
}
