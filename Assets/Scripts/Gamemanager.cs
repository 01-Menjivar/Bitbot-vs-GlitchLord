using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager — Controlador central del juego.
/// Responsable de: flujo entre niveles, victoria y derrota.
/// Implementado como Singleton para acceso global desde cualquier script.
/// </summary>
public class GameManager : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private int currentLevel = 1;
    private const int TOTAL_LEVELS = 2; // Red de Datos, Base de Datos Central

    // -------------------------------------------------------
    // FLUJO DE NIVELES
    // -------------------------------------------------------

    /// <summary>
    /// Llamar cuando el jugador completa exitosamente un minijuego.
    /// </summary>
    public void OnLevelComplete()
    {
        if (currentLevel < TOTAL_LEVELS)
        {
            currentLevel++;
            LoadNextLevel();
        }
        else
        {
            TriggerVictory();
        }
    }

    /// <summary>
    /// Carga la escena del siguiente nivel.
    /// </summary>
    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + currentLevel);
    }

    // -------------------------------------------------------
    // VICTORIA Y DERROTA
    // -------------------------------------------------------

    /// <summary>
    /// Se activa al completar los niveles del juego.
    /// </summary>
    public void TriggerVictory()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    /// <summary>
    /// Se activa cuando LifeManager reporta 0 vidas.
    /// Debe mostrar la animación de Pantalla Azul de la Muerte (BSOD).
    /// </summary>
    public void TriggerGameOver()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowBSOD();
        }
        SceneManager.LoadScene("GameOverScreen");
    }

    // -------------------------------------------------------
    // UTILIDADES
    // -------------------------------------------------------
    public int GetCurrentLevel() => currentLevel;
}