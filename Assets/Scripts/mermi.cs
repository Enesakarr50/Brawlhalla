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
                    Vector3 pushDirection = new Vector3 (collision.transform.position.x - transform.position.x, 0,0);
                    photonView.RPC("KnockBack", RpcTarget.OthersBuffered, pv.ViewID, pushDirection.normalized * 10);
                    
                }
            }


        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void KnockBack(int viewID, Vector3 force)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            Rigidbody2D rb = pv.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.transform.position += force;

                
            }
        }
    }
}
