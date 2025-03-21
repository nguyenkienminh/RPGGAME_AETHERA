using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    public bool dashUnclocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot dashUnclockedButton;

    [Header("Clone on dash")]
    public bool cloneOnDashUnclocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnclockedButton;


    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnclocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnclockedButton;

    protected override void Start()
    {
        base.Start();

        dashUnclockedButton.GetComponent<Button>().onClick.AddListener(UnclockDash);
        cloneOnDashUnclockedButton.GetComponent<Button>().onClick.AddListener(UnclockCloneOnDash);
        cloneOnArrivalUnclockedButton.GetComponent<Button>().onClick.AddListener(UnclockCloneOnArrival);
    }
    protected override void CheckUnlock()
    {
        UnclockDash();
        UnclockCloneOnArrival();
        UnclockCloneOnDash();   
    }
    public override void UseSkill()
    {
        base.UseSkill();
    }

    public void UnlockDashSkill()
    {
        UnclockDash();
    }
    public void UnlockDashCloneSkill()
    {
        UnclockCloneOnDash();
    }
    public void UnlockCloneOnArrivalCSkill()
    {
        UnclockCloneOnArrival();
    }
    private void UnclockDash()
    {
        if (dashUnclockedButton.unlocked)
        {
            dashUnclocked = true;
        }
    }

    private void UnclockCloneOnDash()
    {
        if (cloneOnDashUnclockedButton.unlocked)
        {
            cloneOnDashUnclocked = true;
        }
    }

    private void UnclockCloneOnArrival()
    {
        if (cloneOnArrivalUnclockedButton.unlocked)
        {
            cloneOnArrivalUnclocked = true;

        }
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnclocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
        }
    }
    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnclocked)
        {
            SkillManager.instance.cloneSkill.CreateClone(player.transform, Vector3.zero);
        }
    }
}
