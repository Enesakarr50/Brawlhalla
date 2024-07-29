using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSellection : MonoBehaviour
{
    public CharacterData[] Characters;
    public CharacterData CurrentData;
    public GameObject PlayerPrefab;

    public Image CharImage;
    public Image WeaponImage;
    public Image PasifSkill;
    public Image ActiveSkill;

    public void choseChar(int intex)
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
        {
            
            CurrentData = Characters[intex];
            PlayerPrefab.GetComponent<Player>().Character = CurrentData;
            CharImage.sprite = CurrentData.CharacterImage;
            WeaponImage.sprite = CurrentData.WeaponImage;
            PasifSkill.sprite = CurrentData._passiveSkill;
            ActiveSkill.sprite = CurrentData._activeSkill;
        } 
    }

    public void SpawnPlayer()
    {

    }
}
