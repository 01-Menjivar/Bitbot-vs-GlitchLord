using UnityEngine;

/// <summary>
/// BugHealth — Gestiona la vida individual de cada bug en el minijuego.
/// </summary>
public class BugHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Aplica daño al bug. Si la vida llega a 0, el bug es eliminado.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Elimina al bug.
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }

    // Métodos para consultar y configurar la vida en tiempo de ejecución o desde editores
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = newMaxHealth;
    }
}
