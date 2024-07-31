using Photon.Pun;
using UnityEngine;
using System.Collections;

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
            if (photonView.IsMine)
            {
                // KnockBack ve Destroy iþlemlerini tüm oyunculara gönder
                photonView.RPC("ApplyKnockBackAndDestroy", RpcTarget.All, _rigidbody2.position, knockBackForce);
            }
        }
    }

    [PunRPC]
    void ApplyKnockBackAndDestroy(Vector2 targetPosition, float knockBack)
    {
        Vector2 pushDirection = targetPosition - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized * knockBack, ForceMode2D.Impulse);

        // Mermiyi yok et
        PhotonNetwork.Destroy(gameObject);
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
