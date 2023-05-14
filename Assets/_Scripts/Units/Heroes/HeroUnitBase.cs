using Assets._Scripts.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    public override async Task TakeDamage(float dmgToTake, List<ConditionBase> conditions)
    {
        statistics.CurrentHp -= Convert.ToInt32(dmgToTake * statistics.Armor);

        healthBar.SetHealth(statistics.CurrentHp);

        if (statistics.CurrentHp <= 0)
            Die();

        await ConditionAffect(conditions);

        return;
    }

    #region Conditions

    private async Task ConditionAffect(List<ConditionBase> conditions)
    {
        var conTokenSource = new CancellationTokenSource();
        foreach (ConditionBase condition in conditions)
        {
            await Affect(condition, conTokenSource);
        }
        Task.WaitAll();
        conTokenSource.Dispose();

        return;
    }

    private async Task Affect(ConditionBase condition, CancellationTokenSource conTokenSource)
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
               
                break;
            case global::Conditions.Slow:

                if (!IsSlowed)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(SlowTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(SlowTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.Freeze:

                if (!IsFreezed)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(FreezeTask(ct, condition.AffectTime));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(FreezeTask(ct, condition.AffectTime));
                }

                break;
            case global::Conditions.Poison:

                if (!IsPoisoned)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(PoisonTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(PoisonTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.SpeedUp:

                if (!IsSpeededUp)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(SpeedUpTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(SlowTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.ArmorUp:

                if (!HasArmorUp)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(ArmorUpTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(SlowTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.ArmorDown:

                if (!HasArmorDown)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(ArmorDownTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(ArmorDownTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.Haste:

                if (!HasHaste)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(HasteTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(HasteTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            case global::Conditions.DmgUp:

                if (!HasDmgUp)
                {
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(DmgUpTask(ct, condition.AffectTime, condition.AffectOnTick));
                }
                else
                {
                    conTokenSource.Cancel();
                    CancellationToken ct = conTokenSource.Token;
                    await Task.Run(DmgUpTask(ct, condition.AffectTime, condition.AffectOnTick));
                }

                break;
            default:
                break;
        }
    }

    //private async void DealWithCon(bool IsAffected, Action affectTask, CancellationTokenSource conTokenSource)
    //{
    //    var conTokenSourcee = new CancellationTokenSource();

    //    if (!IsAffected)
    //    {
    //        CancellationToken ct = conTokenSourcee.Token;
    //        await Task.Run(affectTask);
    //    }
    //    else
    //    {
    //        conTokenSource.Cancel();
    //        CancellationToken ct = conTokenSourcee.Token;
    //        await Task.Run(affectTask);
    //    }

    //    conTokenSourcee.Dispose();
    //}

    private Action BurnTask(CancellationToken ct, float burnTime, float burnDmgPerTick)
    {
        return async () =>
        {
            IsBurning = true;

            var end = DateTime.Now.Second + burnTime;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Delay(1000);
                statistics.CurrentHp -= Convert.ToInt32(burnDmgPerTick);

                healthBar.SetHealth(statistics.CurrentHp);

                if (statistics.CurrentHp <= 0)
                    Die();
            }

            IsBurning = false;
        };
    }

    private Action SlowTask(CancellationToken ct, float slowTime, float slowPercent)
    {
        return async () =>
        {
            IsSlowed = true;

            var end = DateTime.Now.Second + slowTime;
            var tempSpeed = statistics.MovementSpeed;

            statistics.MovementSpeed -= statistics.MovementSpeed * slowPercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.MovementSpeed = tempSpeed;
            IsSlowed = false;
        };
    }

    private Action FreezeTask(CancellationToken ct, float freezeTime)
    {
        return async () =>
        {
            IsFreezed = true;
            _canMove = false;

            var end = DateTime.Now.Second + freezeTime;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            _canMove = true;
            IsFreezed = false;
        };
    }

    private Action PoisonTask(CancellationToken ct, float poisonTime, float poisonDmgPerTick)
    {
        return async () =>
        {
            IsPoisoned = true;

            var end = DateTime.Now.Second + poisonTime;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Delay(1000);

                statistics.CurrentHp -= Convert.ToInt32(poisonDmgPerTick);

                healthBar.SetHealth(statistics.CurrentHp);

                if (statistics.CurrentHp <= 0)
                    Die();
            }

            IsPoisoned = false;
        };
    }

    private Action SpeedUpTask(CancellationToken ct, float speedUpTime, float speedUpPercent)
    {
        return async () =>
        {
            IsSpeededUp = true;

            var end = DateTime.Now.Second + speedUpTime;
            var tempMoveSpeed = statistics.MovementSpeed;

            statistics.MovementSpeed += statistics.MovementSpeed * speedUpPercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.MovementSpeed = tempMoveSpeed;
            IsSpeededUp = false;
        };
    }

    private Action ArmorUpTask(CancellationToken ct, float armorUpTime, float armorUpPercent)
    {
        return async () =>
        {
            HasArmorUp = true;

            var end = DateTime.Now.Second + armorUpTime;
            var tempArmor = statistics.Armor;

            statistics.Armor += statistics.Armor * armorUpPercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.Armor = tempArmor;
            HasArmorUp = false;
        };
    }

    private Action ArmorDownTask(CancellationToken ct, float armorDownTime, float armorDownPercent)
    {
        return async () =>
        {
            HasArmorDown = true;

            var end = DateTime.Now.Second + armorDownTime;
            var tempArmorDown = statistics.Armor;

            statistics.Armor -= statistics.Armor * armorDownPercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.Armor = tempArmorDown;
            HasArmorDown = false;
        };
    }

    private Action HasteTask(CancellationToken ct, float hasteTime, float hastePercent)
    {
        return async () =>
        {
            HasHaste = true;

            var end = DateTime.Now.Second + hasteTime;
            var tempCooldown = statistics.CooldownModifier;

            statistics.CooldownModifier += statistics.CooldownModifier * hastePercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.CooldownModifier = tempCooldown;
            HasHaste = false;
        };
    }

    private Action DmgUpTask(CancellationToken ct, float dmgUpTime, float dmgUpPercent)
    {
        return async () =>
        {
            HasDmgUp = true;

            var end = DateTime.Now.Second + dmgUpTime;
            var tempDmg = statistics.DmgModifier;

            statistics.DmgModifier += statistics.DmgModifier * dmgUpPercent;

            while (DateTime.Now.Second < end)
            {
                if (ct.IsCancellationRequested)
                    return;

                await Task.Yield();
            }

            statistics.DmgModifier = tempDmg;
            HasDmgUp = false;
        };
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