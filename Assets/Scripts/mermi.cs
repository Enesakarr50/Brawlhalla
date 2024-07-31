using Photon.Pun;
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
            // KnockBack ve Destroy iþlemlerini tüm oyunculara gönder
            photonView.RPC("ApplyKnockBack", RpcTarget.All, _rigidbody2.position, knockBackForce);

            // Eðer bu istemci nesnenin sahibi veya MasterClient ise nesneyi yok et
            if (photonView.IsMine || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void ApplyKnockBack(Vector2 targetPosition, float knockBack)
    {
        Vector2 pushDirection = targetPosition - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized * knockBack, ForceMode2D.Impulse);
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
