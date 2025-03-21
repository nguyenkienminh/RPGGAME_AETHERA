using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;


[System.Serializable]
public class EnemyData
{
    public List<string> defeatedEnemies;

    public EnemyData()
    {
        defeatedEnemies = new List<string>();
    }
}

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSysItem;
    //public Stats soulsDropAmount; 

    [Header("Level details")]
    [SerializeField] private int level = 1;
    //[SerializeField] private TextMeshPro LevelDisplayPrefabs;
    [SerializeField] private TextMeshPro LevelDisplay;
    [SerializeField] private bool boss = false;
    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .3f;

    private static string savePath;
    private static EnemyData enemyData;
    protected override void Start()
    {
        //soulsDropAmount.SetDefaultValue(100);

        ApplyModifier();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSysItem = GetComponent<ItemDrop>();

        if (enemyData == null)
        {
            LoadEnemyData();
        }
        if (enemyData.defeatedEnemies.Contains(enemy.name))
        {
            Destroy(gameObject);
        }

        if (LevelDisplay != null)
        {
            LevelDisplay.text = "Level " + level.ToString();
            if (level == 2)
            {
                transform.localScale *= 1.2f;
            }
            else if (level == 3)
            {
                transform.localScale *= 1.3f;
            }
            else if (level == 4)
            {
                transform.localScale *= 1.4f;
            }
            else if (level == 5)
            {
                transform.localScale *= 1.5f;
            }

        }
        if (boss == true)
        {
            LevelDisplay.text = "BOSS";
        }




    }

    private void ApplyModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChange);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        //Modify(soulsDropAmount);
    }


    private void Modify(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stats.GetValue() * percentageModifier;

            _stats.AddModifier((int)modifier);
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }
    protected  override void Die()
    {
        base.Die();
        enemy.Die();

        AudioManager.instance.PlaySFX(18, null);
        //PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSysItem.GenerateDrop();

        // Save enemy death
        RegisterEnemyDeath(enemy.name);

        Destroy(gameObject, 3f);
    }
    public void RegisterEnemyDeath(string enemyID)
    {
        if (!enemyData.defeatedEnemies.Contains(enemyID))
        {
            enemyData.defeatedEnemies.Add(enemyID);
            SaveEnemyData();
        }
    }

    private static void LoadEnemyData()
    {
        savePath = Application.persistentDataPath + "/EnemyData.json";

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            enemyData = JsonUtility.FromJson<EnemyData>(json);
        }
        else
        {
            enemyData = new EnemyData();
        }
    }

    private static void SaveEnemyData()
    {
        string json = JsonUtility.ToJson(enemyData, true);
        File.WriteAllText(savePath, json);
    }
}
