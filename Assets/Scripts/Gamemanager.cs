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
            if (transform.parent != null)
            {
                transform.SetParent(null); // Desvincular de _Managers para permitir DontDestroyOnLoad
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private int currentLevel = 1;
    private const int TOTAL_LEVELS = 2; // Nivel 1: File Catcher (Level2), Nivel 2: Debug Smash (Level3)
    private string failedSceneName = "Level2";

    // -------------------------------------------------------
    // FLUJO DE NIVELES
    // -------------------------------------------------------

    public void OnLevelComplete()
    {
        // Al completar con éxito cualquiera de los minijuegos, cargamos la pantalla de victoria
        // para permitir volver al menú principal/selección de niveles.
        TriggerVictory();
    }

    /// <summary>
    /// Carga la escena del siguiente nivel.
    /// Mapea el nivel lógico al nombre real de la escena (Nivel 1 -> Level2, Nivel 2 -> Level3).
    /// </summary>
    private void LoadNextLevel()
    {
        SceneManager.LoadScene("Level" + (currentLevel + 1));
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
    /// </summary>
    public void TriggerGameOver()
    {
        failedSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOverScreen");
    }

    // -------------------------------------------------------
    // UTILIDADES DE NAVEGACIÓN Y REINICIO
    // -------------------------------------------------------
    
    public int GetCurrentLevel() => currentLevel;

    public void RestartGame()
    {
        if (LifeManager.Instance != null)
        {
            LifeManager.Instance.ResetLives();
        }

        // Ajustar el nivel lógico según la escena que estamos reintentando
        if (failedSceneName == "Level3")
        {
            currentLevel = 2;
        }
        else
        {
            currentLevel = 1;
        }

        SceneManager.LoadScene(failedSceneName);
    }

    /// <summary>
    /// Vuelve al menú principal y resetea el nivel lógico.
    /// </summary>
    public void LoadMainMenu()
    {
        currentLevel = 1;
        SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// Inicializa el estado del juego (vidas y nivel lógico) al empezar partida.
    /// </summary>
    public void ResetGame()
    {
        currentLevel = 1;
        if (LifeManager.Instance != null)
        {
            LifeManager.Instance.ResetLives();
        }
    }
}