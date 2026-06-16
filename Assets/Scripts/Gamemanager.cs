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
    private const int TOTAL_LEVELS = 3; // Sala de Servidores, Red de Datos, Base de Datos Central

    // -------------------------------------------------------
    // FLUJO DE NIVELES
    // -------------------------------------------------------

    /// <summary>
    /// Llamar cuando el jugador completa exitosamente un minijuego.
    /// </summary>
    public void OnLevelComplete()
    {
        // PENDIENTE: ¿se muestra alguna animación/cinemática entre niveles antes de cargar?
        // if (currentLevel < TOTAL_LEVELS)
        // {
        //     currentLevel++;
        //     LoadNextLevel();
        // }
        // else
        // {
        //     TriggerVictory();
        // }
    }

    /// <summary>
    /// Carga la escena del siguiente nivel.
    /// PENDIENTE: confirmar nombres exactos de escenas con el equipo.
    /// </summary>
    private void LoadNextLevel()
    {
        // SceneManager.LoadScene("Level" + currentLevel);
    }

    // -------------------------------------------------------
    // VICTORIA Y DERROTA
    // -------------------------------------------------------

    /// <summary>
    /// Se activa al completar los 3 niveles.
    /// PENDIENTE: definir si hay animación de victoria antes de cambiar escena.
    /// </summary>
    public void TriggerVictory()
    {
        // SceneManager.LoadScene("VictoryScreen");
    }

    /// <summary>
    /// Se activa cuando LifeManager reporta 0 vidas.
    /// Debe mostrar la animación de Pantalla Azul de la Muerte (BSOD).
    /// PENDIENTE: definir si GameManager llama directamente a la escena
    /// o si primero notifica a UIManager para reproducir la animación BSOD.
    /// </summary>
    public void TriggerGameOver()
    {
        // UIManager.Instance.ShowBSOD(); // Mostrar animación primero (si aplica)
        // SceneManager.LoadScene("GameOverScreen");
    }

    // -------------------------------------------------------
    // UTILIDADES
    // -------------------------------------------------------
    public int GetCurrentLevel() => currentLevel;
}