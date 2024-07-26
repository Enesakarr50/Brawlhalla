using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public CharacterData _cD;
    public Rigidbody2D _rigidbody2;
    private void Start()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players)
        {
            PhotonView pv = p.GetComponent<PhotonView>();
            if (!pv.IsMine)
            {
                _rigidbody2 = p.GetComponent<Rigidbody2D>();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            if (_rigidbody2 != null)
            {
            photonView.RPC("KnockBack", RpcTarget.AllViaServer);
            }
    }

    [PunRPC]
    void KnockBack()
    {
        
        Vector2 pushDirection = _rigidbody2.position - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized * 200, ForceMode2D.Impulse);
        PhotonNetwork.Destroy(gameObject);
    }
}
