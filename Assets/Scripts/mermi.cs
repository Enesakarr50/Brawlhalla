using Fusion;
using System.Collections;
using UnityEngine;

public class Mermi : NetworkBehaviour
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 5f; // Knockback force
    private bool isInitialized = false;

    private void Start()
    {
        InitializeRigidbody();
    }

    private void InitializeRigidbody()
    {
        if (isInitialized) return;

        _rigidbody2 = GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            _rigidbody2.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            isInitialized = true;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D not found on Mermi object.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only proceed if collided with a player that is not the local player
        if (!IsRelevantCollision(collision)) return;

        ApplyKnockBack(collision);

        // Destroy the object if this client has authority
        if (Object.HasInputAuthority)
        {
            Runner.Despawn(Object); // Despawn the NetworkObject
        }
    }

    private bool IsRelevantCollision(Collision2D collision)
    {
        NetworkObject netObject = collision.gameObject.GetComponent<NetworkObject>();
        return netObject != null && netObject.InputAuthority != Object.InputAuthority;
    }

    private void ApplyKnockBack(Collision2D collision)
    {
        Rigidbody2D targetRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if (targetRigidbody != null)
        {
            Vector2 collisionPoint = collision.contacts[0].point;
            Vector2 pushDirection = (targetRigidbody.position - collisionPoint).normalized;
            StartCoroutine(ApplyKnockbackSmooth(targetRigidbody, pushDirection * knockBackForce));
        }
    }

    private IEnumerator ApplyKnockbackSmooth(Rigidbody2D rb, Vector2 force)
    {
        float duration = 0.1f; // Reduced knockback duration
        Vector2 initialVelocity = rb.velocity;
        Vector2 targetVelocity = initialVelocity + force;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            rb.velocity = Vector2.Lerp(initialVelocity, targetVelocity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = targetVelocity; // Maintain the final knockback velocity
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}


