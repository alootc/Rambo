using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && !player.IsDead())
            {
                Debug.Log("Jugador cayó en zona de muerte - Muerte instantánea");

                // Muerte instantánea - establecer hearts a 0
                player.TakeDamage(); // Primera llamada para reducir un corazón

                // Si aún no está muerto, forzar muerte instantánea
                if (!player.IsDead())
                {
                    // Llamar TakeDamage hasta que muera
                    System.Type type = player.GetType();
                    var heartsField = type.GetField("hearts",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    if (heartsField != null)
                    {
                        heartsField.SetValue(player, 0);
                    }
                    player.TakeDamage(); // Esto debería activar la muerte
                }
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemy.IsDead())
            {
                enemy.TakeDamage();
            }
        }
    }
}