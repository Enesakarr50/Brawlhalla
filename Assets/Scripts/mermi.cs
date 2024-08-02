using Photon.Pun;
using System.Collections;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 1f; // Knockback kuvveti

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            // KnockBack i�lemlerini t�m oyunculara g�nder
            photonView.RPC("ApplyKnockBack", RpcTarget.All, photonView.ViewID, _rigidbody2.position, _rigidbody2.velocity, knockBackForce);
        }
    }

    [PunRPC]
    void ApplyKnockBack(int viewID, Vector2 targetPosition, Vector2 targetVelocity, float knockBack)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            Rigidbody2D targetRigidbody2D = targetPhotonView.GetComponent<Rigidbody2D>();
            if (targetRigidbody2D != null)
            {
                Vector2 pushDirection = targetRigidbody2D.position - (Vector2)transform.position;
                StartCoroutine(ApplyKnockbackSmooth(targetRigidbody2D, pushDirection.normalized * knockBack));

                // E�er bu istemci nesnenin sahibi ise nesneyi yok et
                if (targetPhotonView.IsMine)
                {
                    PhotonNetwork.Destroy(photonView.gameObject);
                }
            }
        }
    }

    private IEnumerator ApplyKnockbackSmooth(Rigidbody2D rb, Vector2 force)
    {
        float duration = 0.2f; // Knockback s�resi
        float elapsed = 0f;

        Vector2 initialVelocity = rb.velocity;
        Vector2 targetVelocity = force;

        while (elapsed < duration)
        {
            rb.velocity = Vector2.Lerp(initialVelocity, targetVelocity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.velocity = targetVelocity;
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
