using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _radius = 300f;
    [SerializeField] private float _force = 300f;

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Rigidbody exploding in GetExplodingObjects(colliders))
            exploding.AddExplosionForce(_force, transform.position, _radius);
    }

    private List<Rigidbody> GetExplodingObjects(Collider[] colliders)
    {
        List<Rigidbody> cubes = new List<Rigidbody>();

        foreach (Collider collider in colliders)
        {
            if (collider.attachedRigidbody != null)
                cubes.Add(collider.attachedRigidbody);
        }

        return cubes;
    }
}
