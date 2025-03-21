using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform rectTransform;
    private Slider slider;
    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        rectTransform = GetComponent<RectTransform>();


        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (myStats != null)
        {
            slider.maxValue = myStats.GetMaxValueHP();
            slider.value = myStats.currentHealth;
        }
    }

    private void OnEnable()
    {
        if (entity != null)
        {
            entity.onFlipped += FlipUI;
        }

        if (myStats != null)
        {
            myStats.onHealthChange += UpdateHealthUI;
        }
    }

    private void OnDisable()
    {
        if (entity != null)
        {
            entity.onFlipped -= FlipUI;
        }

        if(myStats != null)
        {
            myStats.onHealthChange -= UpdateHealthUI;
        }
    }
    private void FlipUI()
    {
        if (rectTransform != null)
        {
            rectTransform.Rotate(0, 180, 0);
        }
    }
}
