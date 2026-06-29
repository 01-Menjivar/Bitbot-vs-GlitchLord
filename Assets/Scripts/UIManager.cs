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

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Level2" || scene.name == "Level3")
        {
            RebindHUDReferences();
        }
    }

    public void RebindHUDReferences()
    {
        Debug.Log("[UIManager] Re-vinculando referencias del HUD para la escena activa...");
        
        // Buscar la barra de tiempo
        GameObject timerBarObj = GameObject.Find("HUD_Timer");
        if (timerBarObj != null)
        {
            timerBar = timerBarObj.GetComponent<Slider>();
        }

        // Buscar el texto del cronómetro
        GameObject timerTextObj = GameObject.Find("TimerText");
        if (timerTextObj != null)
        {
            timerText = timerTextObj.GetComponent<Text>();
        }

        // Buscar el texto de la puntuación
        GameObject scoreTextObj = GameObject.Find("ScoreText");
        if (scoreTextObj != null)
        {
            scoreText = scoreTextObj.GetComponent<Text>();
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

    // -------------------------------------------------------
    // FEEDBACK VISUAL DEL TEMPORIZADOR
    // -------------------------------------------------------

    [Header("Configuración de Feedback del Timer")]
    [SerializeField] private Color timeGainedColor = new Color(0.2f, 1f, 0.4f);  // Verde neón
    [SerializeField] private Color timeLostColor = new Color(1f, 0.2f, 0.2f);    // Rojo neón
    [SerializeField] private float timerPulseDuration = 0.5f;
    [SerializeField] private float timerPunchScale = 1.25f;

    private Coroutine timerFeedbackCoroutine;
    private Color originalTimerTextColor = Color.white;
    private Vector3 originalTimerTextScale = Vector3.one;
    private Text lastTimerText;

    /// <summary>
    /// Activa el feedback visual (color y escala) en el texto del temporizador.
    /// </summary>
    /// <param name="isPositive">true si se ganó tiempo, false si se perdió tiempo.</param>
    public void TriggerTimerFeedback(bool isPositive)
    {
        if (timerText == null) return;

        // Si el objeto Text ha cambiado o es el primero, guardamos sus valores originales
        if (timerText != lastTimerText)
        {
            originalTimerTextColor = timerText.color;
            originalTimerTextScale = timerText.transform.localScale;
            lastTimerText = timerText;
        }

        if (timerFeedbackCoroutine != null)
        {
            StopCoroutine(timerFeedbackCoroutine);
        }

        timerFeedbackCoroutine = StartCoroutine(TimerFeedbackCoroutine(isPositive));
    }

    private System.Collections.IEnumerator TimerFeedbackCoroutine(bool isPositive)
    {
        Color flashColor = isPositive ? timeGainedColor : timeLostColor;
        float elapsed = 0f;

        // Establecer color de inicio de flash y escala original
        timerText.color = flashColor;
        timerText.transform.localScale = originalTimerTextScale;

        while (elapsed < timerPulseDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / timerPulseDuration;

            // Curva de escala tipo "punch" (crece rápido al inicio y regresa lento)
            float scaleMultiplier = 1f;
            if (t < 0.3f)
            {
                scaleMultiplier = Mathf.Lerp(1f, timerPunchScale, t / 0.3f);
            }
            else
            {
                scaleMultiplier = Mathf.Lerp(timerPunchScale, 1f, (t - 0.3f) / 0.7f);
            }

            timerText.transform.localScale = originalTimerTextScale * scaleMultiplier;

            // Interpolación de color de regreso al color original
            timerText.color = Color.Lerp(flashColor, originalTimerTextColor, t);

            yield return null;
        }

        // Asegurar restablecimiento final
        timerText.color = originalTimerTextColor;
        timerText.transform.localScale = originalTimerTextScale;
        timerFeedbackCoroutine = null;
    }
}