using Photon.Pun;
using System.Collections;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public CharacterData _cD;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
                if (pv != null && !pv.IsMine)
                {
                    Vector2 pushDirection = collision.transform.position - transform.position;
                    photonView.RPC("KnockBack", RpcTarget.OthersBuffered, pv.ViewID, pushDirection.normalized * 10);
                    StartCoroutine("dest");
                }
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
    IEnumerator dest()
    {
        yield return new WaitForSeconds(0.1f);
        PhotonNetwork.Destroy(gameObject);

    }
}
