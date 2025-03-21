using System.Collections;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChange,
    critPower,
    health,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage,
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;


    [Header("Major stats")]
    public Stats strength; // 1 point increase damage and 1  crit power 1%
    public Stats agility; // 1 point increate  evasion and 1 crit change 1%
    public Stats intelligence; // 1 point increase magic dâmge by 1 and magic resistance by 3
    public Stats vitality; // 1 point increase health by 5 hp

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critPower;// default 150%
    public Stats critChange;


    [Header("Defensive stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    public bool isIgnited; // does damages over time
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20$

    [SerializeField] private float ailmentDuration = 4;
    public float ignitedTimer;
    public float chilledTimer;
    public float shockedTimer;

    public float igniteDamageCoolDown = .3f;
    public float igniteDamageTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    private int igniteDamage;

    public int currentHealth;

    public System.Action onHealthChange;

    public bool isDead { get; private set; }
    public bool isInvincible {  get; private set; }
    public bool isVulnerable;
    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxValueHP();
        fx = GetComponent<EntityFX>();
    }
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }

        if (isIgnited && !isDead)
        {
            ApplyIgniteDamage();
        }
    }


    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCoroutine(_duration));
    private IEnumerator VulnerableForCoroutine(float duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _statsToModify)
    {
        StartCoroutine(StatsModCoroutine(_modifier, _duration, _statsToModify));
    }

    public IEnumerator StatsModCoroutine(int _modifier, float _duration, Stats _statToModify)
    {
        if (_statToModify == maxHealth)
        {
            int healthBeforeBuff = GetMaxValueHP();
            _statToModify.AddModifier(_modifier);
            int healthAfterBuff = GetMaxValueHP();

            int healthDifference = healthAfterBuff - healthBeforeBuff;
            currentHealth += healthDifference;

            currentHealth = Mathf.Clamp(currentHealth, 0, healthAfterBuff);

            if (onHealthChange != null)
            {
                onHealthChange();
            }
        }
        else
        {
            _statToModify.AddModifier(_modifier);
        }

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);

        if (_statToModify == maxHealth)
        {
            int healthAfterRemoveBuff = GetMaxValueHP();
            currentHealth = Mathf.Clamp(currentHealth, 0, healthAfterRemoveBuff);

            if (onHealthChange != null)
            {
                onHealthChange();
            }
        }
    }



    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DescreaseHealthBy(igniteDamage);

            if (currentHealth < 0)
            {
                Die();

            }

            igniteDamageTimer = igniteDamageCoolDown;
        }
    }
    public virtual void DoDamage(CharacterStats _targetStats, bool? _specialSkill)
    {
        if (_targetStats.isInvincible)
        {
            return;
        }

        bool CanCriticalDamageFX = false;

        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (canCrit())
        {
            totalDamage = CalculateCriticalDame(totalDamage);
            CanCriticalDamageFX = true;
        }

        fx.CreateHitFx(_targetStats.transform, CanCriticalDamageFX);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if ((bool)_specialSkill == true)
        {
            totalDamage = (int)(totalDamage * .5f);
        }

        _targetStats.TakeDamage(totalDamage);
        //DoMagicalDamage(_targetStats);  //remove if you dont't want to apply magic hit on primary attack
    }
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        if (_targetStats.isInvincible)
        {
            return;
        }

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDame = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicalDame = CheckTargetResistance(_targetStats, totalMagicalDame);

        _targetStats.TakeDamage(totalMagicalDame);




        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        AttemptyToApllyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }
    public virtual void DealTotalDamage(CharacterStats _targetStats, bool? _specialSkill)
    {
        if (_targetStats.isInvincible)
        {
            return;
        }

        bool CanCriticalDamageFX = false;

        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (canCrit())
        {
            totalDamage = CalculateCriticalDame(totalDamage);
            CanCriticalDamageFX = true;
        }

        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        totalDamage += totalMagicalDamage;
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if ((bool)_specialSkill == true)
        {
            totalDamage = (int)(totalDamage * .5f);
        }

        fx.CreateHitFx(_targetStats.transform, CanCriticalDamageFX);
        //fx.CreatePopupText(totalDamage.ToString());

        _targetStats.TakeDamage(totalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) > 0)
        {
            AttemptyToApllyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
        }
    }

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible) return;

        fx.StartCoroutine("FlashFX");

        DescreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();


        if (currentHealth < 0)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxValueHP())
        {
            currentHealth = GetMaxValueHP();
        }

        if (onHealthChange != null)
        {
            onHealthChange();
        }
    }

    public virtual void DescreaseHealthBy(int _damage)
    {
        if (isVulnerable)
        {
            _damage = (int)(_damage * 1.2f);
        }

        if (_damage > 0)
        {
            fx.CreatePopupText(_damage.ToString());
        }

        currentHealth -= _damage;

        if (onHealthChange != null)
        {
            onHealthChange();
        }
    }
  
    private void AttemptyToApllyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _fireDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

                return;
            }

            if (Random.value < .5f && _lightingDamage > 0)
            {
                canApplyShock = true;

                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }


        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage((int)(_fireDamage * .2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage((int)(_lightingDamage * .3f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDame)
    {
        totalMagicalDame -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);

        totalMagicalDame = Mathf.Clamp(totalMagicalDame, 0, int.MaxValue);
        return totalMagicalDame;
    }
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration;

            fx.IgniteFxFor(ailmentDuration);
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentDuration;

            float slowPercentage = .4f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentDuration);
            fx.ChillFxFor(ailmentDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitTargetWithShockStrike();
            }
        }
    }
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedTimer = ailmentDuration;

        fx.ShockFxFor(ailmentDuration);
    }
    private void HitTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closesEnemy = null;
        foreach (var hit in colliders)
        {
            //&& Vector2.Distance(transform.position, hit.transform.position) > 1
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closesEnemy = hit.transform;
                }
            }

            //if (closesEnemy == null)
            //{
            //    closesEnemy = transform;
            //}
        }

        if (closesEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ThunderStrike_Controller>().SetupThunder(closesEnemy.GetComponent<CharacterStats>(), shockDamage);
        }

        // find cloest target, only among the enemies
        // instantiate thunder strike
        // setup thunder strike
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    public int GetMaxValueHP()
    {
        return maxHealth.GetValue() + (vitality.GetValue() * 5);
    }
   

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }



        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= (int)(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    protected bool canCrit()
    {
        int totalCriticalChance = critChange.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    protected int CalculateCriticalDame(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;


        float critDamage = _damage * totalCritPower;

        return (int)critDamage;
    }
    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
        {
            Die();
        }
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    public Stats GetStat(StatType buffType)
    {
        if (buffType == StatType.strength) return strength;
        else if (buffType == StatType.agility) return agility;
        else if (buffType == StatType.intelligence) return intelligence;
        else if (buffType == StatType.vitality) return vitality;
        else if (buffType == StatType.damage) return damage;
        else if (buffType == StatType.critChange) return critChange;
        else if (buffType == StatType.critPower) return critPower;
        else if (buffType == StatType.health) return maxHealth;
        else if (buffType == StatType.armor) return armor;
        else if (buffType == StatType.evasion) return evasion;
        else if (buffType == StatType.magicRes) return magicResistance;
        else if (buffType == StatType.fireDamage) return fireDamage;
        else if (buffType == StatType.iceDamage) return iceDamage;
        else if (buffType == StatType.lightingDamage) return lightingDamage;

        return null;
    }
}
