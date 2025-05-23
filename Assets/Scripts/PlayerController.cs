using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{
    Warrior,
    Mage,
    Archer,
    Rogue
}

public class PlayerController : MonoBehaviour
{
    [Header("Character Setup")]
    public CharacterClass characterClass = CharacterClass.Warrior;
    
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    
    [Header("Combat")]
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer = 1;
    
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 50;
    public int currentMana;
    public int attackPower = 20;
    public int defense = 10;
    public int experience = 0;
    public int level = 1;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // Movement variables
    private Vector2 moveInput;
    private Vector2 mobileInput;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private bool usingMobileControls = false;
    
    // Combat variables
    private bool canAttack = true;
    private float attackTimer = 0f;
    
    // Class specific stats
    private struct ClassStats
    {
        public int health;
        public int mana;
        public int attack;
        public int defense;
        public float speed;
        public string weaponType;
    }
    
    private Dictionary<CharacterClass, ClassStats> classStatsTable;
    
    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Initialize class stats table
        InitializeClassStats();
        
        // Apply class stats
        ApplyClassStats();
        
        // Initialize health and mana
        currentHealth = maxHealth;
        currentMana = maxMana;
        
        Debug.Log($"Player created as {characterClass} - HP: {maxHealth}, MP: {maxMana}, ATK: {attackPower}");
    }
    
    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleCombat();
        UpdateTimers();
    }
    
    void InitializeClassStats()
    {
        classStatsTable = new Dictionary<CharacterClass, ClassStats>
        {
            {
                CharacterClass.Warrior,
                new ClassStats
                {
                    health = 120,
                    mana = 30,
                    attack = 25,
                    defense = 15,
                    speed = 4f,
                    weaponType = "Sword"
                }
            },
            {
                CharacterClass.Mage,
                new ClassStats
                {
                    health = 80,
                    mana = 100,
                    attack = 30,
                    defense = 5,
                    speed = 3f,
                    weaponType = "Staff"
                }
            },
            {
                CharacterClass.Archer,
                new ClassStats
                {
                    health = 90,
                    mana = 60,
                    attack = 22,
                    defense = 8,
                    speed = 5f,
                    weaponType = "Bow"
                }
            },
            {
                CharacterClass.Rogue,
                new ClassStats
                {
                    health = 85,
                    mana = 50,
                    attack = 20,
                    defense = 6,
                    speed = 6f,
                    weaponType = "Dagger"
                }
            }
        };
    }
    
    void ApplyClassStats()
    {
        if (classStatsTable.ContainsKey(characterClass))
        {
            ClassStats stats = classStatsTable[characterClass];
            maxHealth = stats.health;
            maxMana = stats.mana;
            attackPower = stats.attack;
            defense = stats.defense;
            moveSpeed = stats.speed;
        }
    }
    
    void HandleInput()
    {
        // Check if using mobile controls
        if (usingMobileControls && mobileInput != Vector2.zero)
        {
            moveInput = mobileInput;
        }
        else
        {
            // Movement input (WASD or Arrow Keys)
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            
            // Normalize diagonal movement
            moveInput = moveInput.normalized;
        }
        
        // Dash input (Space or Shift)
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
        
        // Attack input (Left Mouse Button or Z)
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            if (canAttack)
            {
                PerformAttack();
            }
        }
        
        // Class abilities (1, 2, 3, 4 keys)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseClassAbility1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseClassAbility2();
        }
    }
    
    void HandleMovement()
    {
        if (isDashing)
        {
            // Dash movement
            rb.linearVelocity = moveInput * dashSpeed;
        }
        else
        {
            // Normal movement
            rb.linearVelocity = moveInput * moveSpeed;
        }
        
        // Update animator parameters
        if (animator != null)
        {
            animator.SetFloat("Speed", rb.linearVelocity.magnitude);
            animator.SetBool("IsMoving", rb.linearVelocity.magnitude > 0.1f);
        }
        
        // Flip sprite based on movement direction
        if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    
    void StartDash()
    {
        if (moveInput != Vector2.zero)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;
            
            Debug.Log($"{characterClass} performs dash!");
        }
    }
    
    void HandleCombat()
    {
        // Combat logic will be expanded later
    }
    
    void PerformAttack()
    {
        canAttack = false;
        attackTimer = attackCooldown;
        
        // Class-specific attack logic
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                WarriorAttack();
                break;
            case CharacterClass.Mage:
                MageAttack();
                break;
            case CharacterClass.Archer:
                ArcherAttack();
                break;
            case CharacterClass.Rogue:
                RogueAttack();
                break;
        }
    }
    
    void WarriorAttack()
    {
        Debug.Log("Warrior swings sword!");
        // Melee attack in front of player
        DealDamageInRange(attackRange, attackPower);
    }
    
    void MageAttack()
    {
        if (currentMana >= 10)
        {
            currentMana -= 10;
            Debug.Log("Mage casts magic missile!");
            // Ranged magic attack
            DealDamageInRange(attackRange * 1.5f, attackPower);
        }
        else
        {
            Debug.Log("Not enough mana!");
        }
    }
    
    void ArcherAttack()
    {
        Debug.Log("Archer shoots arrow!");
        // Ranged arrow attack
        DealDamageInRange(attackRange * 2f, attackPower);
    }
    
    void RogueAttack()
    {
        Debug.Log("Rogue strikes with dagger!");
        // Fast melee attack with chance to crit
        int damage = Random.Range(0, 100) < 30 ? attackPower * 2 : attackPower; // 30% crit chance
        DealDamageInRange(attackRange * 0.8f, damage);
    }
    
    void DealDamageInRange(float range, int damage)
    {
        // Find all enemies in range
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        
        foreach (Collider2D enemy in enemiesInRange)
        {
            // Deal damage to enemy (will implement enemy health system later)
            Debug.Log($"Dealt {damage} damage to {enemy.name}");
        }
    }
    
    void UseClassAbility1()
    {
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                // Shield Block
                if (currentMana >= 5)
                {
                    currentMana -= 5;
                    Debug.Log("Warrior raises shield!");
                    // Temporarily increase defense
                }
                break;
            case CharacterClass.Mage:
                // Heal
                if (currentMana >= 15)
                {
                    currentMana -= 15;
                    int healAmount = 20;
                    currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                    Debug.Log($"Mage heals for {healAmount} HP!");
                }
                break;
            case CharacterClass.Archer:
                // Multi-shot
                if (currentMana >= 8)
                {
                    currentMana -= 8;
                    Debug.Log("Archer fires multiple arrows!");
                }
                break;
            case CharacterClass.Rogue:
                // Stealth
                if (currentMana >= 10)
                {
                    currentMana -= 10;
                    Debug.Log("Rogue becomes invisible!");
                }
                break;
        }
    }
    
    void UseClassAbility2()
    {
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                // Charge Attack
                Debug.Log("Warrior charges forward!");
                break;
            case CharacterClass.Mage:
                // Fireball
                if (currentMana >= 20)
                {
                    currentMana -= 20;
                    Debug.Log("Mage casts fireball!");
                }
                break;
            case CharacterClass.Archer:
                // Explosive Arrow
                Debug.Log("Archer fires explosive arrow!");
                break;
            case CharacterClass.Rogue:
                // Backstab
                Debug.Log("Rogue attempts backstab!");
                break;
        }
    }
    
    void UpdateTimers()
    {
        // Dash timer
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        
        // Dash cooldown timer
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
        // Attack cooldown timer
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                canAttack = true;
            }
        }
        
        // Mana regeneration
        if (currentMana < maxMana)
        {
            currentMana += Mathf.RoundToInt(Time.deltaTime * 5); // 5 mana per second
            currentMana = Mathf.Min(currentMana, maxMana);
        }
    }
    
    public void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(1, damage - defense);
        currentHealth -= actualDamage;
        
        Debug.Log($"{characterClass} takes {actualDamage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void GainExperience(int exp)
    {
        experience += exp;
        Debug.Log($"Gained {exp} experience! Total: {experience}");
        
        // Level up check (100 exp per level)
        if (experience >= level * 100)
        {
            LevelUp();
        }
    }
    
    void LevelUp()
    {
        level++;
        experience = 0;
        
        // Increase stats based on class
        switch (characterClass)
        {
            case CharacterClass.Warrior:
                maxHealth += 15;
                attackPower += 3;
                defense += 2;
                break;
            case CharacterClass.Mage:
                maxHealth += 8;
                maxMana += 15;
                attackPower += 4;
                break;
            case CharacterClass.Archer:
                maxHealth += 10;
                maxMana += 8;
                attackPower += 3;
                break;
            case CharacterClass.Rogue:
                maxHealth += 10;
                maxMana += 5;
                attackPower += 3;
                moveSpeed += 0.2f;
                break;
        }
        
        currentHealth = maxHealth;
        currentMana = maxMana;
        
        Debug.Log($"Level up! {characterClass} is now level {level}");
    }
    
    void Die()
    {
        Debug.Log($"{characterClass} has died!");
        // Handle player death (restart, respawn, etc.)
    }
    
    // Public methods for UI
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
    
    public float GetManaPercentage()
    {
        return (float)currentMana / maxMana;
    }
    
    public void ChangeClass(CharacterClass newClass)
    {
        characterClass = newClass;
        ApplyClassStats();
        currentHealth = maxHealth;
        currentMana = maxMana;
        Debug.Log($"Changed class to {characterClass}");
    }
    
    // Mobile Controls Integration
    public void SetMobileInput(Vector2 input)
    {
        mobileInput = input;
        usingMobileControls = input != Vector2.zero;
    }
    
    public void PerformMobileAttack()
    {
        if (canAttack)
        {
            PerformAttack();
        }
    }
    
    public void PerformMobileDash()
    {
        if (!isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }
    
    public void UseMobileAbility1()
    {
        UseClassAbility1();
    }
    
    public void UseMobileAbility2()
    {
        UseClassAbility2();
    }
    
    // Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}