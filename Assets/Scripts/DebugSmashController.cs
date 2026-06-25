using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// DebugSmashController — Controla el minijuego 3: "Debug Smash — Eliminar Bugs de la Pantalla".
/// Nivel 3: Base de Datos Central.
/// </summary>
public class DebugSmashController : MonoBehaviour
{
    [Header("Configuración de Victoria")]
    [SerializeField] private int bugsRequired = 10; // Cantidad de bugs a eliminar para ganar
    [SerializeField] private float gameDuration = 30f; // Tiempo límite en segundos

    private bool isMinigameActive = false;
    private int bugsDestroyed = 0;
    private BugSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<BugSpawner>();
    }

    private void Update()
    {
        if (!isMinigameActive) return;

        DetectClick();
    }

    /// <summary>
    /// Inicializa el minijuego. Llamado al finalizar la animación de introducción del monitor.
    /// </summary>
    public void StartMinigame()
    {
        isMinigameActive = true;
        bugsDestroyed = 0;

        // Iniciar el generador de bugs
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
        else
        {
            Debug.LogError("DebugSmashController: No se encontró el componente BugSpawner.");
        }

        // Iniciar el temporizador del nivel
        if (TimerController.Instance != null)
        {
            TimerController.Instance.StartTimer(gameDuration);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowMinigameInstruction("¡Elimina todos los bugs antes de que se acabe el tiempo!");
        }
    }

    /// <summary>
    /// Detecta el clic o tap del jugador sobre un bug en el espacio 2D.
    /// </summary>
    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            // Realizar un Raycast 2D en el punto del cursor
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                BugHealth bugHealth = hit.collider.GetComponent<BugHealth>();
                if (bugHealth != null)
                {
                    // Infligir daño al bug (1 punto por clic)
                    bugHealth.TakeDamage(1);
                }
            }
        }
    }

    /// <summary>
    /// Callback llamado por BugHealth cuando un bug muere (su vida llega a 0).
    /// </summary>
    public void OnBugDestroyed(GameObject bug)
    {
        if (!isMinigameActive) return;

        bugsDestroyed++;
        CheckMinigameComplete();
    }

    /// <summary>
    /// Callback opcional por si se quiere reaccionar cuando un bug expira sin ser eliminado.
    /// </summary>
    public void OnBugExpired(GameObject bug)
    {
        // El daño al jugador ya es manejado de manera independiente por BugHealth.cs llamando al LifeManager.
    }

    /// <summary>
    /// Verifica si se cumple la condición de victoria.
    /// </summary>
    private void CheckMinigameComplete()
    {
        if (bugsDestroyed >= bugsRequired)
        {
            EndMinigame(true);
        }
    }

    /// <summary>
    /// Finaliza el minijuego de forma exitosa o fallida.
    /// </summary>
    private void EndMinigame(bool success)
    {
        isMinigameActive = false;

        // Detener spawneo y limpiar bugs restantes
        if (spawner != null)
        {
            spawner.StopSpawning();
            spawner.ClearActiveBugs();
        }

        // Detener temporizador
        if (TimerController.Instance != null)
        {
            TimerController.Instance.StopTimer();
        }

        if (success)
        {
            Debug.Log("DebugSmash completed successfully!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnLevelComplete();
            }
        }
    }

    /// <summary>
    /// Callback llamado por TimerController cuando el cronómetro llega a 0.
    /// </summary>
    public void OnTimerExpired()
    {
        if (!isMinigameActive) return;
        EndMinigame(false);
    }
}