using UnityEngine;

/// <summary>
/// FileCatcherController — Controlador central para el minijuego 2: "File Catcher".
/// Responsable de inicializar el nivel, coordinar el spawner de archivos, instanciar al jugador y
/// verificar la condición de victoria (supervivencia al tiempo límite).
/// </summary>
public class FileCatcherController : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    [Tooltip("Duración del minijuego en segundos (tiempo de supervivencia requerido)")]
    [SerializeField] private float gameDuration = 45f;

    [Header("Referencias de Componentes")]
    [Tooltip("Referencia al FileSpawner en la escena")]
    [SerializeField] private FileSpawner spawner;
    [Tooltip("Prefab del jugador Bit-Bot (con el script FileCatcherPlayer)")]
    [SerializeField] private GameObject playerPrefab;
    [Tooltip("Punto de aparición inicial del jugador en la escena")]
    [SerializeField] private Transform playerSpawnPoint;

    private bool isMinigameActive = false;
    private GameObject spawnedPlayer;

    private void Awake()
    {
        // Auto-detectar el spawner si no está asignado en el Inspector
        if (spawner == null)
        {
            spawner = FindObjectOfType<FileSpawner>();
        }
    }

    private void Start()
    {
        // Iniciar automáticamente el minijuego para pruebas y al cargar la escena
        StartMinigame();
    }

    /// <summary>
    /// Inicializa y arranca la mecánica completa del minijuego.
    /// </summary>
    public void StartMinigame()
    {
        isMinigameActive = true;

        // 1. Instanciar y posicionar al jugador
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            if (spawnedPlayer != null)
            {
                Destroy(spawnedPlayer);
            }
            spawnedPlayer = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("[FileCatcherController] Prefab del jugador o Spawn Point no asignados.");
        }

        // 2. Activar el generador de archivos
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
        else
        {
            Debug.LogError("[FileCatcherController] No se encontró el componente FileSpawner.");
        }

        // 3. Iniciar el temporizador del nivel
        if (TimerController.Instance != null)
        {
            TimerController.Instance.StartTimer(gameDuration);
        }

        // 4. Mostrar la instrucción visual al jugador en el HUD
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMinigameInstruction("¡Atrapa los archivos y esquiva los virus!");
        }

        // 5. Reiniciar la puntuación para este intento
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        Debug.Log("[FileCatcherController] Minijuego File Catcher iniciado.");
    }

    private void Update()
    {
        if (!isMinigameActive) return;

        // Condición de Victoria: Sobrevivir hasta que el temporizador llegue a 0
        if (TimerController.Instance != null && TimerController.Instance.GetTimer() <= 0.05f)
        {
            // Detener el timer antes de que expire internamente y reste una vida
            TimerController.Instance.StopTimer();
            EndMinigame(true);
        }
    }

    /// <summary>
    /// Finaliza el minijuego de forma exitosa o fallida.
    /// </summary>
    public void EndMinigame(bool success)
    {
        isMinigameActive = false;

        // Detener la generación de archivos
        if (spawner != null)
        {
            spawner.StopSpawning();
        }

        // Asegurar que el temporizador esté detenido
        if (TimerController.Instance != null)
        {
            TimerController.Instance.StopTimer();
        }

        if (success)
        {
            Debug.Log("[FileCatcherController] ¡Victoria! Has sobrevivido el nivel.");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelComplete();
            }
        }
    }
}