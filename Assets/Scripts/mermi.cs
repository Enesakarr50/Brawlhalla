using Fusion;
using System.Collections;
using UnityEngine;

public class Mermi : NetworkBehaviour
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 5f; // Knockback kuvveti

    private void Start()
    {
        _rigidbody2 = GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            _rigidbody2.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with a player and not the local player
        NetworkObject netObject = collision.gameObject.GetComponent<NetworkObject>();
        if (netObject != null && netObject.InputAuthority != Object.InputAuthority)
        {
            _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2 != null)
            {
                Vector2 collisionPoint = collision.contacts[0].point;
                ApplyKnockBack(_rigidbody2, collisionPoint, knockBackForce);
            }

            // If this client is the owner of the object, destroy it
            if (Object.HasInputAuthority)
            {
                Runner.Despawn(Object); // Despawn the NetworkObject
            }
        }
    }



    void ApplyKnockBack(Rigidbody2D targetRigidbody2D, Vector2 collisionPoint, float knockBack)
    {
        if (targetRigidbody2D != null)
        {
            Vector2 pushDirection = (targetRigidbody2D.position - collisionPoint).normalized;
            StartCoroutine(ApplyKnockbackSmooth(targetRigidbody2D, pushDirection * knockBack));
        }
    }

    private IEnumerator ApplyKnockbackSmooth(Rigidbody2D rb, Vector2 force)
    {
        float duration = 0.1f; // Knockback süresini kısalt
        float elapsed = 0f;

        Vector2 initialVelocity = rb.velocity;

        while (elapsed < duration)
        {
            rb.velocity = Vector2.Lerp(initialVelocity, force, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = initialVelocity; // Başlangıç hızına geri dön
    }


    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
