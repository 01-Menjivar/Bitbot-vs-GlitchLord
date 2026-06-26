using System.Collections;
using UnityEngine;

/// <summary>
/// FileSpawner — Gestiona la generación de archivos válidos (azules/verdes) y corruptos (virus) en la parte superior.
/// Controla los intervalos de aparición, la distribución de probabilidades y el escalado de dificultad progresivo.
/// </summary>
public class FileSpawner : MonoBehaviour
{
    [Header("Prefabs de Objetos")]
    [Tooltip("0: Prefab de Archivo Válido (ValidFile), 1: Prefab de Archivo Corrupto (CorruptFile)")]
    [SerializeField] private GameObject[] filePrefabs;

    [Header("Configuración de Spawn")]
    [Tooltip("Punto de spawn superior (Transform)")]
    [SerializeField] private Transform spawnZone;
    [Tooltip("Ancho de la zona de aparición horizontal")]
    [SerializeField] private float spawnWidth = 8f;

    [Header("Parámetros del Minijuego (Dificultad)")]
    [Tooltip("Intervalo de tiempo inicial entre spawns")]
    [SerializeField] private float spawnInterval = 1.5f;
    [Tooltip("Intervalo mínimo de tiempo entre spawns (dificultad máxima)")]
    [SerializeField] private float minSpawnInterval = 0.4f;
    [Tooltip("Reducción del intervalo de spawn por ciclo de escalado")]
    [SerializeField] private float difficultyRamp = 0.05f;
    [Tooltip("Velocidad de caída inicial de los objetos")]
    [SerializeField] private float baseFallSpeed = 3f;
    [Tooltip("Velocidad de caída máxima de los objetos")]
    [SerializeField] private float maxFallSpeed = 8f;
    [Tooltip("Probabilidad inicial de generar un virus (de 0 a 1)")]
    [Range(0f, 1f)] [SerializeField] private float virusChance = 0.3f;

    // Variables internas de estado
    private float currentSpawnInterval;
    private float currentFallSpeed;
    private float currentVirusChance;
    private bool isSpawning = false;
    private Coroutine spawnCoroutine;
    private Coroutine difficultyCoroutine;

    private void Awake()
    {
        ResetParameters();
    }

    /// <summary>
    /// Restablece los parámetros de dificultad y velocidad a sus valores iniciales.
    /// </summary>
    public void ResetParameters()
    {
        currentSpawnInterval = spawnInterval;
        currentFallSpeed = baseFallSpeed;
        currentVirusChance = virusChance;
    }

    /// <summary>
    /// Inicia la generación de archivos y el ciclo de aumento de dificultad.
    /// </summary>
    public void StartSpawning()
    {
        if (isSpawning) return;
        isSpawning = true;
        ResetParameters();
        spawnCoroutine = StartCoroutine(SpawnSequence());
        difficultyCoroutine = StartCoroutine(DifficultyProgressSequence());
        
        Debug.Log("[FileSpawner] Spawner iniciado con éxito.");
    }

    /// <summary>
    /// Detiene el spawner y limpia todos los archivos que estén cayendo en pantalla.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        if (difficultyCoroutine != null) StopCoroutine(difficultyCoroutine);
        
        DestroyAllFallingObjects();
        Debug.Log("[FileSpawner] Spawner detenido y objetos limpiados.");
    }

    private IEnumerator SpawnSequence()
    {
        while (isSpawning)
        {
            SpawnFile();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    /// <summary>
    /// Escala la dificultad de forma progresiva cada 5 segundos de juego.
    /// </summary>
    private IEnumerator DifficultyProgressSequence()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(5f);

            // 1. Reducir el intervalo entre apariciones (más objetos por segundo)
            currentSpawnInterval = Mathf.Max(currentSpawnInterval - difficultyRamp, minSpawnInterval);

            // 2. Incrementar la velocidad de caída de forma lineal hacia el límite máximo (aproximadamente un 5% del rango por ciclo)
            float speedStep = (maxFallSpeed - baseFallSpeed) * 0.05f;
            currentFallSpeed = Mathf.Min(currentFallSpeed + speedStep, maxFallSpeed);

            // 3. Aumentar progresivamente la probabilidad de virus de 30% hasta un tope de 50%
            currentVirusChance = Mathf.Min(currentVirusChance + 0.02f, 0.5f);

            Debug.Log($"[FileSpawner - Dificultad Escalada] Intervalo: {currentSpawnInterval:F2}s, Velocidad: {currentFallSpeed:F2}, Prob. Virus: {currentVirusChance:F2}");
        }
    }

    /// <summary>
    /// Instancia un archivo o virus en un punto X aleatorio de la zona de aparición.
    /// </summary>
    private void SpawnFile()
    {
        if (filePrefabs == null || filePrefabs.Length < 2)
        {
            Debug.LogError("[FileSpawner] Prefabs de archivos no asignados en el Inspector.");
            return;
        }

        // Determinar si generar virus o archivo válido según la probabilidad
        bool isVirus = Random.value < currentVirusChance;
        GameObject prefabToInstantiate = isVirus ? filePrefabs[1] : filePrefabs[0];

        if (prefabToInstantiate == null) return;

        // Calcular posición aleatoria en X en la zona superior
        float randomX = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
        Vector3 spawnPosition = spawnZone != null ? spawnZone.position : transform.position;
        spawnPosition.x += randomX;

        // Instanciar
        GameObject spawnedObj = Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);

        // Inicializar la velocidad y el estado del FallingObject
        FallingObject fallingComponent = spawnedObj.GetComponent<FallingObject>();
        if (fallingComponent != null)
        {
            fallingComponent.Initialize(currentFallSpeed, !isVirus);
        }

        // Aplicar tintes de color si es un archivo válido (alterna entre azul o verde)
        if (!isVirus)
        {
            SpriteRenderer spriteRenderer = spawnedObj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                bool tintGreen = Random.value < 0.5f;
                spriteRenderer.color = tintGreen ? new Color(0.4f, 1f, 0.6f) : new Color(0.4f, 0.7f, 1f);
            }
        }
    }

    /// <summary>
    /// Destruye todos los objetos que están cayendo en la escena de forma inmediata.
    /// </summary>
    private void DestroyAllFallingObjects()
    {
        FallingObject[] activeObjects = FindObjectsOfType<FallingObject>();
        foreach (var obj in activeObjects)
        {
            Destroy(obj.gameObject);
        }
    }
}
