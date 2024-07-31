using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviourPunCallbacks
{
    public CharacterData[] Characters; // Karakterlerin listesi
    private CharacterData selectedCharacter; // Seçilen karakterin verisi

    // Player UI elementleri
    public Image CharImageUI;
    public Image WeaponImageUI;
    public Image PassiveSkillUI;
    public Image ActiveSkillUI;

    private void Start()
    {
        // Seçilen karakteri Photon üzerinden al
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("SelectedCharacterIndex", out object index))
        {
            int selectedIndex = (int)index;
            SelectCharacter(selectedIndex);
        }
    }

    // Karakter seçimi için metod
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= Characters.Length)
        {
            Debug.LogError("Geçersiz karakter indeksi: " + index);
            return;
        }

        selectedCharacter = Characters[index];

        // UI'ý güncelle
        UpdateUI();

        // Seçilen karakter bilgisini Photon üzerinden diðer oyunculara bildir
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
        {
<<<<<<< Updated upstream
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
=======
            { "SelectedCharacterIndex", index }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
>>>>>>> Stashed changes
    }

    // UI elementlerini güncelle
    private void UpdateUI()
    {
        if (selectedCharacter != null)
        {
            CharImageUI.sprite = selectedCharacter.CharacterImage;
            WeaponImageUI.sprite = selectedCharacter.WeaponImage;
            PassiveSkillUI.sprite = selectedCharacter._passiveSkill;
            ActiveSkillUI.sprite = selectedCharacter._activeSkill;
        }
    }

    // Oyuna baþlama metodu
    public void StartGame()
    {
        if (selectedCharacter != null)
        {
            // Sahneye geçiþ yapmadan önce tüm oyuncularýn karakter verilerini senkronize edin
            PhotonNetwork.LoadLevel("GameScene"); // Oyuna baþlayacaðýnýz sahne adýný buraya yazýn
        }
        else
        {
            Debug.Log("Karakter seçilmedi. Karakter seçmeniz gerekiyor!");
        }
    }

    // Sahne yüklendiðinde oyuncunun karakterini instantiate et
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            // MasterClient olarak, tüm oyuncularýn karakterlerini instantiate edin
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue("SelectedCharacterIndex", out object index))
                {
                    int selectedIndex = (int)index;
                    CharacterData characterData = Characters[selectedIndex];
                    Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), 0, 0); // Rastgele bir spawn pozisyonu
                    PhotonNetwork.Instantiate(characterData.InGamePlayer.name, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
<<<<<<< Updated upstream

    public void SpawnPlayer()
    {
        if (CurrentData != null)
        {
            // Optional: Save the selected character data to Photon Player properties
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "SelectedCharacterIndex", CurrentData.Index}
        });

            SceneManager.LoadScene(1); // Load the game scene
        }
        else
        {
            Debug.Log("No character selected! Please select a character.");
        }
    }
}
=======
}
>>>>>>> Stashed changes
