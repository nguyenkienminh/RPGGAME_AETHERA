using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private UI_SkillTreeSlot unclockCloneInsteadButton;
    [SerializeField] private bool CloneInsteadOfCrystal;

    [Header("Crystal simple")]
    [SerializeField] private UI_SkillTreeSlot unclockCrystalButton;
    public bool crystalUnclocked;

    [Header("Explosive crystal")]
    [SerializeField] private UI_SkillTreeSlot unclockExplosiveButton;
    [SerializeField] private float exploviveCooldown;
    [SerializeField] private bool canExplode;


    [Header("Moving crystal")]
    [SerializeField] private UI_SkillTreeSlot unclockMovingCrystalButton;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("multi stacking crystal")]
    [SerializeField] private UI_SkillTreeSlot unclockMultiStackButton;
    [SerializeField] private bool canUseMultiStack;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unclockCrystalButton.GetComponent<Button>().onClick.AddListener(UnclockCrystal);
        unclockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnclockCrystalMirage);
        unclockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unclockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnclockMovingCrystal);
        unclockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnclockMultiStack);

    }

    #region Unclock Skill
    protected override void CheckUnlock()
    {
        UnclockCrystal();
        UnclockCrystalMirage();
        UnlockExplosiveCrystal(); 
        UnclockMovingCrystal();
        UnclockMultiStack();
    }

    public void UnlockCrystalSkill()
    {
        UnclockCrystal();
    }
    public void UnlockCrystalMirageSkill()
    {
        UnclockCrystalMirage();
    }
    public void UnlockExplosiveCrystalSkill()
    {
        UnlockExplosiveCrystal();
    }
    public void UnlockMovingCrystalSkill()
    {
        UnclockMovingCrystal();
    }
    public void UnlockMultiStackSkill()
    {
        UnclockMultiStack();
    }

    private void UnclockCrystal()
    {
        if (unclockCrystalButton.unlocked)
        {
            crystalUnclocked = true;
        }
    }

    private void UnclockCrystalMirage()
    {
        if (unclockCloneInsteadButton.unlocked)
        {
            CloneInsteadOfCrystal = true;
        }
    }

    private void UnlockExplosiveCrystal()
    {
        if (unclockExplosiveButton.unlocked)
        {
            canExplode = true;
            cooldown = exploviveCooldown;
        }
    }
    private void UnclockMovingCrystal()
    {
        if (unclockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    private void UnclockMultiStack()
    {
        if (unclockMultiStackButton.unlocked)
        {
            canUseMultiStack = true;
        }
    }
    #endregion


    public override void UseSkill()
    {
        base.UseSkill();


        if (CanUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }

            Vector2 playerPos = player.transform.position;

            player.transform.position = currentCrystal.transform.position;

            currentCrystal.transform.position = playerPos;

            if (CloneInsteadOfCrystal)
            {
                SkillManager.instance.cloneSkill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();

            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

        Crystal_Skill_Controller currentCrystalScript = currentCrystal.GetComponent<Crystal_Skill_Controller>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosesEnemy(currentCrystal.transform), player);
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStack)
        {
            if (crystalLeft.Count > 0)
            {
                cooldown = 0;
                float offset = .5f;

                for (int i = crystalLeft.Count - 1; i >= 0; i--)
                {
                    GameObject crystalToSpawn = crystalLeft[i];

                    Vector2 spawnPosition = player.transform.position;

                    spawnPosition += new Vector2(i * offset, 0);

                    GameObject newCrystal = Instantiate(crystalToSpawn, spawnPosition, Quaternion.identity);

                    newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosesEnemy(newCrystal.transform), player);

                    crystalLeft.RemoveAt(i);
                }

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }

                return true;
            }
        }

        return false;
    }


    private void RefillCrystal()
    {
        for (int i = 0; i < amountOfStacks; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

}