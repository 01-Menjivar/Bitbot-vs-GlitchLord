using UnityEngine;

/// <summary>
/// TimerController — Maneja el cronómetro de cada minijuego.
/// Cada minijuego tiene su propio tiempo límite.
/// Al llegar a 0 notifica a LifeManager para restar una vida.
/// </summary>
public class TimerController : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static TimerController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private float timer;           // Tiempo restante actual
    private float maxTime;         // Tiempo inicial del minijuego
    private bool isRunning = false;

    // PENDIENTE: definir los tiempos por nivel con el equipo
    // Ejemplo orientativo:
    // Nivel 1 (Reconnect)   → 30 segundos
    // Nivel 2 (File Catcher)→ 45 segundos (tiempo de supervivencia)
    // Nivel 3 (Debug Smash) → 30 segundos

    // PENDIENTE: definir si la dificultad progresiva reduce el tiempo
    // entre minijuegos del mismo nivel o entre niveles distintos.

    // -------------------------------------------------------
    // CONTROL DEL TIMER
    // -------------------------------------------------------

    /// <summary>
    /// Iniciar el cronómetro con un tiempo dado.
    /// Llamar al inicio de cada minijuego.
    /// </summary>
    public void StartTimer(float time)
    {
        maxTime = time;
        timer = time;
        isRunning = true;
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimerBar(timer, maxTime); // Actualizar barra de progreso
        }

        if (timer <= 0f)
        {
            timer = 0f;
            isRunning = false;
            OnTimerExpired();
        }
    }

    /// <summary>
    /// Se ejecuta cuando el tiempo llega a 0.
    /// Resta una vida al jugador.
    /// </summary>
    private void OnTimerExpired()
    {
        if (LifeManager.Instance != null)
        {
            LifeManager.Instance.LoseLife();
        }
    }

    /// <summary>
    /// Detener el cronómetro manualmente.
    /// Llamar cuando el jugador completa el minijuego antes de que acabe el tiempo.
    /// </summary>
    public void StopTimer()
    {
        isRunning = false;
    }

    // -------------------------------------------------------
    // MODIFICACIÓN DE TIEMPO EN VIVO
    // -------------------------------------------------------

    /// <summary>
    /// Agrega tiempo extra al cronómetro (recompensa por atrapar archivos válidos).
    /// El tiempo no puede superar el tiempo máximo original del nivel.
    /// </summary>
    public void AddTime(float seconds)
    {
        if (!isRunning) return;

        timer += seconds;
        // Limitar al tiempo máximo para evitar acumulación excesiva
        timer = Mathf.Min(timer, maxTime);
    }

    /// <summary>
    /// Resta tiempo del cronómetro (penalización por virus/archivos corruptos).
    /// Si el tiempo resultante es menor o igual a 0, se activa la expiración.
    /// </summary>
    public void SubtractTime(float seconds)
    {
        if (!isRunning) return;

        timer -= seconds;

        if (timer <= 0f)
        {
            timer = 0f;
            isRunning = false;
            OnTimerExpired();
        }
    }

    // -------------------------------------------------------
    // UTILIDADES
    // -------------------------------------------------------
    public float GetTimer() => timer;
    public float GetMaxTime() => maxTime;
}