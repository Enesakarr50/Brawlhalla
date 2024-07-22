using System.Collections;
using UnityEngine;

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

[CreateAssetMenu(fileName = "Inviciblety", menuName = "ActiveSkillTypes/Inviciblety")]
public class Inviciblety : ActiveSkill
{
    public float duration; 

    public override void Activate(GameObject character)
    {
        
    }
}

[CreateAssetMenu(fileName = "MagicWall", menuName = "ActiveSkillTypes/MagicWall")]
public class MagicWall : ActiveSkill
{
    public float duration;

    public override void Activate(GameObject character)
    {

    }
}

[CreateAssetMenu(fileName = "Catapult", menuName = "ActiveSkillTypes/Catapult")]
public class Catapult : ActiveSkill
{
    public float duration;

    public override void Activate(GameObject character)
    {

    }
}
