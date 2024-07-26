using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    private Rigidbody2D targetRigidbody;
    public CharacterData _cD;

    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (!pv.IsMine) // Yerel oyuncu deðilse
            {
                targetRigidbody = player.GetComponent<Rigidbody2D>();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (pv != null && !pv.IsMine) // Çarpýlan oyuncu yerel deðilse
            {
                // Merminin hareket yönünü hesaplayýn
                Vector2 collisionNormal = collision.contacts[0].normal;
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, collisionNormal);
            }
        }
    }

    [PunRPC]
    void KnockBack(int targetViewID, Vector2 collisionNormal)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);
        if (targetView != null)
        {
            Rigidbody2D rb = targetView.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Çarpma yönünün tersine itme
                Vector2 pushDirection = -collisionNormal;
                rb.AddForce(pushDirection.normalized * 200, ForceMode2D.Impulse);
            }
        }

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
