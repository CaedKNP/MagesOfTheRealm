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
    
    int temp = 0;

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

    [SerializeField]
    public GameObject healthBarManagerObj;
    HealthBarManager healthBar;
    private Animator _anim;

    [SerializeField]
    public GameObject conditionsBar;

    ConditionUI _conditionUI;

    bool IsBurning, IsFreezed, IsSlowed, IsSpeededUp, IsPoisoned, HasArmorUp, HasArmorDown, HasHaste, HasDmgUp;
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

        _conditionUI = conditionsBar.GetComponent<ConditionUI>();
    }

    void FixedUpdate()
    {
        TryMove();
        
        if (IsBurning && temp == 0)
        {
            temp++;
            _conditionUI.AddConditionSprite(0);
        }
        else
        {
            temp = 0;
        }
      //  _conditionUI.RemoveConditionSprite(0);

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

    public override void LockMovement()
    {
        _canMove = false;
    }

    public override void UnlockMovement()
    {
        _canMove = true;
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

        return;
    }

    #region Conditions

    private void ConditionAffect(List<ConditionBase> conditions)
    {
        foreach (ConditionBase condition in conditions)
        {
            Affect(condition);
        }

        return;
    }

    private void Affect(ConditionBase condition)
    {
        switch (condition.Conditions)
        {
            case global::Conditions.Burn:


                if (!IsBurning)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(BurnTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(BurnTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
               
                burnRoutine ??= StartCoroutine(BurnTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.Slow:

                slowRoutine ??= StartCoroutine(SlowTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.Freeze:

                freezeRoutine ??= StartCoroutine(FreezeTask(condition.AffectTime));

                break;
            case global::Conditions.Poison:

                poisonRoutine ??= StartCoroutine(PoisonTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.SpeedUp:

                speedUpRoutine ??= StartCoroutine(SpeedUpTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.ArmorUp:

                armorUpRoutine ??= StartCoroutine(ArmorUpTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.ArmorDown:

                armorDownRoutine ??= StartCoroutine(ArmorDownTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.Haste:

                hasteRoutine ??= StartCoroutine(HasteTask(condition.AffectTime, condition.AffectOnTick));

                break;
            case global::Conditions.DmgUp:

                dmgUpRoutine ??= StartCoroutine(DmgUpTask(condition.AffectTime, condition.AffectOnTick));

                break;
            default:
                break;
        }
    }

    //private IEnumerator BurnTask(float burnTime, float burnDmgPerTick)
    //{
    //    var end = DateTime.Now.Second + burnTime;

    //    while (DateTime.Now.Second < end)
    //    {

    //        statistics.CurrentHp -= Convert.ToInt32(burnDmgPerTick);

    //        healthBar.SetHealth(statistics.CurrentHp);

    //        if (statistics.CurrentHp <= 0)
    //            Die();

    //        yield return new WaitForSeconds(1);
    //    }

    //    burnRoutine = null;
    //}

    private IEnumerator BurnTask(float burnTime, float burnDmgPerTick)
    {
        var end = DateTime.Now.Second + burnTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(burnDmgPerTick);

            healthBar.SetHealth(statistics.CurrentHp);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        burnRoutine = null;
    }

    private IEnumerator SlowTask(float slowTime, float slowPercent)
    {
        var end = DateTime.Now.Second + slowTime;
        var tempSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed -= statistics.MovementSpeed * slowPercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.MovementSpeed = tempSpeed;

        slowRoutine = null;
    }

    private IEnumerator FreezeTask(float freezeTime)
    {
        _canMove = false;

        var end = DateTime.Now.Second + freezeTime;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        _canMove = true;

        freezeRoutine = null;
    }

    private IEnumerator PoisonTask(float poisonTime, float poisonDmgPerTick)
    {
        var end = DateTime.Now.Second + poisonTime;

        while (DateTime.Now.Second < end)
        {
            statistics.CurrentHp -= Convert.ToInt32(poisonDmgPerTick);

            healthBar.SetHealth(statistics.CurrentHp);

            if (statistics.CurrentHp <= 0)
                Die();

            yield return new WaitForSeconds(1);
        }

        poisonRoutine = null;
    }

    private IEnumerator SpeedUpTask(float speedUpTime, float speedUpPercent)
    {
        var end = DateTime.Now.Second + speedUpTime;
        var tempMoveSpeed = statistics.MovementSpeed;

        statistics.MovementSpeed += statistics.MovementSpeed * speedUpPercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.MovementSpeed = tempMoveSpeed;

        speedUpRoutine = null;
    }

    private IEnumerator ArmorUpTask(float armorUpTime, float armorUpPercent)
    {
        var end = DateTime.Now.Second + armorUpTime;
        var tempArmor = statistics.Armor;

        statistics.Armor += statistics.Armor * armorUpPercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmor;

        armorUpRoutine = null;
    }

    private IEnumerator ArmorDownTask(float armorDownTime, float armorDownPercent)
    {
        var end = DateTime.Now.Second + armorDownTime;
        var tempArmorDown = statistics.Armor;

        statistics.Armor -= statistics.Armor * armorDownPercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.Armor = tempArmorDown;

        armorDownRoutine = null;
    }

    private IEnumerator HasteTask(float hasteTime, float hastePercent)
    {
        var end = DateTime.Now.Second + hasteTime;
        var tempCooldown = statistics.CooldownModifier;

        statistics.CooldownModifier += statistics.CooldownModifier * hastePercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.CooldownModifier = tempCooldown;

        hasteRoutine = null;
    }

    private IEnumerator DmgUpTask(float dmgUpTime, float dmgUpPercent)
    {
        var end = DateTime.Now.Second + dmgUpTime;
        var tempDmg = statistics.DmgModifier;

        statistics.DmgModifier += statistics.DmgModifier * dmgUpPercent;

        while (DateTime.Now.Second < end)
        {
            yield return null;
        }

        statistics.DmgModifier = tempDmg;

        dmgUpRoutine = null;
    }

    #endregion

    public override void Die()
    {
        Debug.Log($"{name} is dead");
    }

    #region Attack

    async void OnPrimaryAttack()
    {
        //if (Time.time > primaryCooldownCounter)
        //{
        //    CastSpell(PrimarySpell);
        //    primaryCooldownCounter = Time.time + PrimarySpell.cooldown * statistics.CooldownModifier;
        //}
        await TakeDamage(0, new List<ConditionBase>() { new ConditionBase() { Conditions = Conditions.Burn, AffectOnTick = 1, AffectTime = 2 } });
        //if (Time.time > primaryCooldownCounter)
        //{
        //    CastSpell(PrimarySpell);
        //    primaryCooldownCounter = Time.time + PrimarySpell.cooldown * statistics.CooldownModifier;
        //}
        TakeDamage(0, new List<ConditionBase>() { new ConditionBase(Conditions.Burn, 3f, 5f) });
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

    public override void SetupCondtionsBar(Canvas canvas)
    {
        conditionsBar.transform.SetParent(canvas.transform);
    }
    #endregion
}