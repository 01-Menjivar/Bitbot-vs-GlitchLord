using UnityEngine;

/// <summary>
/// FileCatcherPlayer — Controla el movimiento horizontal del jugador en el Nivel 2 y gestiona
/// las colisiones con los archivos y virus que caen.
/// </summary>
public class FileCatcherPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Velocidad de movimiento lateral")]
    [SerializeField] private float moveSpeed = 7f;
    [Tooltip("Límite horizontal izquierdo (coordenada X)")]
    [SerializeField] private float leftLimit = -8f;
    [Tooltip("Límite horizontal derecho (coordenada X)")]
    [SerializeField] private float rightLimit = 8f;

    [Header("Audio SFX")]
    [SerializeField] private AudioClip caughtSFX;
    [SerializeField] private AudioClip damageSFX;

    [Header("Aesthetics & Feedback (Juiciness)")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private float damageFlashDuration = 0.8f;


    private SpriteRenderer spriteRenderer;
    private bool isHandlingDamage = false;
    private Vector3 originalScale;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;

        // Auto-asignar el sprite normal si no está configurado en el Inspector
        if (normalSprite == null && spriteRenderer != null)
        {
            normalSprite = spriteRenderer.sprite;
        }
    }

    private void Update()
    {
        // 1. Obtener entrada horizontal
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D o Flechas

        // 2. Mover el transform de forma directa
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // 3. Restringir la posición dentro de los límites del escenario
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, leftLimit, rightLimit);
        transform.position = currentPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detectar colisiones con objetos que caen
        FallingObject fallingObject = other.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            if (fallingObject.IsValid())
            {
                // Archivo Válido recolectado — sumar puntuación y tiempo
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.OnValidFileCaught();
                }
                Debug.Log($"[FileCatcherPlayer] ¡Archivo atrapado! Puntuación actual: {(ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0)}");

                // Reproducir efecto de sonido de acierto
                if (AudioManager.Instance != null && caughtSFX != null)
                {
                    AudioManager.Instance.PlaySFX(caughtSFX);
                }

                // Destruir el objeto
                Destroy(other.gameObject);
            }
            else
            {
                // Virus golpeado — restar puntuación y tiempo
                Debug.LogWarning("[FileCatcherPlayer] ¡Virus golpeado! Perdiendo vida...");

                // Registrar penalización de puntuación y tiempo en el ScoreManager
                if (ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.OnVirusHit();
                }

                // Reproducir efecto de sonido de daño
                if (AudioManager.Instance != null && damageSFX != null)
                {
                    AudioManager.Instance.PlaySFX(damageSFX);
                }

                // Restar una vida al LifeManager
                if (LifeManager.Instance != null)
                {
                    LifeManager.Instance.LoseLife();
                }

                // Disparar feedback estético y sacudida de pantalla
                if (!isHandlingDamage && gameObject.activeInHierarchy)
                {
                    StartCoroutine(DamageReactionCoroutine());
                }

                // Destruir el objeto
                Destroy(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Corutina que maneja el parpadeo de glitch (rojo/cian), deformación elástica de impacto
    /// y sacudida de la cámara principal al recibir daño.
    /// </summary>
    private System.Collections.IEnumerator DamageReactionCoroutine()
    {
        isHandlingDamage = true;

        // 1. Cambiar sprite a triste
        if (spriteRenderer != null && sadSprite != null)
        {
            spriteRenderer.sprite = sadSprite;
        }

        // 2. Generar explosión de partículas glitch binarias (0 y 1)
        GlitchParticle.SpawnBurst(transform.position, 12);

        float elapsed = 0f;
        float flashInterval = 0.08f;
        float lastFlashTime = 0f;
        bool flashToggle = false;

        // Referencias para el screen shake de la cámara
        Transform cameraTransform = Camera.main != null ? Camera.main.transform : null;
        Vector3 originalCameraPos = cameraTransform != null ? cameraTransform.position : Vector3.zero;

        while (elapsed < damageFlashDuration)
        {
            elapsed += Time.deltaTime;

            // Parpadeo de color tipo Glitch (Alterna tinte rojo de corrupción y cian digital)
            if (elapsed - lastFlashTime >= flashInterval)
            {
                lastFlashTime = elapsed;
                flashToggle = !flashToggle;

                if (spriteRenderer != null)
                {
                    spriteRenderer.color = flashToggle ? new Color(1f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.9f, 1f, 1f);
                }
            }

            // Deformación física elástica (aplastamiento horizontal e incremento vertical)
            if (elapsed < damageFlashDuration * 0.4f)
            {
                float t = elapsed / (damageFlashDuration * 0.4f);
                transform.localScale = Vector3.Lerp(originalScale, new Vector3(originalScale.x * 0.7f, originalScale.y * 1.3f, originalScale.z), t);
            }
            else
            {
                float t = (elapsed - damageFlashDuration * 0.4f) / (damageFlashDuration * 0.6f);
                transform.localScale = Vector3.Lerp(new Vector3(originalScale.x * 0.7f, originalScale.y * 1.3f, originalScale.z), originalScale, t);
            }

            // Sacudida de pantalla (Screen Shake) en los primeros 0.25 segundos
            if (cameraTransform != null && elapsed < 0.25f)
            {
                float shakeMagnitude = 0.15f * (1.0f - (elapsed / 0.25f)); // Disminuye progresivamente
                Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
                cameraTransform.position = new Vector3(originalCameraPos.x + randomOffset.x, originalCameraPos.y + randomOffset.y, originalCameraPos.z);
            }
            else if (cameraTransform != null && elapsed >= 0.25f)
            {
                // Devolver cámara a su posición original estable
                cameraTransform.position = originalCameraPos;
            }

            yield return null;
        }

        // Restablecer valores originales
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            if (normalSprite != null)
            {
                spriteRenderer.sprite = normalSprite;
            }
        }
        transform.localScale = originalScale;
        
        if (cameraTransform != null)
        {
            cameraTransform.position = originalCameraPos;
        }

        isHandlingDamage = false;
    }

    /// <summary>
    /// Retorna la puntuación actual del jugador en este nivel.
    /// </summary>
    public int GetScore() => ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;
}
