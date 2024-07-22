using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName ="Class")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;
    public int CharacterSpeed;
    public int CharacterJumpForce;
    public int characterJumpCount;
    public string weaponName;
    public string CharDesc;
    public Sprite CharacterImage;
    public Sprite WeaponImage;
    public Sprite _passiveSkill;
    public Skill Skill;

}
