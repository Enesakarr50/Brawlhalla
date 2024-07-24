using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class mermi : MonoBehaviour
{
    public CharacterData _cD;

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 pushDirection = rb.position - (Vector2)transform.position;
            rb.AddForce(pushDirection.normalized * _cD.KnockBackRate, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}
