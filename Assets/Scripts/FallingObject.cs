using UnityEngine;

/// <summary>
/// FallingObject — Controla el comportamiento individual de cada objeto que cae (archivos y virus).
/// Maneja el movimiento de caída, la rotación sutil, patrones de movimiento físicos y el ciclo de vida.
/// </summary>
public class FallingObject : MonoBehaviour
{
    public enum FileType
    {
        Virus,
        BlueFile,
        GreenFile,
        GoldFile
    }

    public enum MovementPattern
    {
        Straight,
        Sway,
        Diagonal,
        Accelerating
    }

    private float baseSpeed;
    private float currentSpeed;
    private FileType fileType;
    private MovementPattern movementPattern;
    private float rotationSpeed;
    private float destroyYThreshold = -6f; // Límite inferior de la pantalla para destruir el objeto

    // Parámetros específicos para patrones de movimiento
    private float spawnX;
    private float swayAmplitude;
    private float swayFrequency;
    private float swayOffset;
    private float diagonalDirection; // Desplazamiento en X (-0.3f a 0.3f)
    private float accelerationRate;

    /// <summary>
    /// Inicializa las propiedades del objeto cuando es instanciado por el FileSpawner.
    /// </summary>
    public void Initialize(float speed, FileType type, MovementPattern pattern)
    {
        baseSpeed = speed;
        currentSpeed = speed;
        fileType = type;
        movementPattern = pattern;
        spawnX = transform.position.x;

        // Configuración de rotación y atributos especiales por tipo
        if (type == FileType.GoldFile)
        {
            // El dorado gira muy rápido ("giro loco")
            rotationSpeed = Random.Range(-120f, -80f) * (Random.value < 0.5f ? 1f : -1f);
        }
        else if (type == FileType.Virus)
        {
            rotationSpeed = Random.Range(-80f, 80f);
        }
        else
        {
            rotationSpeed = Random.Range(-40f, 40f);
        }

        // Inicializar parámetros para patrones específicos
        swayAmplitude = Random.Range(0.5f, 1.2f);
        swayFrequency = Random.Range(2f, 4.5f);
        swayOffset = Random.Range(0f, Mathf.PI * 2f);
        diagonalDirection = Random.value < 0.5f ? -0.3f : 0.3f;
        accelerationRate = Random.Range(1.5f, 3.5f); // Ritmo de aceleración de velocidad

        // Aplicar tintes de color neón en código
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            switch (type)
            {
                case FileType.Virus:
                    spriteRenderer.color = new Color(0.9f, 0.2f, 0.8f); // Neon Magenta/Violeta
                    break;
                case FileType.BlueFile:
                    spriteRenderer.color = new Color(0.2f, 0.6f, 1.0f); // Azul Neon
                    break;
                case FileType.GreenFile:
                    spriteRenderer.color = new Color(0.2f, 1.0f, 0.4f); // Verde Neon
                    break;
                case FileType.GoldFile:
                    spriteRenderer.color = new Color(1.0f, 0.85f, 0.1f); // Oro Brillante
                    break;
            }
        }
    }

    private void Update()
    {
        // 1. Calcular posición basándose en el patrón de movimiento
        Vector3 pos = transform.position;

        switch (movementPattern)
        {
            case MovementPattern.Straight:
                pos.y -= currentSpeed * Time.deltaTime;
                break;
            case MovementPattern.Sway:
                pos.y -= currentSpeed * Time.deltaTime;
                // Movimiento horizontal tipo onda seno
                pos.x = spawnX + Mathf.Sin(Time.time * swayFrequency + swayOffset) * swayAmplitude;
                break;
            case MovementPattern.Diagonal:
                pos.y -= currentSpeed * Time.deltaTime;
                // Desplazamiento diagonal lateral constante
                pos.x += diagonalDirection * currentSpeed * Time.deltaTime;
                break;
            case MovementPattern.Accelerating:
                // Velocidad aumenta con el tiempo (gravedad)
                currentSpeed += accelerationRate * Time.deltaTime;
                pos.y -= currentSpeed * Time.deltaTime;
                break;
        }

        transform.position = pos;

        // 2. Aplicar rotación sutil en el eje Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 3. Auto-destruirse si cae por debajo del límite vertical establecido
        if (transform.position.y < destroyYThreshold)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Retorna si el objeto es un archivo válido o un virus.
    /// </summary>
    public bool IsValid() => fileType != FileType.Virus;

    /// <summary>
    /// Retorna el tipo de archivo específico.
    /// </summary>
    public FileType GetFileType() => fileType;

    private void OnBecameInvisible()
    {
        // Medida de seguridad adicional para prevenir fugas de memoria si el objeto sale de la cámara
        Destroy(gameObject);
    }
}

