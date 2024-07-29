using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviourPun
{
    public CharacterData[] Characters;
    public CharacterData CurrentData;
    public GameObject PlayerPrefab;

    public Image CharImage;
    public Image WeaponImage;
    public Image PasifSkill;
    public Image ActiveSkill;

    public void ChooseChar(int index)
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            CurrentData = Characters[index];
            PlayerPrefab.GetComponent<Player>().Character = CurrentData;
            CharImage.sprite = CurrentData.CharacterImage;
            WeaponImage.sprite = CurrentData.WeaponImage;
            PasifSkill.sprite = CurrentData._passiveSkill;
            ActiveSkill.sprite = CurrentData._activeSkill;

            // Karakter se�imini di�er oyunculara bildir
            photonView.RPC("OnCharacterSelected", RpcTarget.OthersBuffered, index);
        }
    }

    [PunRPC]
    public void OnCharacterSelected(int index)
    {
        // Di�er oyuncunun se�imini g�ncelle
        Debug.Log($"Other player selected: {Characters[index].CharacterName}");
        // Burada UI g�ncellemesi yapabilirsiniz.
    }
}