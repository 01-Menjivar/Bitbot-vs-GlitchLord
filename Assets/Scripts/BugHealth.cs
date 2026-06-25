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

        // Buscar el gestor de efectos del Level 3
        Level3Effects effects = FindObjectOfType<Level3Effects>();

        if (currentHealth <= 0)
        {
            if (effects != null)
            {
                effects.PlayDestructionEffect(transform.position);
            }
            Die();
        }
        else
        {
            if (effects != null)
            {
                effects.PlayDamageEffect(gameObject);
            }
        }
    }

    /// <summary>
    /// Elimina al bug y notifica al controlador del minijuego.
    /// </summary>
    private void Die()
    {
        DebugSmashController controller = FindObjectOfType<DebugSmashController>();
        if (controller != null)
        {
            controller.OnBugDestroyed(gameObject);
        }
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
