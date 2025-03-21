using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;


    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;

    private SkillManager skills;


    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;

    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChange += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }

    private void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dashSkill.dashUnclocked)
        {
            SetCoolDown(dashImage);
        }

        if (Input.GetKeyDown(KeyCode.Q) && skills.parrySkill.parryUnclocked)
        {
            SetCoolDown(parryImage);
        }

        if (Input.GetKeyDown(KeyCode.F) && skills.crystalSkill.crystalUnclocked)
        {
            SetCoolDown(crystalImage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.swordSkill.swordUnclocked)
        {
            Debug.Log("Sword");
            SetCoolDown(swordImage);
        }

        if (Input.GetKeyDown(KeyCode.R) && skills.BlackholeSkill.blackHoleUnclocked)
        {
            SetCoolDown(blackHoleImage);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCoolDown(flaskImage);
        }

        CheckCooldown(dashImage, skills.dashSkill.cooldown);
        CheckCooldown(parryImage, skills.parrySkill.cooldown);
        CheckCooldown(crystalImage, skills.crystalSkill.cooldown);
        CheckCooldown(swordImage, skills.swordSkill.cooldown);
        CheckCooldown(blackHoleImage, skills.BlackholeSkill.cooldown);
        CheckCooldown(flaskImage, Inventory.instance.flaskCooldown);


    }

    private void UpdateSoulsUI()
    {
 
        currentSouls.text = PlayerManager.instance.GetCurrency().ToString("#,#");

    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxValueHP();
        slider.value = playerStats.currentHealth;
    }
    private void SetCoolDown(Image _image)
    {

        if (_image.fillAmount <= 0)
        {
            Debug.Log("SetCoolDown");
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldown(Image _image, float _cooldown)
    {

        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
