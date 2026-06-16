using UnityEngine;

/// <summary>
/// FileCatcherController — Controla el minijuego 2: "File Catcher — Atrapar Archivos, Evitar Virus".
/// Nivel 2: Red de Datos.
///
/// Mecánica base (según reporte):
/// - Archivos caen desde la parte superior de la pantalla.
/// - Bit-Bot se mueve horizontalmente (izquierda/derecha) en la parte inferior.
/// - Controles: teclas A/D o flechas izquierda/derecha.
/// - Archivos azules y verdes → recoger (suman progreso).
/// - Archivos violeta oscuro con ícono de Glitch-Lord → esquivar (restan una vida).
/// - El ritmo de caída se acelera conforme pasa el tiempo.
/// - PENDIENTE: definir condición de victoria (¿recoger X archivos? ¿sobrevivir el tiempo?).
/// </summary>
public class FileCatcherController : MonoBehaviour
{
    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private bool isMinigameActive = false;

    // Movimiento de Bit-Bot
    private float moveSpeed = 5f; // PENDIENTE: ajustar valor con el equipo
    // private float horizontalInput;

    // Progreso del minijuego
    // private int filesCaught = 0;
    // private int filesRequired; // PENDIENTE: Oscar define cuántos archivos hay que recoger

    // Dificultad progresiva
    // private float spawnInterval;     // Intervalo entre aparición de archivos
    // private float spawnSpeedIncrease; // Cuánto se acelera con el tiempo
    // PENDIENTE: definir valores iniciales y curva de aceleración con el equipo

    // -------------------------------------------------------
    // INICIO DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Inicializa el minijuego. Llamar desde GameManager al cargar el Nivel 2.
    /// </summary>
    public void StartMinigame()
    {
        isMinigameActive = true;
        // filesCaught = 0;

        // PENDIENTE: activar spawner de archivos y virus
        // PENDIENTE: posicionar a Bit-Bot en el centro inferior de la pantalla
        // TimerController.Instance.StartTimer(tiempoDefinidoPorEquipo);
        // UIManager.Instance.ShowMinigameInstruction("¡Atrapa los archivos y esquiva los virus!");
    }

    // -------------------------------------------------------
    // MOVIMIENTO DE BIT-BOT
    // -------------------------------------------------------

    private void Update()
    {
        if (!isMinigameActive) return;

        HandleMovement();
    }

    /// <summary>
    /// Maneja el input de movimiento horizontal de Bit-Bot.
    /// Controles: A/D o flechas izquierda/derecha (según Input Manager de Oscar).
    /// </summary>
    private void HandleMovement()
    {
        // horizontalInput = Input.GetAxisRaw("Horizontal"); // -1, 0 o 1
        // transform.Translate(Vector2.right * horizontalInput * moveSpeed * Time.deltaTime);

        // PENDIENTE: definir límites horizontales para que Bit-Bot no salga de pantalla.
        // PENDIENTE: confirmar controles exactos con el Input Manager de Oscar.
    }

    // -------------------------------------------------------
    // SPAWNER DE ARCHIVOS Y VIRUS
    // -------------------------------------------------------

    /// <summary>
    /// Genera archivos válidos y archivos corruptos (virus) desde la parte superior.
    /// PENDIENTE: Oscar define la proporción de archivos válidos vs virus.
    /// PENDIENTE: definir si el spawner es un script separado (FileSpawner.cs).
    /// </summary>
    private void SpawnFile()
    {
        // Elegir aleatoriamente entre archivo válido (azul/verde) o virus (violeta)
        // Instanciar el prefab en una posición X aleatoria en la parte superior
        // PENDIENTE: definir los prefabs de archivo válido y virus (los assets los hace Raúl)
    }

    // -------------------------------------------------------
    // COLISIONES
    // -------------------------------------------------------

    /// <summary>
    /// Se activa cuando Bit-Bot colisiona con un objeto que cae.
    /// PENDIENTE: definir si se usa OnTriggerEnter2D o un sistema de detección propio.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!isMinigameActive) return;

        // if (other.CompareTag("ValidFile"))
        // {
        //     filesCaught++;
        //     AudioManager.Instance.PlaySFX(sfxFileCaught);
        //     Destroy(other.gameObject);
        //     CheckMinigameComplete();
        // }

        // if (other.CompareTag("VirusFile"))
        // {
        //     AudioManager.Instance.PlaySFX(sfxVirusHit);
        //     LifeManager.Instance.LoseLife();
        //     Destroy(other.gameObject);
        // }

        // PENDIENTE: Oscar define los tags/identificadores de cada tipo de archivo.
    }

    // -------------------------------------------------------
    // DIFICULTAD PROGRESIVA
    // -------------------------------------------------------

    /// <summary>
    /// Aumenta la velocidad de caída de los archivos con el tiempo.
    /// PENDIENTE: definir la curva de dificultad con el equipo.
    /// </summary>
    private void IncreaseDifficulty()
    {
        // spawnInterval = Mathf.Max(spawnInterval - spawnSpeedIncrease, minSpawnInterval);
        // PENDIENTE: ¿también aumenta la velocidad de caída de cada archivo individualmente?
    }

    // -------------------------------------------------------
    // CONDICIÓN DE VICTORIA DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Verifica si se cumplió la condición de victoria.
    /// PENDIENTE: definir si se gana por recoger X archivos o por sobrevivir el tiempo completo.
    /// </summary>
    private void CheckMinigameComplete()
    {
        // Opción A: victoria por cantidad de archivos recogidos
        // if (filesCaught >= filesRequired)
        // {
        //     EndMinigame();
        // }

        // Opción B: victoria al sobrevivir hasta que el timer llegue a 0
        // (en ese caso OnTimerExpired debería llamar a OnLevelComplete en vez de LoseLife)
        // PENDIENTE: decidir cuál opción aplica con el equipo.
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
    /// PENDIENTE: el comportamiento depende de la condición de victoria elegida (Opción A o B).
    /// </summary>
    public void OnTimerExpired()
    {
        isMinigameActive = false;
        // LifeManager.Instance.LoseLife(); // Solo si la victoria es por cantidad (Opción A)
    }
}