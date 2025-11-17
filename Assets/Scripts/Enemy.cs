using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float fire_rate = 2f;
    [SerializeField] private float detectionRange = 8f;

    private EnemyWeapon weapon;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float deltaTimeFire = 0;
    private bool isDead = false;

    void Awake()
    {
        weapon = GetComponent<EnemyWeapon>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Buscar al jugador
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead) return;

        // Rotar hacia el jugador
        RotateTowardsPlayer();

        // Disparar
        deltaTimeFire += Time.deltaTime;
        if (deltaTimeFire > fire_rate && IsPlayerInRange())
        {
            weapon.Shoot();
            deltaTimeFire = 0;
        }
    }

    private void RotateTowardsPlayer()
    {
        if (player == null) return;

        // Rotar sprite según la posición del jugador
        if (player.position.x > transform.position.x)
        {
            // Jugador a la derecha
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            // Jugador a la izquierda
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private bool IsPlayerInRange()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionRange;
    }

    public override void TakeDamage()
    {
        if (isDead) return;

        hearts = Mathf.Clamp(hearts - 1, 0, hearts);

        if (hearts <= 0)
        {
            Die();
        }
        else
        {
            // Animación de daño
            if (animator != null)
                animator.SetTrigger("TakeDamage");
        }
    }

    private void Die()
    {
        isDead = true;

        // Detener cualquier movimiento
        rb.linearVelocity = Vector2.zero;

        // Desactivar colisiones
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
            collider.enabled = false;

        // Reproducir animación de muerte
        if (animator != null)
            animator.SetTrigger("Die");
    }

    public bool IsDead()
    {
        return isDead;
    }

    // Método para ser llamado desde la animación de muerte
    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }
}