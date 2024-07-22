using System.Collections;
using UnityEngine;


[System.Serializable]
public class ActiveSkill : ScriptableObject
{
    public Sprite icon;
    public float cooldown;

    // Aktif yetenek i�in bir metot tan�mlay�n (�rnek olarak)
    public virtual void Activate(GameObject character)
    {
        // Yetenek aktif edildi�inde yap�lacak i�lemler
    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "Bazuka", menuName = "ActiveSkillTypes/Bazuka")]
public class Bazuka : ActiveSkill
{
    public GameObject fireballPrefab; 
    public int speed;
    public int knockBackPower;
    public int radius;

    public override void Activate(GameObject character)
    {
        
      
    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "Inviciblety", menuName = "ActiveSkillTypes/Inviciblety")]
public class Inviciblety : ActiveSkill
{
    public float duration; 

    public override void Activate(GameObject character)
    {
        
    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "MagicWall", menuName = "ActiveSkillTypes/MagicWall")]
public class MagicWall : ActiveSkill
{
    public float duration;

    public override void Activate(GameObject character)
    {

    }
}

[System.Serializable]
[CreateAssetMenu(fileName = "Catapult", menuName = "ActiveSkillTypes/Catapult")]
public class Catapult : ActiveSkill
{
    public float duration;

    public override void Activate(GameObject character)
    {

    }
}
