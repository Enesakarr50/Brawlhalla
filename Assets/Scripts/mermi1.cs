/*using Photon.Pun;
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
            NetworkObject.RPC("ApplyKnockBack", Rpc.All, NetworkObject.ViewID, _rigidbody2.position, knockBackForce);
        }
    }

    [PunRPC]
    void ApplyKnockBack(int viewID, Vector2 targetPosition, float knockBack)
    {

        NetworkObject targetNetworkObject = NetworkObject.Find(viewID);
        if (targetNetworkObject != null)
        {
            Rigidbody2D targetRigidbody2D = targetNetworkObject.GetComponent<Rigidbody2D>();
            if (targetRigidbody2D != null)
            {
                Vector2 pushDirection = targetPosition - (Vector2)transform.position;
                targetRigidbody2D.AddForce(pushDirection.normalized * knockBack, ForceMode2D.Impulse);
            }

            // E�er bu istemci nesnenin sahibi ise nesneyi yok et
            if (targetNetworkObject.IsMine)
            {
                PhotonNetwork.Destroy(targetNetworkObject.gameObject);
            }
        }
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
*/