using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float time_life;

    private Rigidbody2D rb;
    private Vector2 direction;
    private bool isEnemyBullet = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, time_life);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Balas de enemigo - dañar al jugador
        if (isEnemyBullet && other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && !player.IsDead())
            {
                Debug.Log("Bala enemiga golpeó al jugador");
                player.TakeDamage();
            }
            Destroy(gameObject);
        }
        // Balas del jugador - dañar enemigos
        else if (!isEnemyBullet && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead())
            {
                enemy.TakeDamage();
            }
            Destroy(gameObject);
        }
        // Destruir al chocar con paredes/suelo
        else if (other.CompareTag("Wall") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void SetAsEnemyBullet(bool isEnemy)
    {
        isEnemyBullet = isEnemy;
    }
}