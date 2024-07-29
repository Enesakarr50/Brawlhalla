using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Class")]
public class CharacterData : ScriptableObject
{
    public string CharacterName;


    public int CharacterSpeed;
    public int CharacterJumpForce;
    public int characterJumpCount;


    public float FireRate;
    public float FireSpeed;
    public float KnockBackRate;


    public string weaponName;
    public string CharDesc;


    public Sprite CharacterImage;
    public Sprite WeaponImage;
    public Sprite _passiveSkill;
    public Sprite _activeSkill;


    public Skill Skill;


    public GameObject projectilePrefab;

   

}
