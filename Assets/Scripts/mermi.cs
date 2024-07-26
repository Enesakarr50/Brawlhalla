using Photon.Pun;
using UnityEngine;

public class Mermi : MonoBehaviourPun
{
    public CharacterData _cD;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
