using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public abstract class Creatable <T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected float MinLifeTime = 2f;
    [SerializeField] protected float MaxLifeTime = 5f;

    protected Coroutine Coroutine;
    protected Rigidbody Rigidbody;
    protected Material Material;

    public abstract event Action<T> Released;

    protected void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Material = GetComponent<Renderer>().material;
    }

    public abstract void StopVelocity();

    protected abstract IEnumerator WaitBeforeDeath();
}
