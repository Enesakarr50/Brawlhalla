using Photon.Pun;
using UnityEngine;

public class mermi : MonoBehaviourPun
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float knockBackForce = 1f; // Knockback kuvveti

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            // KnockBack iþlemlerini tüm oyunculara gönder
            photonView.RPC("ApplyKnockBack", RpcTarget.All, photonView.ViewID, _rigidbody2.position);
        }
    }

    [PunRPC]
    void ApplyKnockBack(int viewID, Vector3 targetPosition, float knockBack)
    {

        for(int i=0; i<5; i++) 
        {
            PhotonView targetPhotonView = PhotonView.Find(viewID);
             if (targetPhotonView != null)
            {
                targetPhotonView.gameObject.transform.position += targetPosition;
            }
        
        }


       
    }

    public void SetKnockBack(float knockBack)
    {
        knockBackForce = knockBack;
    }
}
