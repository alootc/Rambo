using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject bulletPrefab;

    private Enemy enemy;
    private Transform player;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void Shoot()
    {
        if (player == null || enemy.IsDead()) return;

        GameObject bullet = Instantiate(bulletPrefab, pivot.position, Quaternion.identity);

        // Calcular dirección hacia el jugador
        Vector2 direction = (player.position - pivot.position).normalized;
        bullet.GetComponent<Bullet>().SetDirection(direction);

        // Marcar la bala como enemiga
        bullet.GetComponent<Bullet>().SetAsEnemyBullet(true);
    }
}