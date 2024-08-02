using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour
{
    public GameObject PlayerPrefab;
    public void go() 
    {
     
        Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        
    }
}
