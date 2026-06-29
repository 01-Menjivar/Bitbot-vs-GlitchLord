using UnityEngine;

/// <summary>
/// LifeManager — Gestiona el sistema de vidas del jugador.
/// Las vidas se representan como "núcleos de procesador" en el HUD.
/// Notifica a GameManager cuando el jugador se queda sin vidas.
/// </summary>
public class LifeManager : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static LifeManager Instance;



    private void Awake()
    {
        if (Instance == null)
        {
            if (transform.parent != null)
            {
                transform.SetParent(null); // Desvincular de _Managers para permitir DontDestroyOnLoad
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private int lives = 3; // Vidas iniciales (núcleos de procesador)
    private const int MAX_LIVES = 3;

    // -------------------------------------------------------
    // PERDER VIDA
    // -------------------------------------------------------

    /// <summary>
    /// Llamar cuando:
    /// - El cronómetro llega a 0 sin completar el minijuego.
    /// - El jugador comete un error crítico (ej. atrapar un virus en File Catcher).
    /// </summary>
    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateLivesHUD(lives);
            }
        }

        if (lives <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TriggerGameOver();
            }
        }
    }

    // -------------------------------------------------------
    // UTILIDADES
    // -------------------------------------------------------
    public int GetLives() => lives;

    /// <summary>
    /// Reinicia las vidas al comenzar una nueva partida.
    /// PENDIENTE: definir si se llama desde GameManager o desde un menú de reinicio.
    /// </summary>
    public void ResetLives()
    {
        lives = MAX_LIVES;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateLivesHUD(lives);
        }
    }
}