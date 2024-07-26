using Photon.Pun;
using UnityEngine;

public class Mermi : MonoBehaviourPun
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
