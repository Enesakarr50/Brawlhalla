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
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.z = 0;
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, pushDirection.normalized * _cD.KnockBackRate);
            }
        }
    }

    [PunRPC]
    void KnockBack(int viewID, Vector3 force)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv != null)
        {
            pv.gameObject.transform.position += force;
            PhotonNetwork.Destroy(gameObject);
        }
        
    }
}