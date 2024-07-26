using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public float pushForce = 200f; // Ýtme kuvveti
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction; // Merminin hareket yönü
    public CharacterData _cD;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _direction = _rigidbody2D.velocity.normalized; // Baþlangýçta merminin yönünü belirleyin
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine) // Çarpýlan oyuncu yerel deðilse
            {
                // Merminin hareket yönünü gönder
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, _direction);
            }
        }
    }

    [PunRPC]
    void KnockBack(int targetViewID, Vector2 direction)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);
        if (targetView != null)
        {
            Rigidbody2D rb = targetView.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Merminin geldiði yönün tersine itme
                Vector2 pushDirection = -direction;
                rb.AddForce(pushDirection.normalized * -pushForce);
            }
        }

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
