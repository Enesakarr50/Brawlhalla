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
            // KnockBack iþlemlerini tüm oyunculara gönder
            photonView.RPC("ApplyKnockBack", RpcTarget.Others, photonView.ViewID, _rigidbody2.position, knockBackForce);
        }
    }

    [PunRPC]
    void ApplyKnockBack(int viewID, Vector2 targetPosition, float knockBack)
    {

        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            Rigidbody2D targetRigidbody2D = targetPhotonView.GetComponent<Rigidbody2D>();
            if (targetRigidbody2D != null)
            {
                Vector2 pushDirection = targetPosition - (Vector2)transform.position;
                targetRigidbody2D.velocity = pushDirection * knockBack;
            }

            // Eðer bu istemci nesnenin sahibi ise nesneyi yok et
            if (targetPhotonView.IsMine)
            {
                PhotonNetwork.Destroy(targetPhotonView.gameObject);
            }
        }
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
