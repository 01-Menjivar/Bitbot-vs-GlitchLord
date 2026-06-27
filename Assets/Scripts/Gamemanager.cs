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
            transform.SetParent(null); // Desvincular de _Managers para permitir DontDestroyOnLoad
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
    private const int TOTAL_LEVELS = 2; // Nivel 1: File Catcher (Level2), Nivel 2: Debug Smash (Level3)

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
        StartCoroutine(GameOverRoutine());
    }

    private System.Collections.IEnumerator GameOverRoutine()
    {
        // 1. Mostrar pantalla azul del HUD en el UIManager si está asignada
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowBSOD();
        }

        // 2. Activar el fondo local de derrota en la escena si existe
        GameObject failBg = GameObject.Find("FailBackground");
        if (failBg == null) failBg = GameObject.Find("level 2 fail");
        if (failBg != null)
        {
            failBg.SetActive(true);
            SpriteRenderer sr = failBg.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = 100; // Asegurar que esté al frente
            }
        }

        // 3. Esperar 2 segundos para dar retroalimentación al jugador
        yield return new WaitForSeconds(2.0f);

        // 4. Cargar la escena de GameOver
        SceneManager.LoadScene("GameOverScreen");
    }

    // -------------------------------------------------------
    // UTILIDADES DE NAVEGACIÓN Y REINICIO
    // -------------------------------------------------------
    
    public int GetCurrentLevel() => currentLevel;

    /// <summary>
    /// Reinicia la partida desde el primer nivel jugable (Level2) con vidas completas.
    /// </summary>
    public void RestartGame()
    {
        ResetGame();
        SceneManager.LoadScene("Level2");
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