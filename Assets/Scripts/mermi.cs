using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        PhotonNetwork.Destroy(gameObject);
    }


    private void Update()
    {
        if (_rigidbody2 != null && photonView.IsMine)
        {
            _rigidbody2.velocity = new Vector2(1, 0);
        }
        
    }
}
