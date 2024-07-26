using Photon.Pun;
using System.Collections;
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
                Vector2 pushDirection = -collision.contacts[0].normal;
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, pushDirection.normalized * 200);
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
                // Kuvvet uygulamasýný FixedUpdate içerisinde yapýn
                StartCoroutine(ApplyForceAfterDelay(rb, force, 0.1f));
            }
        }
    }

    private IEnumerator ApplyForceAfterDelay(Rigidbody2D rb, Vector2 force, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero; // Hýzý sýfýrla
        rb.AddForce(force, ForceMode2D.Impulse); // Kuvveti uygula
    }
}
