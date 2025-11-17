using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    private Animator animator;
    private Enemy enemy;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        // Actualizar parámetros del animator basado en el estado del enemigo
        if (animator != null && enemy != null)
        {
            // Usar el parámetro correcto "memydead" en lugar de "isDead"
            animator.SetBool("memydead", enemy.IsDead());
        }
    }

    // Método que puede ser llamado por Animation Events
    public void OnDeathAnimationComplete()
    {
        // Este método puede ser llamado al final de la animación de muerte
        if (enemy != null && enemy.IsDead())
        {
            Destroy(gameObject);
        }
    }
}