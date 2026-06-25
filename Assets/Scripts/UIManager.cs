using UnityEngine;
using UnityEngine.UI; // Descomentar cuando se conecten elementos del Canvas

/// <summary>
/// UIManager — Gestiona todos los elementos visuales de la interfaz.
/// Maneja el HUD durante los minijuegos y las pantallas de victoria/derrota.
/// Los assets visuales (iconos, barras, pantallas) los define Gabriela en Assets/Art/UI.
/// </summary>
public class UIManager : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // REFERENCIAS DE UI
    // -------------------------------------------------------

    // PENDIENTE: Gabriela entrega los assets visuales en Assets/Art/UI
    // Una vez disponibles, asignar desde el Inspector de Unity:

    // HUD — Barra de tiempo
    [SerializeField] private Slider timerBar;

    // HUD — Iconos de vidas (núcleos de procesador)
    [SerializeField] private Image[] lifeIcons; // Array de 3 iconos

    // Pantallas
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen; // Pantalla Azul de la Muerte (BSOD)

    // PENDIENTE: definir si el BSOD es una animación, un panel estático o una escena aparte.

    // -------------------------------------------------------
    // HUD — TIMER
    // -------------------------------------------------------

    /// <summary>
    /// Actualiza la barra de progreso del cronómetro.
    /// Llamar desde TimerController en cada frame.
    /// </summary>
    public void UpdateTimerBar(float currentTime, float maxTime)
    {
        if (timerBar != null && maxTime > 0f)
        {
            timerBar.value = currentTime / maxTime;
        }
    }

    /// <summary>
    /// Feedback visual de alerta cuando el tiempo está por acabarse.
    /// PENDIENTE: definir con el equipo qué animación o efecto se usa.
    /// </summary>
    public void ShowTimerWarning()
    {
        // Ejemplo: hacer parpadear la barra o cambiarla a rojo
    }

    // -------------------------------------------------------
    // HUD — VIDAS
    // -------------------------------------------------------

    /// <summary>
    /// Actualiza los iconos de vidas en pantalla.
    /// Llamar desde LifeManager al perder una vida.
    /// </summary>
    public void UpdateLivesHUD(int currentLives)
    {
        if (lifeIcons != null)
        {
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (lifeIcons[i] != null)
                {
                    lifeIcons[i].enabled = (i < currentLives);
                }
            }
        }
    }

    // -------------------------------------------------------
    // PANTALLAS DE RESULTADO
    // -------------------------------------------------------

    /// <summary>
    /// Muestra la pantalla de victoria al completar todos los niveles.
    /// Llamar desde GameManager.TriggerVictory().
    /// </summary>
    public void ShowVictoryScreen()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
        }
    }

    /// <summary>
    /// Muestra la Pantalla Azul de la Muerte (BSOD) al quedarse sin vidas.
    /// Llamar desde GameManager.TriggerGameOver().
    /// PENDIENTE: coordinar con Raúl la animación del BSOD (está en Assets/Art/Animations).
    /// </summary>
    public void ShowBSOD()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    // -------------------------------------------------------
    // INSTRUCCIONES DE MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Muestra el mensaje de instrucción breve antes de iniciar un minijuego.
    /// PENDIENTE: definir el formato (panel de texto, animación, tiempo de display).
    /// </summary>
    public void ShowMinigameInstruction(string instruction)
    {
        // instructionPanel.SetActive(true);
        // instructionText.text = instruction;
        // PENDIENTE: ¿cuántos segundos se muestra antes de iniciar el timer?
    }
}