using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviourPunCallbacks
{
    public CharacterData[] Characters; // Karakterlerin listesi
    private CharacterData selectedCharacter; // Se�ilen karakterin verisi

    // Player UI elementleri
    public Image CharImageUI;
    public Image WeaponImageUI;
    public Image PassiveSkillUI;
    public Image ActiveSkillUI;

    private void Start()
    {
        // Se�ilen karakteri Photon �zerinden al
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("SelectedCharacterIndex", out object index))
        {
            int selectedIndex = (int)index;
            SelectCharacter(selectedIndex);
        }
    }

    // Karakter se�imi i�in metod
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= Characters.Length)
        {
            Debug.LogError("Ge�ersiz karakter indeksi: " + index);
            return;
        }

        selectedCharacter = Characters[index];

        // UI'� g�ncelle
        UpdateUI();

        // Se�ilen karakter bilgisini Photon �zerinden di�er oyunculara bildir
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

    // UI elementlerini g�ncelle
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

    // Oyuna ba�lama metodu
    public void StartGame()
    {
        if (selectedCharacter != null)
        {
            // Sahneye ge�i� yapmadan �nce t�m oyuncular�n karakter verilerini senkronize edin
            PhotonNetwork.LoadLevel("GameScene"); // Oyuna ba�layaca��n�z sahne ad�n� buraya yaz�n
        }
        else
        {
            Debug.Log("Karakter se�ilmedi. Karakter se�meniz gerekiyor!");
        }
    }

    // Sahne y�klendi�inde oyuncunun karakterini instantiate et
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            // MasterClient olarak, t�m oyuncular�n karakterlerini instantiate edin
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
