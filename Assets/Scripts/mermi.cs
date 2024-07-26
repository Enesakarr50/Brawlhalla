using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public CharacterData _cD;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine)
            {
                // �arp��ma noktas�n�n normalini tersine �evirerek y�n� belirleyin
                Vector2 pushDirection = -collision.contacts[0].normal;
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, pushDirection.normalized * 10);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    [PunRPC]
    void KnockBack(int viewID, Vector2 force)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            Rigidbody2D rb = pv.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
