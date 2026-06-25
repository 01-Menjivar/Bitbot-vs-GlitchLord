using UnityEngine;

/// <summary>
/// BugBehavior — Controla el movimiento aleatorio y el rebote en los límites del monitor para los bugs del Nivel 3.
/// </summary>
public class BugBehavior : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 2.0f;

    private Vector2 direction;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private bool isInitialized = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Elegir una dirección inicial aleatoria (360 grados)
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        UpdateSpriteFlipping();
    }

    /// <summary>
    /// Inicializa las fronteras de movimiento permitidas en coordenadas del mundo.
    /// </summary>
    public void Initialize(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY + 1.0f;
        this.maxY = maxY;
        isInitialized = true;
    }

    /// <summary>
    /// Permite ajustar la velocidad dinámicamente si es necesario.
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Calcular nueva posición propuesta
        Vector3 newPosition = transform.position + (Vector3)(direction * speed * Time.deltaTime);
        bool bounced = false;

        // Comprobar rebote y límites en el eje X
        if (newPosition.x < minX)
        {
            newPosition.x = minX;
            direction.x = Mathf.Abs(direction.x); // Ir a la derecha
            bounced = true;
        }
        else if (newPosition.x > maxX)
        {
            newPosition.x = maxX;
            direction.x = -Mathf.Abs(direction.x); // Ir a la izquierda
            bounced = true;
        }

        // Comprobar rebote y límites en el eje Y
        if (newPosition.y < minY)
        {
            newPosition.y = minY;
            direction.y = Mathf.Abs(direction.y); // Ir hacia arriba
            bounced = true;
        }
        else if (newPosition.y > maxY)
        {
            newPosition.y = maxY;
            direction.y = -Mathf.Abs(direction.y); // Ir hacia abajo
            bounced = true;
        }

        // Si rebotó, actualizar volteado del sprite
        if (bounced)
        {
            UpdateSpriteFlipping();
        }

        // Aplicar posición actualizada
        transform.position = newPosition;
    }

    private void UpdateSpriteFlipping()
    {
        if (spriteRenderer != null)
        {
            // Voltear horizontalmente según la dirección del movimiento en el eje X
            spriteRenderer.flipX = direction.x > 0;
        }
    }
}
