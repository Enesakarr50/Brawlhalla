using UnityEngine;
using Photon.Pun;

public class mermi : MonoBehaviourPun
{
    public float knockbackDistance;
    public float knockbackDuration = 0.5f;

    private Vector3 knockbackStartPos;
    private Vector3 knockbackEndPos;
    private float knockbackStartTime;
    private bool isKnockedBack;

    void Update()
    {
        if (isKnockedBack)
        {
            float elapsed = (Time.time - knockbackStartTime) / knockbackDuration;
            if (elapsed < 1f)
            {
                transform.position = Vector3.Lerp(knockbackStartPos, knockbackEndPos, elapsed);
            }
            else
            {
                isKnockedBack = false;
            }
        }
    }

    public void ApplyKnockback(Vector3 direction)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ApplyKnockback", RpcTarget.All, direction);
        }
    }

    [PunRPC]
    public void RPC_ApplyKnockback(Vector3 direction)
    {
        isKnockedBack = true;
        knockbackStartTime = Time.time;
        knockbackStartPos = transform.position;
        knockbackEndPos = transform.position + direction.normalized * knockbackDistance;
        PhotonNetwork.Destroy(gameObject);
    }
}
