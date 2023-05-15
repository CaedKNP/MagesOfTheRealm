using Assets._Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroUnitBase : UnitBase
{
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Stats statistics;
    bool _canMove;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();

    [SerializeField]
    Spell PrimarySpell;
    float primaryCooldownCounter = 0f;

    [SerializeField]
    Spell SecondarySpell;
    float secondaryCooldownCounter = 0f;

    [SerializeField]
    Spell QSpell;
    float QCooldownCounter = 0f;

    [SerializeField]
    Spell ESpell;
    float ECooldownCounter = 0f;

    [SerializeField]
    Spell DashSpell;
    float DashCooldownCounter = 0f;

    StaffRotation spellRotator;

    Component conditionsBar;

    ConditionUI _conditionUI;

    [SerializeField]
    public GameObject healthBarManagerObj;
    HealthBarManager healthBar;
    private Animator _anim;

    Coroutine burnRoutine, freezeRoutine, slowRoutine, speedUpRoutine, poisonRoutine, armorUpRoutine, armorDownRoutine, hasteRoutine, dmgUpRoutine;

    void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spellRotator = GetComponentInChildren<StaffRotation>();

        healthBar = FindObjectOfType<HealthBarManager>();
        healthBar.SetMaxHealth(statistics.MaxHp);

        _anim = GetComponent<Animator>();

        conditionsBar = gameObject.transform.GetChild(0);

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
    }

    void FixedUpdate()
    {
        TryMove();
    }

    #region Movement

    void TryMove()
    {
        if (_canMove)
        {
            Move();
        }
    }

    void Move()
    {
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {

            bool success = TryMove(movementInput);
            _anim.CrossFade("Walk", 0, 0);
            //Gliding around walls
            #region Gliding

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success)
            {
                _ = TryMove(new Vector2(0, movementInput.y));
            }

            #endregion

        }
        else
        {
            _anim.CrossFade("Idle", 0, 0);
        }

        // Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    public override bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                statistics.MovementSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + statistics.MovementSpeed * Time.fixedDeltaTime * direction);
                return true;
            }

            return false;
        }

        // Can't move if there's no direction to move in
        return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    #endregion

    void OnStateChanged(GameState newState)
    {
        if (newState == GameState.Playing) _canMove = true;
    }

    public override void SetStats(Stats stats)
    {
        statistics = stats;
    }

    public override void TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        statistics.CurrentHp -= Convert.ToInt32(dmgToTake * statistics.Armor);

        healthBar.SetHealth(statistics.CurrentHp);

        if (statistics.CurrentHp <= 0)
            Die();

        ConditionAffect(conditions);
    }

    #region Conditions

    private void ConditionAffect(List<ConditionBase> conditions)
    {
        foreach (ConditionBase condition in conditions)
        {
            Affect(condition);
        }
    }

    private void Affect(ConditionBase condition)
    {
        switch (condition.Conditions)
        {
            case global::Conditions.Burn:

                burnRoutine ??= StartCoroutine(BurnTask(condition));

                break;
            case global::Conditions.Slow:

                slowRoutine ??= StartCoroutine(SlowTask(condition));

                break;
            case global::Conditions.Freeze:

                freezeRoutine ??= StartCoroutine(FreezeTask(condition));

                break;
            case global::Conditions.Poison:

                poisonRoutine ??= StartCoroutine(PoisonTask(condition));

                break;
            case global::Conditions.SpeedUp:

                speedUpRoutine ??= StartCoroutine(SpeedUpTask(condition));

                break;
            case global::Conditions.ArmorUp:

                armorUpRoutine ??= StartCoroutine(ArmorUpTask(condition));

                break;
            case global::Conditions.ArmorDown:

                armorDownRoutine ??= StartCoroutine(ArmorDownTask(condition));

                break;
            case global::Conditions.Haste:

                hasteRoutine ??= StartCoroutine(HasteTask(condition));

                break;
            case global::Conditions.DmgUp:

                dmgUpRoutine ??= StartCoroutine(DmgUpTask(condition));

                break;
            default:
                break;
        }
    }

    private IEnumerator BurnTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(0);

        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(statistics.CurrentHp);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(0);
        burnRoutine = null;
    }

    private IEnumerator SlowTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(1);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed -= statistics.MovementSpeed * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(1);
        statistics.MovementSpeed = tempSpeed;
        slowRoutine = null;
    }

    private IEnumerator PoisonTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(2);
        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(condition.AffectOnTick);

            healthBar.SetHealth(statistics.CurrentHp);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        _conditionUI.RemoveConditionSprite(2);
        poisonRoutine = null;
    }

    private IEnumerator FreezeTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(3);

        _canMove = false;

        var end = DateTime.Now.Second + condition.AffectTime;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(3);
        _canMove = true;
        freezeRoutine = null;
    }

    private IEnumerator SpeedUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(4);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempMoveSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed += statistics.MovementSpeed * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _conditionUI.RemoveConditionSprite(4);
        statistics.MovementSpeed = tempMoveSpeed;
        speedUpRoutine = null;
    }

    private IEnumerator ArmorUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(5);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempArmor = statistics.Armor;

        statistics.Armor += statistics.Armor * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmor;

        _conditionUI.RemoveConditionSprite(5);
        armorUpRoutine = null;
    }

    private IEnumerator ArmorDownTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(6);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempArmorDown = statistics.Armor;

        statistics.Armor -= statistics.Armor * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmorDown;

        _conditionUI.RemoveConditionSprite(6);
        armorDownRoutine = null;
    }

    private IEnumerator HasteTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(7);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempCooldown = statistics.CooldownModifier;

        statistics.CooldownModifier += statistics.CooldownModifier * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.CooldownModifier = tempCooldown;

        _conditionUI.RemoveConditionSprite(7);
        hasteRoutine = null;
    }

    private IEnumerator DmgUpTask(ConditionBase condition)
    {
        _conditionUI.AddConditionSprite(8);

        var end = DateTime.Now.Second + condition.AffectTime;
        var tempDmg = statistics.DmgModifier;

        statistics.DmgModifier += statistics.DmgModifier * condition.AffectOnTick;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.DmgModifier = tempDmg;

        _conditionUI.RemoveConditionSprite(8);
        dmgUpRoutine = null;
    }

    #endregion

    #region Input

    void OnPrimaryAttack()
    {
        if (Time.time > primaryCooldownCounter)
        {
            CastSpell(PrimarySpell);
            primaryCooldownCounter = Time.time + PrimarySpell.cooldown * statistics.CooldownModifier;
            //SpellPanel.PrimarySpell.Slider.Value = primaryCooldownCounter - Time.time;
        }
    }

    void OnSecondaryAttack()
    {
        if (Time.time > secondaryCooldownCounter)
        {
            CastSpell(SecondarySpell);
            secondaryCooldownCounter = Time.time + SecondarySpell.cooldown * statistics.CooldownModifier;
        }
    }

    void OnQSpell()
    {
        if (Time.time > QCooldownCounter)
        {
            CastSpell(QSpell);
            QCooldownCounter = Time.time + QSpell.cooldown * statistics.CooldownModifier;
        }
    }

    void OnESpell()
    {
        if (Time.time > ECooldownCounter)
        {
            CastSpell(ESpell);
            ECooldownCounter = Time.time + ESpell.cooldown * statistics.CooldownModifier;
        }
    }

    void OnDodge()
    {
        if (Time.time > DashCooldownCounter)
        {
            CastSpell(DashSpell);
            DashCooldownCounter = Time.time + DashSpell.cooldown * statistics.CooldownModifier;
        }
    }

    void OnInteraction()
    {

    }

    #endregion

    void CastSpell(Spell spell)
    {
        Debug.Log("Casting spell " + spell);

        if (spellRotator != null)
        {
            if (spell.CastFromHeroeNoStaff)
            {
                Instantiate(spell.Prefab, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(spell.Prefab, spellRotator.WizandStaffFirePint.transform.position, spellRotator.WizandStaffFirePint.transform.rotation);
            }
        }
        else
        {
            Debug.LogWarning("SpellRotator is not assigned!");
        }
    }

    public override void Die()
    {
        Debug.Log($"{name} is dead");
    }
}