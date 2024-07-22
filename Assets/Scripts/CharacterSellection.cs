using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSellection : MonoBehaviour
{
    public CharacterData[] Characters;
    public CharacterData CurrentData;
    public GameObject PlayerPrefab;
    

    public TextMeshProUGUI CharNameTMP;
    public TextMeshProUGUI CharDescTMP;

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
            CharNameTMP.text = CurrentData.CharacterName;
            CharDescTMP.text = CurrentData.CharDesc;
            CharImage.sprite = CurrentData.CharacterImage;
            WeaponImage.sprite = CurrentData.WeaponImage;
            PasifSkill.sprite = CurrentData._passiveSkill;
            //ActiveSkill.sprite = CurrentData._activeSkill.icon;
        } 
    }

    public void SpawnPlayer()
    {
        Instantiate(PlayerPrefab);
        //SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName(m_Scene));
    }
}
