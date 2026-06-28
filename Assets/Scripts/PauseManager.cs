using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// PauseManager — Inyecta y gestiona de forma procedural el botón de pausa y el panel de pausa
/// en los minijuegos (Nivel 2 y Nivel 3) de manera persistente al transicionar entre escenas.
/// Asegura la existencia del EventSystem en la escena y del componente GraphicRaycaster en el Canvas
/// para garantizar el registro de clics de UI en todo momento.
/// </summary>
public class PauseManager : MonoBehaviour
{
    private GameObject pauseButtonObj;
    private GameObject pausePanelObj;
    private bool isPaused = false;
    private bool shouldCreatePauseButton = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        // Evitar duplicados del gestor persistente
        if (GameObject.Find("PauseManagerHost") != null) return;

        GameObject host = new GameObject("PauseManagerHost");
        DontDestroyOnLoad(host);
        host.AddComponent<PauseManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destruir físicamente cualquier elemento huérfano persistente de la escena previa
        GameObject oldButton = GameObject.Find("Button_Pause");
        if (oldButton != null) Destroy(oldButton);

        GameObject oldPanel = GameObject.Find("PausePanel");
        if (oldPanel != null) Destroy(oldPanel);

        // Resetear estado al transicionar de escena
        isPaused = false;
        Time.timeScale = 1f;
        pauseButtonObj = null;
        pausePanelObj = null;

        // Activar la bandera de creación si entramos a un nivel jugable
        // Se excluyen Level3 y Level2 porque ahora cuentan con su propia interfaz de pausa narrativa
        shouldCreatePauseButton = false;
    }

    private void Update()
    {
        // Solo permitir pausar/crear si estamos en una escena jugable válida que use el sistema antiguo
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "SceneSelection") return;

        // Garantizar que exista un EventSystem en la escena para que la UI responda a los clics del mouse
        EnsureEventSystem();

        // Intentar inicializar el botón de pausa si está planificado (reintenta cada frame hasta encontrar el Canvas)
        if (shouldCreatePauseButton && pauseButtonObj == null)
        {
            Canvas canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
            if (canvas == null) canvas = GameObject.Find("InfectionCanvas")?.GetComponent<Canvas>();
            if (canvas == null) canvas = FindObjectOfType<Canvas>();

            if (canvas != null)
            {
                // Asegurar que el Canvas tenga un GraphicRaycaster para recibir clics
                // (Necesario para Level3 ya que InfectionCanvas se crea por código sin este componente)
                if (canvas.GetComponent<GraphicRaycaster>() == null)
                {
                    Debug.Log($"[PauseManager] Agregando GraphicRaycaster faltante en {canvas.gameObject.name}...");
                    canvas.gameObject.AddComponent<GraphicRaycaster>();
                }

                Debug.Log($"[PauseManager] Canvas '{canvas.gameObject.name}' listo. Inyectando botón de pausa...");
                CreatePauseButton(canvas);
                shouldCreatePauseButton = false; // Creación completada
            }
        }

        // Detectar si el jugador presiona la tecla Escape o la tecla P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            Debug.Log("[PauseManager] No se detectó EventSystem en la escena. Creando uno de forma automática...");
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    private void CreatePauseButton(Canvas canvas)
    {
        // 1. Crear el objeto del botón de pausa
        pauseButtonObj = new GameObject("Button_Pause", typeof(RectTransform));
        pauseButtonObj.transform.SetParent(canvas.transform, false);

        RectTransform rect = pauseButtonObj.GetComponent<RectTransform>();
        // Anclar en la esquina superior derecha
        rect.anchorMin = new Vector2(1f, 1f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.pivot = new Vector2(1f, 1f);
        rect.anchoredPosition = new Vector2(-30f, -30f); // 30px de margen superior/derecho
        rect.sizeDelta = new Vector2(100f, 100f); // Botón de 100x100 píxeles

        // 2. Añadir imagen de fondo (usando Button_Settings como engranaje retro)
        Image img = pauseButtonObj.AddComponent<Image>();
        Sprite settingsSprite = Resources.Load<Sprite>("UI/Buttons/Button_Settings");
        if (settingsSprite != null)
        {
            img.sprite = settingsSprite;
            img.color = Color.white;
        }
        else
        {
            img.color = new Color(0.2f, 0.2f, 0.25f, 0.8f);
        }

        // 3. Añadir texto "II" en el centro para denotar Pausa
        GameObject textObj = new GameObject("Text_Icon", typeof(RectTransform));
        textObj.transform.SetParent(pauseButtonObj.transform, false);
        
        RectTransform txtRect = textObj.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.sizeDelta = Vector2.zero;

        Text txt = textObj.AddComponent<Text>();
        txt.text = "II";
        txt.alignment = TextAnchor.MiddleCenter;
        txt.fontSize = 32;
        txt.color = Color.white;

        Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
        if (retroFont != null) txt.font = retroFont;

        // 4. Añadir componente Button y listener
        Button btn = pauseButtonObj.AddComponent<Button>();
        btn.onClick.AddListener(TogglePause);

        // Configurar transición retro
        btn.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        colors.selectedColor = Color.white;
        btn.colors = colors;
    }

    public void TogglePause()
    {
        // Solo permitir pausar con este manager en las escenas jugables antiguas
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "SceneSelection") return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Debug.Log("[PauseManager] Juego pausado.");
            Time.timeScale = 0f;
            ShowPausePanel();
            
            // Ocultar temporalmente el botón de pausa flotante
            if (pauseButtonObj != null) pauseButtonObj.SetActive(false);
        }
        else
        {
            Debug.Log("[PauseManager] Juego reanudado.");
            Time.timeScale = 1f;
            HidePausePanel();

            // Mostrar de nuevo el botón de pausa flotante
            if (pauseButtonObj != null) pauseButtonObj.SetActive(true);
        }
    }

    private void ShowPausePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        // 1. Crear el Panel de fondo semi-transparente
        pausePanelObj = new GameObject("PausePanel", typeof(RectTransform));
        pausePanelObj.transform.SetParent(canvas.transform, false);
        // Colocar en el frente para que se renderice por encima de otros UI
        pausePanelObj.transform.SetAsLastSibling();

        RectTransform panelRect = pausePanelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        Image panelImg = pausePanelObj.AddComponent<Image>();
        panelImg.color = new Color(0f, 0f, 0f, 0.82f); // 82% negro opaco

        // 2. Crear título "PAUSA"
        GameObject titleObj = new GameObject("Text_Title", typeof(RectTransform));
        titleObj.transform.SetParent(pausePanelObj.transform, false);
        
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = new Vector2(0f, 220f);
        titleRect.sizeDelta = new Vector2(600f, 100f);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "PAUSA";
        titleText.fontSize = 54;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.2f, 0.9f, 1f, 1f); // Tonalidad cian neón

        Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
        if (retroFont != null) titleText.font = retroFont;

        // 3. Crear botón de Reanudar (Button_Continue)
        CreateMenuButton(pausePanelObj.transform, "Button_Resume", "UI/Buttons/Button_Continue", new Vector2(0f, 20f), () =>
        {
            TogglePause();
        });

        // 4. Crear botón de Salir al Menú (Button_Exit)
        CreateMenuButton(pausePanelObj.transform, "Button_MainMenu", "UI/Buttons/Button_Exit", new Vector2(0f, -140f), () =>
        {
            // Asegurar restaurar el TimeScale antes de transicionar de escena
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenuScene");
        });
    }

    private void CreateMenuButton(Transform parent, string name, string resourcePath, Vector2 position, System.Action onClickAction)
    {
        GameObject btnObj = new GameObject(name, typeof(RectTransform));
        btnObj.transform.SetParent(parent, false);

        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(620f, 200f); // Tamaño proporcional retro estándar

        Image img = btnObj.AddComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }
        else
        {
            img.color = new Color(0.25f, 0.25f, 0.3f, 1f);
        }

        Button btn = btnObj.AddComponent<Button>();
        btn.onClick.AddListener(() => onClickAction());

        btn.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        colors.selectedColor = Color.white;
        btn.colors = colors;
    }

    private void HidePausePanel()
    {
        if (pausePanelObj != null)
        {
            Destroy(pausePanelObj);
        }
    }

    private void OnDestroy()
    {
        // Limpieza de seguridad al salir de la escena
        if (isPaused)
        {
            Time.timeScale = 1f;
        }
    }
}
