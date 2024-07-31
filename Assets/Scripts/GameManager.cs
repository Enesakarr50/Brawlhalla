using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] PlayerPrefabs;
    public Transform[] SpawnPoints;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            object selectedIndex;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("SelectedCharacterIndex", out selectedIndex))
            {
                int index = (int)selectedIndex;
                Vector3 spawnPosition = SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1].position;
                PhotonNetwork.Instantiate(PlayerPrefabs[index].name, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Selected character index not found.");
            }
        }
        else
        {
            Debug.LogError("Not connected to Photon Network.");
        }
    }
}