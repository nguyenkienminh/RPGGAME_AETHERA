using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}
[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]

public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public ItemEffect[] itemEffects;
    public float itemCooldown;


    [Header("Major stats")]
    public int strength; // 1 point increase damage and 1  crit power 1%
    public int agility; // 1 point increate  evasion and 1 crit change 1%
    public int intelligence; // 1 point increase magic dâmge by 1 and magic resistance by 3
    public int vitality; // 1 point increase health by 5 hp

    [Header("Offensive stats")]
    public int damage;
    public int critPower;// default 150%
    public int critChange;


    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion; 
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;


    private int descriptionLength;
    public void Effect(Transform _EnemyPosition)
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_EnemyPosition);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChange.AddModifier(critChange);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModififers()
    {

        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChange.RemoveModifier(critChange);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChange, "Crit.Chance");
        AddItemDescription(critPower, "Crit.Power");

        AddItemDescription(health, "Health");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist");

        AddItemDescription(fireDamage, "Fire damage");
        AddItemDescription(iceDamage, "Ice damage");
        AddItemDescription(lightingDamage, "Lighting dmg");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + $"<color=orange>+{itemEffects[i].effectDescription}</color>");
                descriptionLength++;
            }
        }


        if (descriptionLength < 5)
        {
            for(int i = 0; i < 5 -  descriptionLength; i++)
            {
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if(_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if(_value >  0)
            {
                sb.Append($"<color=red>+ {_value}</color> <color=green>{_name}</color>");
            }

            descriptionLength++;
        }
    }
}
