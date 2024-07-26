using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public CharacterData _cD;
    public Rigidbody2D _rigidbody2;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody2 != null && photonView.IsMine)
            {
            photonView.RPC("KnockBack", RpcTarget.All);
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
