using UnityEngine;

[CreateAssetMenu(menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string SkillName;
    public int CoolDown;
    public int Duration;
    public int ProjectileSpeed;
    public int Radius;
    public Sprite SkillIcon;
    public GameObject ProjectilePrefeab;

}
