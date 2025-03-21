using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{
    [SerializeField] private UI_SkillTreeSlot blackHoleUnclockButton;
    public bool blackHoleUnclocked {  get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    BlackHole_Skill_Controller currentBlackhole;

    public void UnlockDarkHoleSkill()
    {
        UnclockBlackHole();
    }

    private void UnclockBlackHole()
    {
        if (blackHoleUnclockButton.unlocked)
        {
            blackHoleUnclocked = true;
        }
    }
    protected override void CheckUnlock()
    {

        UnclockBlackHole();
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<BlackHole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown);

        AudioManager.instance.PlaySFX(52, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);

    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockBlackHole);
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool BlackholeFinished()
    {
        if (!currentBlackhole)
        {
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }
        return false;
    }
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
