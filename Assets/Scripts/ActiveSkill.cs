using System.Collections;
using UnityEngine;

public class ActiveSkill : ScriptableObject
{
    public Sprite icon;
    public float cooldown;


    // Aktif yetenek için bir metot tanýmlayýn (örnek olarak)
    public virtual void Activate(GameObject character)
    {
       
    }

}
[CreateAssetMenu(fileName = "Bazuka", menuName = "ActiveSkillTypes/Bazuka")]
public class Bazuka : ActiveSkill
{
    public GameObject fireballPrefab; // Alev topu prefabý
    public int speed;
    public int knocBackPower;
    public int radius;
    public override void Activate(GameObject character)
    {
        GameObject proj = Instantiate(fireballPrefab);  
    }
}
[CreateAssetMenu(fileName = "Inviciblety", menuName = "ActiveSkillTypes/Inviciblety")]

public class Inviciblety : ActiveSkill
{
    public int duration;
    public override void Activate(GameObject character)
    {
       character.GetComponent<SpriteRenderer>().enabled = false;          
    }
}