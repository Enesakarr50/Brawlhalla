using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    public CharacterData _cD;
    public Rigidbody2D _rigidbody2;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            photonView.RPC("isKock", RpcTarget.AllBuffered, true);
        }
    }

    [PunRPC]
    void KnockBack(bool isKock)
    {
        Vector2 pushDirection = _rigidbody2.position - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized * _cD.KnockBackRate, ForceMode2D.Impulse);
        Destroy(gameObject);
    }
}
