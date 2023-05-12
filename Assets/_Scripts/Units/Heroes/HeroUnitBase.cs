using Assets._Scripts.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroUnitBase : UnitBase
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Stats statistics;
    bool _canMove;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new();

    [SerializeField]
    Spell[] spells = new Spell[5];
    StaffRotation spellRotator;

    [SerializeField]
    public GameObject healthBarManagerObj;
    HealthBarManager healthBar;
    private Animator _anim;
    void Awake() => GameManager.OnBeforeStateChanged += OnStateChanged;

    void OnDestroy() => GameManager.OnBeforeStateChanged -= OnStateChanged;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spellRotator = GetComponentInChildren<StaffRotation>();

        spells[0] = ResourceSystem.Instance.AllSpells.Where(s => s.spellSlot == SpellSlot.Primary).FirstOrDefault();
        spells[1] = ResourceSystem.Instance.AllSpells.Where(s => s.spellSlot == SpellSlot.Secondary).FirstOrDefault();
        spells[2] = ResourceSystem.Instance.AllSpells.Where(s => s.spellSlot == SpellSlot.SpellE).FirstOrDefault();
        spells[3] = ResourceSystem.Instance.AllSpells.Where(s => s.spellSlot == SpellSlot.SpellQ).FirstOrDefault();
        spells[4] = ResourceSystem.Instance.AllSpells.Where(s => s.spellSlot == SpellSlot.Dash).FirstOrDefault();

        healthBar = FindObjectOfType<HealthBarManager>();
        healthBar.SetMaxHealth(statistics.MaxHp);

        _anim = GetComponent<Animator>();
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
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
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

    public override async Task TakeDamage(List<Conditions> conditions, int dmgToTake, float conAffectTime, int affectDmgPerTick)
    {
        statistics.CurrentHp -= dmgToTake;

        await ConditionAffect(conditions, conAffectTime, affectDmgPerTick);

        healthBar.SetHealth(statistics.CurrentHp);

        if (statistics.CurrentHp < 0)
            Die();

        return;
    }

    private async Task ConditionAffect(List<Conditions> conditions, float conAffectTime, int affectDmgPerTick)
    {
        foreach (Conditions con in conditions)
        {
            await Affect(con, conAffectTime, affectDmgPerTick);
        }

        return;
    }

    private async Task Affect(Conditions con, float conAffectTime, int affectDmgPerTick)
    {
        float end;

        switch (con)
        {
            case global::Conditions.Burn:

                await GetTickDmg(conAffectTime, affectDmgPerTick);

                break;
            case global::Conditions.Slow:

                end = Time.time + conAffectTime;
                var tempSpeed = statistics.MovementSpeed;

                while (Time.time < end)
                {
                    statistics.MovementSpeed -= affectDmgPerTick;
                    await Task.Yield();
                }

                statistics.MovementSpeed = tempSpeed;

                break;
            case global::Conditions.Freeze:

                end = Time.time + conAffectTime;

                while (Time.time < end)
                {
                    _canMove = false;
                    await Task.Yield();
                }

                break;
            case global::Conditions.Poison:
                await GetTickDmg(conAffectTime, affectDmgPerTick);
                break;
            case global::Conditions.SpeedUp:

                end = Time.time + conAffectTime;
                var tempMoveSpeed = _canMove;

                while (Time.time < end)
                {
                    statistics.MovementSpeed += affectDmgPerTick;
                    await Task.Yield();
                }

                _canMove = tempMoveSpeed;

                break;
            case global::Conditions.ArmorUp:

                end = Time.time + conAffectTime;
                var tempArmor = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor += affectDmgPerTick;
                    await Task.Yield();
                }

                statistics.Armor = tempArmor;

                break;
            case global::Conditions.ArmorDown:

                end = Time.time + conAffectTime;
                var tempArmorDown = statistics.Armor;

                while (Time.time < end)
                {
                    statistics.Armor -= affectDmgPerTick;
                    await Task.Yield();
                }

                statistics.Armor = tempArmorDown;

                break;
            case global::Conditions.Haste:

                end = Time.time + conAffectTime;
                var tempCooldown = statistics.CooldownModifier;

                while (Time.time < end)
                {
                    statistics.CooldownModifier += affectDmgPerTick;
                    await Task.Yield();
                }

                statistics.CooldownModifier = tempCooldown;

                break;
            case global::Conditions.DmgUp:

                end = Time.time + conAffectTime;
                var tempDmg = statistics.DmgModifier;

                while (Time.time < end)
                {
                    statistics.DmgModifier += affectDmgPerTick;
                    await Task.Yield();
                }

                statistics.DmgModifier = tempDmg;

                break;
            default:
                break;
        }
    }

    private async Task GetTickDmg(float conAffectTime, int affectDmgPerTick)
    {
        var end = Time.time + conAffectTime;
        while (Time.time < end)
        {
            statistics.CurrentHp -= affectDmgPerTick;
            await Task.Delay(1000);
        }
    }

    public override void Die()
    {
        Debug.Log($"{name} is dead");
    }

    #region Attack

    void OnPrimaryAttack()
    {
        CastSpell(0);
    }

    void OnSecondaryAttack()
    {
        CastSpell(1);
    }

    void OnQSpell()
    {
        CastSpell(2);
    }

    void OnESpell()
    {
        CastSpell(3);
    }

    void OnDodge()
    {
        CastSpell(4);
    }

    void CastSpell(int index)
    {
        Debug.Log("Casting spell " + index);

        if (spellRotator != null)
        {
            Instantiate(spells[index].Prefab, spellRotator.WizandStaffFirePint.transform.position, spellRotator.WizandStaffFirePint.transform.rotation);
        }
        else
        {
            Debug.LogWarning("SpellRotator is not assigned!");
        }
    }
    #endregion
}