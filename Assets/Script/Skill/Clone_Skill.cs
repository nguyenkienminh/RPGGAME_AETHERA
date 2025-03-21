using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
    [Header("CLone info")]
    [SerializeField] private float attackMutiplier;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [Space]

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnclockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Header("Aggressive clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveUnclockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    [SerializeField] public bool canApplyOnHitEffect;

    [Header("Multiple clone")]
    [SerializeField] private UI_SkillTreeSlot multipleUnclockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private float changeToDuplicate;
    [SerializeField] private bool canDuplicateClone;

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadOfCloneUnclockButton;
    public bool crystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockCloneAttack);
        aggressiveUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockAggressiveClone);
        multipleUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockMultipleClone);
        crystalInsteadOfCloneUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockCrystalInsteadOfClone);

    }

    #region Unclock Skill

    protected override void CheckUnlock()
    {
        UnclockCloneAttack();
        UnclockAggressiveClone();
        UnclockCrystalInsteadOfClone();
        UnclockMultipleClone();
    }

    public void UnlockCloneSkill()
    {
        UnclockCloneAttack();
    }
    public void UnlockAggressiveCloneSkill()
    {
        UnclockAggressiveClone();
    }
    public void UnlockMultipleCloneSkill()
    {
        UnclockMultipleClone();
    }
    public void UnlockCrystalInsteadOfCloneSkill()
    {
        UnclockCrystalInsteadOfClone();
    }

    private void UnclockCloneAttack()
    {
        if (cloneAttackUnclockButton.unlocked)
        {
            canAttack = true;
            attackMutiplier = cloneAttackMultiplier;
        }
    }
    private void UnclockAggressiveClone()
    {
        if (aggressiveUnclockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMutiplier = aggressiveCloneAttackMultiplier;
        }
    }
    private void UnclockMultipleClone()
    {
        if (multipleUnclockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMutiplier = multipleCloneAttackMultiplier;
        }
    }
    private void UnclockCrystalInsteadOfClone()
    {
        if (crystalInsteadOfCloneUnclockButton.unlocked)
        {
            crystalInsteadOfClone = true;
        }
    }
    #endregion 


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if(crystalInsteadOfClone)
        {
            SkillManager.instance.crystalSkill.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset,canDuplicateClone,changeToDuplicate,player,attackMutiplier);
    }


    public void CreateCloneWithDelay(Transform _enemyTransform)
    {      
          StartCoroutine(CloneDelayCaroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));     
    }
    private IEnumerator CloneDelayCaroutine(Transform _transform, Vector3 offset)
    {
        yield return new WaitForSeconds(.2f);
        CreateClone(_transform, offset);
    }
}
