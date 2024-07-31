using Photon.Pun;
using UnityEngine;
using System.Collections;

public class mermi : MonoBehaviourPun
{
    [SerializeField] private CharacterData _cD;
    private Rigidbody2D _rigidbody2;
    private float  kncokBack; // Knockback kuvveti
    [SerializeField] private float destroyDelay = 2f;    // Yok edilme gecikmesi

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2 = collision.gameObject.GetComponent<Rigidbody2D>();
        if (_rigidbody2 != null)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("KnockBack", RpcTarget.All, _rigidbody2.position);
            }
        }
    }

    [PunRPC]
    void KnockBack(Vector2 targetPosition)
    {
        Vector2 pushDirection = targetPosition - (Vector2)transform.position;
        _rigidbody2.AddForce(pushDirection.normalized *   kncokBack, ForceMode2D.Impulse);

        if (photonView.IsMine)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        PhotonNetwork.Destroy(gameObject);
    }
}