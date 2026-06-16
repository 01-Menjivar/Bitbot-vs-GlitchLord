using UnityEngine;

/// <summary>
/// DebugSmashController — Controla el minijuego 3: "Debug Smash — Eliminar Bugs de la Pantalla".
/// Nivel 3: Base de Datos Central.
///
/// Mecánica base (según reporte):
/// - Bugs aparecen en posiciones aleatorias en pantalla.
/// - El jugador hace clic sobre cada bug para eliminarlo.
/// - Los bugs pueden desaparecer o multiplicarse si no se eliminan a tiempo.
/// - Algunos bugs son más rápidos o más pequeños que otros.
/// - La dificultad aumenta con la cantidad de bugs simultáneos y velocidad de aparición.
/// - PENDIENTE: Oscar define los tipos de bug (normal, especial) y sus comportamientos exactos.
/// </summary>
public class DebugSmashController : MonoBehaviour
{
    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private bool isMinigameActive = false;

    // Progreso del minijuego
    // private int bugsDestroyed = 0;
    // private int bugsRequired; // PENDIENTE: Oscar define cuántos bugs hay que eliminar

    // Control de bugs activos en pantalla
    // private List<GameObject> activeBugs = new List<GameObject>();
    // private int maxBugsOnScreen; // PENDIENTE: definir límite máximo simultáneo

    // Dificultad progresiva
    // private float spawnInterval;      // Tiempo entre aparición de bugs
    // private float bugLifetime;        // Tiempo antes de que un bug desaparezca o se multiplique
    // PENDIENTE: definir valores y curva de escalado con el equipo

    // -------------------------------------------------------
    // INICIO DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Inicializa el minijuego. Llamar desde GameManager al cargar el Nivel 3.
    /// </summary>
    public void StartMinigame()
    {
        isMinigameActive = true;
        // bugsDestroyed = 0;

        // PENDIENTE: iniciar spawner de bugs
        // PENDIENTE: definir área de aparición (coordenadas del Tilemap Nivel 3)
        // TimerController.Instance.StartTimer(tiempoDefinidoPorEquipo);
        // UIManager.Instance.ShowMinigameInstruction("¡Elimina todos los bugs antes de que se multipliquen!");
    }

    // -------------------------------------------------------
    // SPAWNER DE BUGS
    // -------------------------------------------------------

    /// <summary>
    /// Genera bugs en posiciones aleatorias dentro del área de juego.
    /// PENDIENTE: Oscar define los tipos de bug y sus propiedades (tamaño, velocidad, comportamiento).
    /// PENDIENTE: definir si el spawner es un script separado (BugSpawner.cs).
    /// </summary>
    private void SpawnBug()
    {
        // Elegir tipo de bug aleatoriamente (normal o especial)
        // Instanciar prefab en posición aleatoria dentro del área definida en el Tilemap
        // PENDIENTE: los prefabs de bug los define Oscar en Assets/Art/Enemies
    }

    // -------------------------------------------------------
    // DETECCIÓN DE CLIC SOBRE UN BUG
    // -------------------------------------------------------

    /// <summary>
    /// Detecta el clic del jugador sobre un bug y lo elimina.
    /// PENDIENTE: definir si la detección es por Raycast desde la cámara o por OnMouseDown en cada bug.
    /// Se recomienda un script individual por bug (BugController.cs) que maneje su propio clic.
    /// </summary>
    private void DetectClick()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        //     if (hit.collider != null && hit.collider.CompareTag("Bug"))
        //     {
        //         OnBugClicked(hit.collider.gameObject);
        //     }
        // }
    }

    /// <summary>
    /// Se ejecuta al hacer clic exitosamente sobre un bug.
    /// </summary>
    private void OnBugClicked(GameObject bug)
    {
        // bugsDestroyed++;
        // AudioManager.Instance.PlaySFX(sfxBugDestroyed);
        // activeBugs.Remove(bug);
        // Destroy(bug);
        // CheckMinigameComplete();

        // PENDIENTE: ¿hay feedback visual al eliminar un bug (explosión, partículas)?
    }

    // -------------------------------------------------------
    // COMPORTAMIENTO DE BUGS (tiempo de vida)
    // -------------------------------------------------------

    /// <summary>
    /// Maneja lo que ocurre si un bug no es eliminado a tiempo.
    /// PENDIENTE: Oscar define si el bug desaparece, se multiplica, o ambos según el tipo.
    /// </summary>
    private void OnBugExpired(GameObject bug)
    {
        // Opción A: el bug desaparece (se escapó)
        // activeBugs.Remove(bug);
        // Destroy(bug);

        // Opción B: el bug se multiplica (genera 1 o 2 bugs adicionales)
        // SpawnBug();
        // SpawnBug();

        // PENDIENTE: decidir cuál aplica por tipo de bug con Oscar.
    }

    // -------------------------------------------------------
    // DIFICULTAD PROGRESIVA
    // -------------------------------------------------------

    /// <summary>
    /// Aumenta la cantidad y velocidad de aparición de bugs con el tiempo.
    /// PENDIENTE: definir la curva de escalado con el equipo.
    /// </summary>
    private void IncreaseDifficulty()
    {
        // spawnInterval = Mathf.Max(spawnInterval - incremento, minSpawnInterval);
        // maxBugsOnScreen++;
        // PENDIENTE: ¿también se reduce el bugLifetime conforme avanza el tiempo?
    }

    // -------------------------------------------------------
    // CONDICIÓN DE VICTORIA DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Verifica si se cumplió la condición de victoria.
    /// PENDIENTE: definir si se gana eliminando X bugs o sobreviviendo el tiempo completo.
    /// </summary>
    private void CheckMinigameComplete()
    {
        // if (bugsDestroyed >= bugsRequired)
        // {
        //     EndMinigame();
        // }
    }

    private void EndMinigame()
    {
        isMinigameActive = false;
        // TimerController.Instance.StopTimer();
        // GameManager.Instance.OnLevelComplete();
    }

    // -------------------------------------------------------
    // CONDICIÓN DE DERROTA (Timer expirado)
    // -------------------------------------------------------

    /// <summary>
    /// Llamar desde TimerController cuando el tiempo llega a 0.
    /// PENDIENTE: igual que en FileCatcher, depende de si la victoria es por cantidad o por tiempo.
    /// </summary>
    public void OnTimerExpired()
    {
        isMinigameActive = false;
        // LifeManager.Instance.LoseLife();
    }
}