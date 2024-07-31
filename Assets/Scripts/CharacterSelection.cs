using Photon.Pun;
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

    // Character selection method
    public void ChooseChar(int index)
    {
        if (Characters == null || Characters.Length == 0 || index < 0 || index >= Characters.Length)
        {
            Debug.LogError("Invalid character selection.");
            return;
        }

        CurrentData = Characters[index];
        if (CurrentData == null || PlayerPrefab == null)
        {
            Debug.LogError("Invalid character data or PlayerPrefab is null.");
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
        photonView.RPC("UpdateUIForAllPlayers", RpcTarget.AllBuffered, index);
        PlayerPrefs.SetInt("Index", index);
    }

    [PunRPC]
    private void UpdateUIForAllPlayers(int index)
    {
        CurrentData = Characters[index];

        if (photonView.IsMine)
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
        SetUI(CharImagePlayer1, WeaponImagePlayer1, PassiveSkillPlayer1, ActiveSkillPlayer1);
    }

    private void UpdateUIForPlayer2()
    {
        SetUI(CharImagePlayer2, WeaponImagePlayer2, PassiveSkillPlayer2, ActiveSkillPlayer2);
    }

    private void SetUI(Image charImage, Image weaponImage, Image passiveSkillImage, Image activeSkillImage)
    {
        if (charImage != null) charImage.sprite = CurrentData.CharacterImage;
        if (weaponImage != null) weaponImage.sprite = CurrentData.WeaponImage;
        if (passiveSkillImage != null) passiveSkillImage.sprite = CurrentData._passiveSkill;
        if (activeSkillImage != null) activeSkillImage.sprite = CurrentData._activeSkill;
    }

    public void SpawnPlayer()
    {
        if (CurrentData != null)
        {
            PhotonNetwork.Instantiate(PlayerPrefab.name, Vector3.zero, Quaternion.identity);
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Character not selected. Please select a character.");
        }
    }
}
