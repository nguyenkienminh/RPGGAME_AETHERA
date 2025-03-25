using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnclockButton;
    public bool parryUnclocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnclockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnclocked { get; private set; }

    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnclockButton;
    public bool parryWithMirageUnclocked { get; private set; }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {

        base.UseSkill();
        if (restoreUnclocked)
        {
            int restoreAmount = (int)(player.chacracterStats.GetMaxValueHP() * restoreHealthPercentage);
            player.chacracterStats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {

        base.Start();

        parryUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockParry);
        restoreUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockParryRestore);
        parryWithMirageUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockParryWithMirage);

    }
    protected override void CheckUnlock()
    {
        UnclockParry();
        UnclockParryRestore();
        UnclockParryWithMirage();
    }

    public void UnlockParrySkill()
    {
        UnclockParry();
    }
    public void UnlockParryRestoreSkill()
    {
        UnclockParryRestore();
    }
    public void UnlockParryWithMirageSkill()
    {
        UnclockParryWithMirage();
    }

    private void UnclockParry()
    {
        if (parryUnclockButton.unlocked)
        {
            parryUnclocked = true;
        }
    }

    private void UnclockParryRestore()
    {
        if(restoreUnclockButton.unlocked)
        {
            restoreUnclocked = true;
        }
    }

    private void UnclockParryWithMirage()
    {
        if ((parryWithMirageUnclockButton.unlocked))
        {
            parryWithMirageUnclocked = true;
        }
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnclocked)
        {
            SkillManager.instance.cloneSkill.CreateCloneWithDelay(_respawnTransform);
        }
    }
}
