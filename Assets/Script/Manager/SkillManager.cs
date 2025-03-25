using UnityEngine;

public class SkillManager : MonoBehaviour
{
   public static SkillManager instance;

    public Dash_Skill dashSkill { get; private set; }
    public Clone_Skill cloneSkill { get; private set; }
    public Sword_Skill swordSkill { get; private set; }
    public Blackhole_Skill BlackholeSkill { get; private set; }
    public Crystal_Skill crystalSkill { get; private set; }
    public Parry_Skill parrySkill { get; private set; }

    public Dodge_Skill dodgeSkill { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dashSkill = GetComponent<Dash_Skill>();
        cloneSkill = GetComponent<Clone_Skill>();
        swordSkill = GetComponent<Sword_Skill>();
        BlackholeSkill = GetComponent<Blackhole_Skill>();
        crystalSkill = GetComponent<Crystal_Skill>();
        parrySkill = GetComponent<Parry_Skill>();
        dodgeSkill = GetComponent<Dodge_Skill>();
    }
}
