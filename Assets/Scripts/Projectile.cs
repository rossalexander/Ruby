using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        _rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var e = other.collider.GetComponent<EnemyController>();
        if (e)
        {
            e.Fix();
        }

        Destroy(gameObject);
    }
}