using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage;
    private Color lockedSkillColor = new Color(109f / 255f, 86f / 255f, 86f / 255f);
    public bool unlocked;

    [SerializeField] private int skillCost;

    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private UI_SkillTreeSlot[] showBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] showBeLocked;

    private Dictionary<string, Action> skillActions = new Dictionary<string, Action>();
    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillShot());
        //DontDestroyOnLoad(gameObject); // Giữ SaveManager khi chuyển scene
        InitSkillActions();
    }

    private void Start()
    {

        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
    }

    private void InitSkillActions()
    {
        skillActions = new Dictionary<string, Action>
        {
            { "Sword Throw", () => FindObjectOfType<Sword_Skill>()?.UnlockSwordSkill() },
            { "Chain saw sword", () => FindObjectOfType<Sword_Skill>()?.UnlockSpinSkill() },
            { "Time stop", () => FindObjectOfType<Sword_Skill>()?.UnlockTimeSkill() },
            { "Vulnerability", () => FindObjectOfType<Sword_Skill>()?.UnlockVulnerableSkill() },
            { "Bouncy sword", () => FindObjectOfType<Sword_Skill>()?.UnlockBoundSkill() },
            { "Bullet sword", () => FindObjectOfType<Sword_Skill>()?.UnlockPierceSkill() },
            { "Dash", () => FindObjectOfType<Dash_Skill>()?.UnlockDashSkill() },
            { "Dash - Phantom dash", () => FindObjectOfType<Dash_Skill>()?.UnlockDashCloneSkill() },
            { "Dash - Echo phantom", () => FindObjectOfType<Dash_Skill>()?.UnlockCloneOnArrivalCSkill() },
            { "Time mirage", () => FindObjectOfType<Clone_Skill>()?.UnlockCloneSkill() },
            { "Aggresive mirage", () => FindObjectOfType<Clone_Skill>()?.UnlockAggressiveCloneSkill() },
            { "Multiple mirage", () => FindObjectOfType<Clone_Skill>()?.UnlockMultipleCloneSkill() },
            { "Crystal mirage", () => FindObjectOfType<Clone_Skill>()?.UnlockCrystalInsteadOfCloneSkill() },
            { "Blackhole", () => FindObjectOfType<Blackhole_Skill>()?.UnlockDarkHoleSkill() },
            { "Dodge", () => FindObjectOfType<Dodge_Skill>()?.UnlockDogeSkill() },
            { "Dodge mirage", () => FindObjectOfType<Dodge_Skill>()?.UnlockMirageDogeSkill() },
            { "Parry", () => FindObjectOfType<Parry_Skill>()?.UnlockParrySkill() },
            { "Restore with parry", () => FindObjectOfType<Parry_Skill>()?.UnlockParryRestoreSkill() },
            { "Parry with a mirage", () => FindObjectOfType<Parry_Skill>()?.UnlockParryWithMirageSkill() },
            { "Crystal", () => FindObjectOfType<Crystal_Skill>()?.UnlockCrystalSkill() },
            { "Explosion", () => FindObjectOfType<Crystal_Skill>()?.UnlockExplosiveCrystalSkill() },
            { "Multiple destruction", () => FindObjectOfType<Crystal_Skill>()?.UnlockMultiStackSkill() },
            { "Mirage blink", () => FindObjectOfType<Crystal_Skill>()?.UnlockMovingCrystalSkill() },
            { "Controlled destruction", () => FindObjectOfType<Crystal_Skill>()?.UnlockCrystalMirageSkill() },
            
            // Thêm kỹ năng khác nếu cần
        };
    }

    public void UnlockSkillShot()
    {
        if (!PlayerManager.instance.HaveEnoughMoney(skillCost))
        {
            return;
        }


        for (int i = 0; i < showBeUnlocked.Length; i++)
        {
            if (showBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Can not unlock skill");
                return;
            }
        }

        for (int i = 0; i < showBeLocked.Length; i++)
        {
            if (showBeLocked[i].unlocked == true)
            {

                Debug.Log("Can not unlock skill");
                return;
            }
        }

        unlocked = true;
        AudioManager.instance.PlaySFX(36, null);
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillCost);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {

        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
        ActivateSkillLoad();
    }

    public void ActivateSkillLoad()
    {
        if (unlocked && skillActions.ContainsKey(skillName))
        {
            skillActions[skillName]?.Invoke();
        }
    }

    public void SaveData(ref GameData _data)
    {

        if (unlocked) // Chỉ lưu kỹ năng đã mở khóa
        {
            if (!_data.skillTree.ContainsKey(skillName))
            {
                _data.skillTree.Add(skillName, true);
            }
            else
            {
                _data.skillTree[skillName] = true;
            }
        }
        else
        {
            _data.skillTree.Remove(skillName);
        }

    }
}
