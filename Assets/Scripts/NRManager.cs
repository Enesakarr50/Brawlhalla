using Fusion.Sockets;
using Fusion;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using Fusion.Photon.Realtime;

/// <summary>
/// NetworkRunnerManager is way we start our connection to Fusion
/// </summary>
public class NetworkRunnerManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public event Action OnStartedRunnerConnection;
    public event Action<NetworkRunner, PlayerRef> OnPlayerLeftRoom;
    public event Action<NetworkRunner, PlayerRef> OnPlayerJoinedSuccessFully;

    public GameObject PlayerPrefab;
    public string LocalPlayerNickName { get; private set; }

    [SerializeField] private NetworkRunner networkRunnerPrefab;
    private NetworkRunner runnerInstance;

    //Preforms a shutdown which will lead to a call
    //to the function below "OnShutdown", hence will load the Lobby scene
    public void ShutDownRunner()
    {
        runnerInstance.Shutdown();
    }

    //Setting our nickname here, this object is under DDOL
    //So we will have it during runtime
    public void SetLocalNickname(string nickName)
    {
        this.LocalPlayerNickName = nickName;
    }

    //Starts the connection, gets called once we created or joined a room
    public async void StartGame(GameMode mode, string roomName)
    {
        OnStartedRunnerConnection?.Invoke();

        if (runnerInstance == null)
        {
            runnerInstance = Instantiate(networkRunnerPrefab);
        }

        // Let the Fusion Runner know that we will be providing user input
        runnerInstance.ProvideInput = true;

        //Register so we will get the callbacks as well
        runnerInstance.AddCallbacks(this);

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            PlayerCount = 4,
            SceneManager = runnerInstance.GetComponent<NetworkSceneManagerDefault>()
        };


        // GameMode.Host = Start a session with a specific name
        // GameMode.Client = Join a session with a specific name
        var result = await runnerInstance.StartGame(startGameArgs);
        if (runnerInstance.IsServer)
        {
            if (result.Ok)
            {
                const string MAIN_SCENE_NAME = "SampleScene";
               // runnerInstance.LoadScene(MAIN_SCENE_NAME);
            }
            else
            {
                Debug.LogError($"Failed to Start: {result.ShutdownReason}");
            }
        }
    }


    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        OnPlayerJoinedSuccessFully?.Invoke(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerLeft");
        OnPlayerLeftRoom?.Invoke(runner, player);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("OnInputMissing");
    }

    // When the local NetworkRunner has shut down, the menu scene is loaded.
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("OnShutdown");
        const string LOBBY_SCENE = "Lobby";
        SceneManager.LoadScene(LOBBY_SCENE);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("OnConnectRequest");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("OnConnectFailed");
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("OnUserSimulationMessage");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("OnSessionListUpdated");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("OnReliableDataReceived");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadDone");
        NetworkRunner player = GameObject.FindGameObjectWithTag("nr").GetComponent<NetworkRunner>();
        if(runner.LocalPlayer != null)
        {
            player.Spawn(PlayerPrefab);
        }
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("OnSceneLoadStart");
    }
}
