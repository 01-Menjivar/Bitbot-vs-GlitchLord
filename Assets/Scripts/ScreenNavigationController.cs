using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ScreenNavigationController — Genera dinámicamente la interfaz para las escenas
/// de GameOver y Victoria, cargando los assets gráficos y tipografías retro desde Resources
/// y asignando las acciones correspondientes a los botones.
/// </summary>
public class ScreenNavigationController : MonoBehaviour
{
    // Registro automático antes de cargar la primera escena
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameObject host = new GameObject("ScreenNavigationControllerHost");
        DontDestroyOnLoad(host);
        host.AddComponent<ScreenNavigationController>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private string lastActiveScene = "Level2";

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOverScreen")
        {
            SetupGameOverScene();
        }
        else if (scene.name == "VictoryScreen")
        {
            SetupVictoryScene();
        }
        else if (scene.name == "Level2" || scene.name == "Level3")
        {
            lastActiveScene = scene.name;
        }
    }

    private void SetupGameOverScene()
    {
        Debug.Log("[SNC] Inicializando interfaz de GameOverScreen...");
        Canvas canvas = GetOrCreateCanvas();
        ClearCanvas(canvas);

        // 1. Fondo de Pantalla Azul (BSOD)
        CreateBackground(canvas, "UI/Screens/GameOverScreen");

        // 2. Cargar sprites de botones
        Sprite retrySprite = Resources.Load<Sprite>("UI/Buttons/Button_Retry");
        Sprite exitSprite = Resources.Load<Sprite>("UI/Buttons/Button_Exit");

        // 3. Botón "Reintentar" (Lado izquierdo)
        CreateButton(canvas, "Reintentar", retrySprite, new Vector2(-300, -320), () =>
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestartGame();
            }
            else
            {
                SceneManager.LoadScene(lastActiveScene);
            }
        });

        // 4. Botón "Menú Principal" (Lado derecho)
        CreateButton(canvas, "Menú Principal", exitSprite, new Vector2(300, -320), () =>
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadMainMenu();
            }
            else
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        });
    }

    private void SetupVictoryScene()
    {
        Debug.Log("[SNC] Inicializando interfaz de VictoryScreen...");
        Canvas canvas = GetOrCreateCanvas();
        ClearCanvas(canvas);

        // 1. Fondo de Victoria
        CreateBackground(canvas, "UI/Screens/VictoryScreen");

        // 2. Cargar sprite de botón
        Sprite exitSprite = Resources.Load<Sprite>("UI/Buttons/Button_Exit");

        // 3. Botón "Menú Principal" (Centrado abajo)
        CreateButton(canvas, "Menú Principal", exitSprite, new Vector2(0, -320), () =>
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadMainMenu();
            }
            else
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        });
    }

    private Canvas GetOrCreateCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        return canvas;
    }

    private void ClearCanvas(Canvas canvas)
    {
        // Destruir los hijos del canvas para asegurar que no haya duplicados ni basura de la plantilla
        foreach (Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreateBackground(Canvas canvas, string resourcePath)
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        
        Image bgImage = bgObj.AddComponent<Image>();
        Sprite bgSprite = Resources.Load<Sprite>(resourcePath);
        
        if (bgSprite != null)
        {
            bgImage.sprite = bgSprite;
            bgImage.preserveAspect = true;
        }
        else
        {
            Debug.LogWarning($"[SNC] No se pudo cargar el sprite del fondo en '{resourcePath}'. Usando fondo negro de respaldo.");
            bgImage.color = new Color(0.05f, 0.05f, 0.08f, 1f);
        }

        // Estirar para llenar la pantalla
        RectTransform rect = bgObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    private void CreateButton(Canvas canvas, string labelText, Sprite buttonSprite, Vector2 position, System.Action onClickAction)
    {
        GameObject btnObj = new GameObject(labelText + "_Button");
        btnObj.transform.SetParent(canvas.transform, false);

        Image img = btnObj.AddComponent<Image>();
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        
        if (buttonSprite != null)
        {
            img.sprite = buttonSprite;
            rect.sizeDelta = new Vector2(510, 165); // Aumentado un 50% (340x110 -> 510x165)
        }
        else
        {
            // Fallback si no hay sprite asignado
            img.color = new Color(0.12f, 0.12f, 0.18f, 0.95f);
            rect.sizeDelta = new Vector2(450, 120);
        }

        Button btn = btnObj.AddComponent<Button>();
        btn.onClick.AddListener(() => onClickAction());

        // Efectos de transición en el botón (Color Tint)
        btn.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f); // Oscurecer un poco al pasar mouse
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        colors.selectedColor = Color.white;
        btn.colors = colors;

        rect.anchoredPosition = position;

        // Si no hay sprite asignado o si queremos texto adicional, agregar el componente Text
        if (buttonSprite == null)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);
            
            Text txt = textObj.AddComponent<Text>();
            txt.text = labelText;
            txt.fontSize = 36; // Aumentado de 24 a 36
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;

            Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
            if (retroFont != null) txt.font = retroFont;
            else txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            RectTransform txtRect = textObj.GetComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;
            txtRect.sizeDelta = Vector2.zero;
        }
    }
}
