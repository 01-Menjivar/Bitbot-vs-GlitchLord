using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BugSpawner — Genera bugs en posiciones y tipos aleatorios dentro de la pantalla del monitor.
/// </summary>
public class BugSpawner : MonoBehaviour
{
    [Header("Prefabs de Bugs")]
    [SerializeField] private GameObject[] bugPrefabs;

    [Header("Configuración de Área de Spawn")]
    [SerializeField] private SpriteRenderer monitorRenderer;
    [SerializeField] private float paddingX = 2.4f;      // Margen izquierdo y derecho
    [SerializeField] private float paddingTop = 1.6f;    // Margen superior
    // [SerializeField] private float paddingBottom = 2.6f; // Margen inferior (más grande para el teclado)
    [SerializeField] private float paddingBottom = 3.0f; // Margen inferior (más grande para el teclado)

    [Header("Configuración de Tiempos")]
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private bool spawnOnStart = false;

    [Header("Dificultad Progresiva (HP de bugs)")]
    [SerializeField] private int minBugHP = 1;
    [SerializeField] private int maxBugHP = 1; // Al inicio solo 1 clic. Sube con el tiempo.
    [SerializeField] private float hpIncreaseInterval = 15f; // Cada 15s sube el HP máximo en 1

    [Header("Sprites de Estado de los Bugs")]
    [SerializeField] private Sprite spriteIdle;
    [SerializeField] private Sprite spriteDamaged;
    [SerializeField] private Sprite spriteSpin;

    private Coroutine spawnCoroutine;
    private List<GameObject> activeBugs = new List<GameObject>();
    private bool isSpawning = false;
    private float elapsedTime = 0f;

    private void Awake()
    {
        if (monitorRenderer == null)
        {
            // Intentar buscar en el mismo GameObject
            monitorRenderer = GetComponent<SpriteRenderer>();

            // Si no está en el mismo objeto, buscar el GameObject llamado "Monitor" en la escena o sub-jerarquía
            if (monitorRenderer == null)
            {
                GameObject monitorObj = GameObject.Find("Monitor");
                if (monitorObj == null)
                {
                    Transform monitorTransform = transform.Find("Monitor");
                    if (monitorTransform != null)
                    {
                        monitorObj = monitorTransform.gameObject;
                    }
                }

                if (monitorObj != null)
                {
                    monitorRenderer = monitorObj.GetComponent<SpriteRenderer>();
                }
            }
        }
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    private void Update()
    {
        if (isSpawning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Comienza a spawnear bugs de forma periódica.
    /// </summary>
    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            isSpawning = true;
            elapsedTime = 0f;
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    /// <summary>
    /// Detiene la generación periódica de bugs.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    /// <summary>
    /// Destruye todos los bugs activos en pantalla y limpia la lista.
    /// </summary>
    public void ClearActiveBugs()
    {
        foreach (var bug in activeBugs)
        {
            if (bug != null)
            {
                Destroy(bug);
            }
        }
        activeBugs.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnBug();
        }
    }

    /// <summary>
    /// Spawnea un bug individual de tipo y posición aleatoria dentro de los límites del monitor.
    /// </summary>
    public GameObject SpawnBug()
    {
        if (bugPrefabs == null || bugPrefabs.Length == 0)
        {
            Debug.LogWarning("BugSpawner: No hay prefabs de bug asignados.");
            return null;
        }

        if (monitorRenderer == null)
        {
            Debug.LogError("BugSpawner: SpriteRenderer del monitor no asignado.");
            return null;
        }

        // Seleccionar prefab aleatorio
        int randomIndex = Random.Range(0, bugPrefabs.Length);
        GameObject selectedPrefab = bugPrefabs[randomIndex];

        if (selectedPrefab == null) return null;

        // Calcular posición aleatoria dentro de los límites del monitor aplicando el padding
        Bounds bounds = monitorRenderer.bounds;
        float minX = bounds.min.x + paddingX;
        float maxX = bounds.max.x - paddingX;
        float minY = bounds.min.y + paddingBottom;
        float maxY = bounds.max.y - paddingTop;

        // Asegurarse de que el padding no invierta el rango en pantallas muy pequeñas
        if (minX >= maxX)
        {
            minX = bounds.center.x - 0.1f;
            maxX = bounds.center.x + 0.1f;
        }
        if (minY >= maxY)
        {
            minY = bounds.center.y - 0.1f;
            maxY = bounds.center.y + 0.1f;
        }

        Vector3 spawnPos = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            monitorRenderer.transform.position.z - 1.0f // Desplazar 1 unidad hacia la cámara en el eje Z para que no queden tapados
        );

        GameObject spawnedBug = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        
        // Los prefabs ya tienen escala 0.35. Multiplicamos x2.0 para hacerlos visibles pero no gigantes.
        spawnedBug.transform.localScale = new Vector3(
            spawnedBug.transform.localScale.x * 2.0f,
            spawnedBug.transform.localScale.y * 2.0f,
            spawnedBug.transform.localScale.z
        );

        // Garantizar que se dibuje por encima del monitor (que tiene sortingOrder = 5)
        SpriteRenderer bugSR = spawnedBug.GetComponent<SpriteRenderer>();
        if (bugSR != null)
        {
            bugSR.sortingOrder = 6;
        }

        // Inicializar el comportamiento de movimiento (límites del monitor)
        BugBehavior bugBehavior = spawnedBug.GetComponent<BugBehavior>();
        if (bugBehavior != null)
        {
            bugBehavior.Initialize(minX, maxX, minY, maxY);
        }

        // Asignar HP variable según tiempo transcurrido
        BugHealth bugHealth = spawnedBug.GetComponent<BugHealth>();
        if (bugHealth != null)
        {
            int currentMaxHP = Mathf.Min(minBugHP + Mathf.FloorToInt(elapsedTime / hpIncreaseInterval), maxBugHP);
            int assignedHP = Random.Range(minBugHP, currentMaxHP + 1);
            bugHealth.SetMaxHealth(assignedHP);

            // Asignar sprites de estado si están configurados
            if (spriteIdle != null || spriteDamaged != null || spriteSpin != null)
            {
                bugHealth.SetStateSprites(spriteIdle, spriteDamaged, spriteSpin);
            }
        }

        activeBugs.Add(spawnedBug);

        // Remover automáticamente de la lista cuando sea destruido
        StartCoroutine(RemoveFromListWhenDestroyed(spawnedBug));

        return spawnedBug;
    }

    private IEnumerator RemoveFromListWhenDestroyed(GameObject bug)
    {
        while (bug != null)
        {
            yield return null;
        }
        activeBugs.Remove(bug);
    }

    public List<GameObject> GetActiveBugs() => activeBugs;

    /// <summary>
    /// Configura programáticamente los prefabs de bugs a utilizar.
    /// </summary>
    public void SetBugPrefabs(GameObject[] prefabs)
    {
        bugPrefabs = prefabs;
    }

    /// <summary>
    /// Configura programáticamente los paddings de spawn.
    /// </summary>
    public void SetSpawnPadding(float x, float top, float bottom)
    {
        paddingX = x;
        paddingTop = top;
        paddingBottom = bottom;
    }
}
