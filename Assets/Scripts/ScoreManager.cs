using UnityEngine;

/// <summary>
/// ScoreManager — Gestor global de puntuación del juego.
/// Mantiene el puntaje acumulado del jugador durante toda la partida
/// y notifica al UIManager para actualizar el HUD en tiempo real.
/// Arquitectura Singleton, persistente entre escenas.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            transform.SetParent(null); // Desvincular para permitir DontDestroyOnLoad
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private int currentScore = 0;

    // -------------------------------------------------------
    // CONFIGURACIÓN DE PUNTOS (Valores por defecto, ajustables)
    // -------------------------------------------------------
    [Header("Configuración de Puntuación")]
    [Tooltip("Puntos otorgados al atrapar un archivo válido (azul/verde)")]
    [SerializeField] private int pointsPerValidFile = 100;

    [Tooltip("Puntos restados al ser golpeado por un virus")]
    [SerializeField] private int penaltyPerVirus = 50;

    [Header("Configuración de Tiempo")]
    [Tooltip("Segundos de tiempo extra al atrapar un archivo válido")]
    [SerializeField] private float timeRewardPerValidFile = 2f;

    [Tooltip("Segundos de tiempo restados al ser golpeado por un virus")]
    [SerializeField] private float timePenaltyPerVirus = 3f;

    // -------------------------------------------------------
    // MÉTODOS PÚBLICOS
    // -------------------------------------------------------

    /// <summary>
    /// Registra la captura de un archivo válido (azul/verde).
    /// Suma puntuación y agrega tiempo extra al cronómetro.
    /// </summary>
    public void OnValidFileCaught()
    {
        AddScore(pointsPerValidFile);

        // Agregar tiempo extra al cronómetro
        if (TimerController.Instance != null)
        {
            TimerController.Instance.AddTime(timeRewardPerValidFile);
        }

        Debug.Log($"[ScoreManager] +{pointsPerValidFile} puntos, +{timeRewardPerValidFile}s tiempo. Total: {currentScore}");
    }

    /// <summary>
    /// Registra el impacto de un virus/archivo corrupto.
    /// Resta puntuación y reduce el tiempo del cronómetro.
    /// </summary>
    public void OnVirusHit()
    {
        SubtractScore(penaltyPerVirus);

        // Restar tiempo del cronómetro
        if (TimerController.Instance != null)
        {
            TimerController.Instance.SubtractTime(timePenaltyPerVirus);
        }

        Debug.LogWarning($"[ScoreManager] -{penaltyPerVirus} puntos, -{timePenaltyPerVirus}s tiempo. Total: {currentScore}");
    }

    /// <summary>
    /// Suma una cantidad de puntos al puntaje actual.
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreHUD();
    }

    /// <summary>
    /// Resta una cantidad de puntos al puntaje actual.
    /// La puntuación no puede bajar de 0.
    /// </summary>
    public void SubtractScore(int points)
    {
        currentScore = Mathf.Max(0, currentScore - points);
        UpdateScoreHUD();
    }

    /// <summary>
    /// Reinicia la puntuación a cero.
    /// Llamar al inicio de una nueva partida.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreHUD();
    }

    /// <summary>
    /// Retorna la puntuación actual del jugador.
    /// </summary>
    public int GetScore() => currentScore;

    // -------------------------------------------------------
    // MÉTODOS PRIVADOS
    // -------------------------------------------------------

    /// <summary>
    /// Notifica al UIManager para que actualice el texto del score en el HUD.
    /// </summary>
    private void UpdateScoreHUD()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateScoreHUD(currentScore);
        }
    }
}
