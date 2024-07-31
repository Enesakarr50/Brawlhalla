using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName ="Class")]
public class CharacterData : ScriptableObject
{


    public int CharacterSpeed;
    public int CharacterJumpForce;
    public int characterJumpCount;
    public int Index;


    public float FireRate;
    public float FireSpeed;
    public float KnockBackRate;

    public Sprite InGamePlayer;
    public Sprite InGameWeapon;

    public Sprite CharacterImage;
    public Sprite WeaponImage;
    public Sprite _passiveSkill;
    public Sprite _activeSkill;


    public Skill Skill;


    public GameObject projectilePrefab;

   

}
