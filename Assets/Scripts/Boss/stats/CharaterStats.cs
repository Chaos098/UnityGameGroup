using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    evasion,
    vitality,
    strength,
    agility,
    intelegence,
    damage,
    critChance,
    critPower,
    health,
    armor,
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;
    private Rigidbody rb;

    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility;  // 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat evasion;
    public Stat vitality; // 1 point incredase health by 5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;              // default value 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;

    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {

    }


    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));

    private IEnumerator VulnerableCorutine(float _duartion)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duartion);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        // start corototuine for stat increase
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;


        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform, criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    #region Magical damage and ailemnts

   
    #endregion

    public virtual void TakeDamage(int _damage)
    {

        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);



        GetComponent<Entity>().DamageImpact();
        _ = fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }


    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }


    protected virtual void DecreaseHealthBy(int _damage)
    {

        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (onHealthChanged != null)
            onHealthChanged();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;


    #region Stat calculations
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();


        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }



    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();


        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }


        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion

    public Stat GetStat(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelegence) return intelligence;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.health) return maxHealth;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.vitality) return vitality;
        return null;
    }
}