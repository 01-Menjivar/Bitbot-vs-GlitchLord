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
            transform.SetParent(null); // Desvincular de _Managers para permitir DontDestroyOnLoad
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
    [SerializeField] private Text timerText;
    [SerializeField] private Sprite barSpriteFull;
    [SerializeField] private Sprite barSpriteWarning;
    [SerializeField] private Sprite barSpriteCritical;

    // HUD — Iconos de vidas (núcleos de procesador)
    [SerializeField] private Image[] lifeIcons; // Array de 3 iconos
    [SerializeField] private Sprite coreActiveSprite;
    [SerializeField] private Sprite coreOfflineSprite;

    // HUD — Puntuación (Score)
    [Header("HUD — Score")]
    [Tooltip("Imagen de marco del score (usa score.png como sprite)")]
    [SerializeField] private Image scoreFrameImage;
    [Tooltip("Texto numérico del puntaje dentro del marco")]
    [SerializeField] private Text scoreText;

    // Pantallas
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen; // Pantalla Azul de la Muerte (BSOD)

    // PENDIENTE: definir si el BSOD es una animación, un panel estático o una escena aparte.

    // -------------------------------------------------------
    // HUD — TIMER
    // -------------------------------------------------------

    /// <summary>
    /// Actualiza la barra de progreso del cronómetro y el texto del tiempo.
    /// Llamar desde TimerController en cada frame.
    /// </summary>
    public void UpdateTimerBar(float currentTime, float maxTime)
    {
        // 1. Actualizar barra de progreso si está configurada
        if (timerBar != null && maxTime > 0f)
        {
            float fillRatio = currentTime / maxTime;
            timerBar.value = fillRatio;

            // Cambiar el sprite de relleno según el porcentaje de tiempo restante
            if (timerBar.fillRect != null)
            {
                Image fillImage = timerBar.fillRect.GetComponent<Image>();
                if (fillImage != null)
                {
                    if (fillRatio <= 0.25f && barSpriteCritical != null)
                    {
                        fillImage.sprite = barSpriteCritical;
                    }
                    else if (fillRatio <= 0.5f && barSpriteWarning != null)
                    {
                        fillImage.sprite = barSpriteWarning;
                    }
                    else if (barSpriteFull != null)
                    {
                        fillImage.sprite = barSpriteFull;
                    }
                }
            }
        }

        // 2. Actualizar el cronómetro numérico (sobreescritura del HUD_Timer estático)
        if (timerText != null)
        {
            int seconds = Mathf.Max(0, Mathf.CeilToInt(currentTime));
            timerText.text = $"00:{seconds:D2}";
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
                    if (i < currentLives)
                    {
                        if (coreActiveSprite != null)
                        {
                            lifeIcons[i].sprite = coreActiveSprite;
                            lifeIcons[i].enabled = true;
                        }
                        else
                        {
                            lifeIcons[i].enabled = true;
                        }
                    }
                    else
                    {
                        if (coreOfflineSprite != null)
                        {
                            lifeIcons[i].sprite = coreOfflineSprite;
                            lifeIcons[i].enabled = true;
                        }
                        else
                        {
                            lifeIcons[i].enabled = false;
                        }
                    }
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
    // HUD — PUNTUACIÓN (SCORE)
    // -------------------------------------------------------

    /// <summary>
    /// Actualiza el texto del puntaje en el HUD.
    /// Llamar desde ScoreManager cada vez que cambie la puntuación.
    /// </summary>
    public void UpdateScoreHUD(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString("N0"); // Formato con separador de miles
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