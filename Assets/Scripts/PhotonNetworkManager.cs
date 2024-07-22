using Photon.Pun;
using UnityEngine;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        if (playerPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-2f, 2f), 0, 0);
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab is not set.");
        }
    }
}
