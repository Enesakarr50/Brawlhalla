using UnityEngine;

public class ActiveSkill : ScriptableObject
{
    public Sprite icon;
    public float cooldown;

    // Aktif yetenek için bir metot tanýmlayýn (örnek olarak)
    public virtual void Activate(GameObject character)
    {
        // Yetenek aktivasyon kodu burada olacak
    }
}
[CreateAssetMenu(fileName = "New", menuName = "ActiveSkillTypes/ThrowableSkills")]
public class ThrowableSkill : ActiveSkill
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
[CreateAssetMenu(fileName = "New", menuName = "ActiveSkillTypes/Usable")]

public class UsableSkill : ActiveSkill
{
    public int duration;
    public override void Activate(GameObject character)
    {
        character.transform.position = new Vector3(0, 0, 0);
    }
}