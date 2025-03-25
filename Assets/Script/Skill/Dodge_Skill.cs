using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unclockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnclocked;


    [Header("Mirage_dodge")]
    [SerializeField] private UI_SkillTreeSlot unclockMirageDodge;
    public bool dodgeMirageUnclocked;

    protected override void Start()
    {
        base.Start();

        unclockDodgeButton.GetComponent<Button>().onClick.AddListener(UnclockDodge);
        unclockMirageDodge.GetComponent<Button>().onClick.AddListener(UnclockMirageDodge);

    }
    protected override void CheckUnlock()
    {
        UnclockDodge();
        UnclockMirageDodge();
    }

    public void UnlockDogeSkill()
    {
        UnclockDodge();
    }
    
    public void UnlockMirageDogeSkill()
    {
        UnclockMirageDodge();
    }

    private void UnclockDodge()
    {
        if (unclockDodgeButton.unlocked && !dodgeUnclocked)
        {
            player.chacracterStats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnclocked = true;
        }
    }

    private void UnclockMirageDodge()
    {
        if (unclockMirageDodge.unlocked)
        {
            dodgeMirageUnclocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnclocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
        }
    }
}
