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
                Vector2 pushDirection = collision.transform.position - transform.position;
                photonView.RPC("KnockBack", RpcTarget.All, pv.ViewID, pushDirection.normalized * 10);

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
        }
    }
}
