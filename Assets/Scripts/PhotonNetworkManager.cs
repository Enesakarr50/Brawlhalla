using Photon.Pun;
using UnityEngine;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{

    void Start()
    {

        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.IncomingLag = 100; // Gelen paketler i�in gecikme (milisaniye)
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.OutgoingLag = 100; // Giden paketler i�in gecikme (milisaniye)
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.IncomingJitter = 10; // Gelen paketler i�in dalgalanma (milisaniye)
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.OutgoingJitter = 10; // Giden paketler i�in dalgalanma (milisaniye)
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.IncomingLossPercentage = 1; // Gelen paketler i�in paket kayb� (y�zde)
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.NetworkSimulationSettings.OutgoingLossPercentage = 1; // Giden paketler i�in paket kayb� (y�zde)
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

    
}
