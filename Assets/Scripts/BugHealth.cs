using UnityEngine;
using System.Collections;

/// <summary>
/// BugHealth — Gestiona la vida individual de cada bug en el minijuego.
/// Ahora soporta múltiples golpes, sprites de estado y animación de muerte (Spin_Virus).
/// </summary>
public class BugHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 1;
    private int currentHealth;

    [Header("Sprites de Estado (Virus)")]
    [SerializeField] private Sprite spriteIdle;     // 1 HP — Idle_Virus
    [SerializeField] private Sprite spriteDamaged;  // 2+ HP al recibir golpe — Damaged_Virus
    [SerializeField] private Sprite spriteSpin;     // Al morir — Spin_Virus (animación de muerte)

    [Header("Animación de Muerte")]
    [SerializeField] private float spinDuration = 0.5f; // Duración del spin antes de destruirse
    [SerializeField] private float spinSpeed = 720f;    // Grados por segundo

    private SpriteRenderer spriteRenderer;
    private bool isDead = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateSprite();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Level3Effects effects = FindObjectOfType<Level3Effects>();

        if (currentHealth <= 0)
        {
            isDead = true;
            if (effects != null) effects.PlayDestructionEffect(transform.position);
            StartCoroutine(DeathSpinRoutine());
        }
        else
        {
            UpdateSprite();
            if (effects != null)
            {
                effects.PlayDamageEffect(gameObject);
                if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("VirusHit");
            }
        }
    }

    /// <summary>
    /// Actualiza el sprite según la vida actual del bug.
    /// </summary>
    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        if (currentHealth >= maxHealth && spriteIdle != null)
            spriteRenderer.sprite = spriteIdle;
        else if (currentHealth < maxHealth && spriteDamaged != null)
            spriteRenderer.sprite = spriteDamaged;
    }

    /// <summary>
    /// Muestra el sprite Spin_Virus rotando antes de destruirse.
    /// </summary>
    private IEnumerator DeathSpinRoutine()
    {
        // Desactivar el collider para que no reciba más clics
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Cambiar al sprite de spin/muerte
        if (spriteRenderer != null && spriteSpin != null)
            spriteRenderer.sprite = spriteSpin;

        // Notificar al controlador ANTES de la animación para que cuente la curación
        DebugSmashController controller = FindObjectOfType<DebugSmashController>();
        if (controller != null)
            controller.OnBugDestroyed(gameObject);

        // Reproducir SFX de destrucción
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("BugDestroy");

        // Animación: rotar y encoger
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;
        while (elapsed < spinDuration)
        {
            if (this == null || gameObject == null) yield break;
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    /// <summary>
    /// Permite al BugSpawner inyectar los sprites desde el Inspector centralmente.
    /// </summary>
    public void SetStateSprites(Sprite idle, Sprite damaged, Sprite spin)
    {
        spriteIdle = idle;
        spriteDamaged = damaged;
        spriteSpin = spin;
        UpdateSprite(); // Aplicar inmediatamente
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = newMaxHealth;
        UpdateSprite();
    }
}
