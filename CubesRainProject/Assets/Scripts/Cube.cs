using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
	private float _minLifeTime = 2f;
	private float _maxLifeTime = 5f;
	private bool _isTouchedPlatform = false;
	private Coroutine _coroutine;

	public event Action<Cube> Released;

	private Rigidbody _rigidbody;
	private Material _material;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_material = GetComponent<Renderer>().material;
	}

	private void OnDisable()
	{
		_isTouchedPlatform = false;
		_material.color = Color.white;

		if (_coroutine != null)
			StopCoroutine(_coroutine);
	}

	public void StopVelocity()
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
		}
	}

	private IEnumerator WaitBeforeDeath()
	{
		var time = new WaitForSeconds(Random.Range(_minLifeTime, _maxLifeTime));
		yield return time;
		Released?.Invoke(this);
	}
}
