using Photon.Pun;
using UnityEngine;

public class Mermi : MonoBehaviourPun
{
    private Rigidbody2D _rigidbody2D;
    private Vector2 _direction; // Merminin hareket y�n�
    public CharacterData _cD;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _direction = _rigidbody2D.velocity.normalized; // Ba�lang��ta merminin y�n�n� belirleyin
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine) // �arp�lan oyuncu yerel de�ilse
            {
                // Merminin hareket y�n�n� g�nder
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, (Vector2)_direction);
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
                // Apply knockback using Rigidbody2D's velocity
                rb.velocity = direction * _cD.KnockBackRate;
            }
        }
    }
}
