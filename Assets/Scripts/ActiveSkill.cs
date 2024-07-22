using System.Collections;
using UnityEngine;


[System.Serializable]
public class ActiveSkill : ScriptableObject
{
    public Sprite icon;
    public float cooldown;

    // Aktif yetenek için bir metot tanýmlayýn (örnek olarak)
    public virtual void Activate(GameObject character)
    {
        // Yetenek aktif edildiðinde yapýlacak iþlemler
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
