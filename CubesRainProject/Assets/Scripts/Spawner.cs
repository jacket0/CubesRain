using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
	[SerializeField] private Cube _cube;
	[SerializeField] private Transform _originPoint;
	[SerializeField] private int _poolCapacity = 5;
	[SerializeField] private int _poolMaxSize = 5;
	[SerializeField] private float _repeatRate = 1f;
	[SerializeField] private float _createRadius = 2f;

	private ObjectPool<Cube> _pool;

	public void ReleaseCube(Cube obj)
	{
		_pool.Release(obj);
		obj.Released -= ReleaseCube;
	}

	private void Awake()
	{
		_pool = new(
			createFunc: () => Instantiate(_cube),
			actionOnGet: (obj) => ActionOnGet(obj),
			actionOnRelease: (obj) => obj.gameObject.SetActive(false),
			actionOnDestroy: (obj) => Destroy(obj),
		collectionCheck: true,
		defaultCapacity: _poolCapacity,
		maxSize: _poolMaxSize);
	}

	private void ActionOnGet(Cube obj)
	{
		var pos = _originPoint.position;
		pos.x += Random.Range(-_createRadius, _createRadius);
		pos.z += Random.Range(-_createRadius, _createRadius);
		obj.transform.position = pos;
		obj.StopVelocity();
		obj.gameObject.SetActive(true);
		obj.Released += ReleaseCube;
	}

	private void Start()
	{
		InvokeRepeating(nameof(GetCube), 0f, _repeatRate);
	}

	private void GetCube()
	{
		_pool.Get();
	}
}
