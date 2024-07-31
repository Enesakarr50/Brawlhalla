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
                photonView.RPC("KnockBack", RpcTarget.AllBufferedViaServer, _rigidbody2.position);
            }

            // Mermi deðdiðinde yok et
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void KnockBack(Vector2 targetPosition)
    {
        Vector2 pushDirection = targetPosition - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized * knockBackForce, ForceMode2D.Impulse);
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
