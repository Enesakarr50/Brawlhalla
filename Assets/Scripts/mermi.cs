using Photon.Pun;
using System.Collections;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 5f; // Knockback kuvveti

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            // KnockBack iþlemlerini tüm oyunculara gönder
            PhotonView targetPhotonView = collision.gameObject.GetComponent<PhotonView>();
            if (targetPhotonView != null)
            {
                Vector2 collisionPoint = collision.contacts[0].point;
                photonView.RPC("ApplyKnockBack", RpcTarget.AllBuffered, targetPhotonView.ViewID, collisionPoint, knockBackForce);
            }
        }
    }

    [PunRPC]
    void ApplyKnockBack(int viewID, Vector2 collisionPoint, float knockBack)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            Rigidbody2D targetRigidbody2D = targetPhotonView.GetComponent<Rigidbody2D>();
            if (targetRigidbody2D != null)
            {
                Vector2 pushDirection = (targetRigidbody2D.position - collisionPoint).normalized;
                StartCoroutine(ApplyKnockbackSmooth(targetRigidbody2D, pushDirection * knockBack));

                // Eðer bu istemci nesnenin sahibi ise nesneyi yok et
                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    private IEnumerator ApplyKnockbackSmooth(Rigidbody2D rb, Vector2 force)
    {
        float duration = 0.2f; // Knockback süresi
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
