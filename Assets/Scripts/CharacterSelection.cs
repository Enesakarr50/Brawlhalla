using Photon.Pun;
using System.ComponentModel;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviourPunCallbacks
{
    public CharacterData[] Characters;
    public CharacterData CurrentData;
    public GameObject PlayerPrefab;

    // Player 1 UI elements
    public Image CharImagePlayer1;
    public Image WeaponImagePlayer1;
    public Image PassiveSkillPlayer1;
    public Image ActiveSkillPlayer1;

    // Player 2 UI elements
    public Image CharImagePlayer2;
    public Image WeaponImagePlayer2;
    public Image PassiveSkillPlayer2;
    public Image ActiveSkillPlayer2;

    private void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("Index", -1);
        if (selectedIndex != -1)
        {
            ChooseChar(selectedIndex);
        }
    }

    // Seçim metodu
    public void ChooseChar(int index)
    {
        if (Characters == null || Characters.Length == 0)
        {
            Debug.LogError("Characters array is null or empty.");
            return;
        }

        if (index < 0 || index >= Characters.Length)
        {
            Debug.LogError("Index out of bounds: " + index);
            return;
        }

        CurrentData = Characters[index];
        if (CurrentData == null)
        {
            Debug.LogError("Character data at index " + index + " is null.");
            return;
        }

        if (PlayerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is null.");
            return;
        }

        Player playerComponent = PlayerPrefab.GetComponent<Player>();
        if (playerComponent == null)
        {
            Debug.LogError("Player component not found on PlayerPrefab.");
            return;
        }

        playerComponent.Character = CurrentData;

        // Send an RPC to update the UI for all players
        bool isMine = photonView.IsMine;
        photonView.RPC("UpdateUIForAllPlayers", RpcTarget.AllBuffered, isMine, index);
        PlayerPrefs.SetInt("Index", index);
    }

    [PunRPC]
    private void UpdateUIForAllPlayers(bool isMine, int index)
    {
        CurrentData = Characters[index];

        if (isMine)
        {
            UpdateUIForPlayer1();
        }
        else
        {
            UpdateUIForPlayer2();
        }
    }

    private void UpdateUIForPlayer1()
    {
        if (CharImagePlayer1 != null)
        {
            CharImagePlayer1.sprite = CurrentData.CharacterImage;
        }
        if (WeaponImagePlayer1 != null)
        {
            WeaponImagePlayer1.sprite = CurrentData.WeaponImage;
        }
        if (PassiveSkillPlayer1 != null)
        {
            PassiveSkillPlayer1.sprite = CurrentData._passiveSkill;
        }
        if (ActiveSkillPlayer1 != null)
        {
            ActiveSkillPlayer1.sprite = CurrentData._activeSkill;
        }
    }

    private void UpdateUIForPlayer2()
    {
        if (CharImagePlayer2 != null)
        {
            CharImagePlayer2.sprite = CurrentData.CharacterImage;
        }
        if (WeaponImagePlayer2 != null)
        {
            WeaponImagePlayer2.sprite = CurrentData.WeaponImage;
        }
        if (PassiveSkillPlayer2 != null)
        {
            PassiveSkillPlayer2.sprite = CurrentData._passiveSkill;
        }
        if (ActiveSkillPlayer2 != null)
        {
            ActiveSkillPlayer2.sprite = CurrentData._activeSkill;
        }
    }

    public void SpawnPlayer()
    {
        if (CurrentData != null)
        {
            photonView.RPC("SpawnPlayerRPC", RpcTarget.All);
        }
        else
        {
            Debug.Log("Karakter Seçilmedi. Karakter seçilmesi lazým!");
        }
    }
    [PunRPC]
    public void SpawnPlayerRPC()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        DontDestroyOnLoad(players[0]);
        DontDestroyOnLoad(players[1]);
        SceneManager.LoadScene(1);
    }
}
