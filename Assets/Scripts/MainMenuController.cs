using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class MainMenuController : MonoBehaviour
{
    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        // 1. Encontrar o crear Canvas y configurar el fondo de forma robusta
        Canvas canvas = GameObject.Find("Canvas")?.GetComponent<Canvas>();
        if (canvas == null) canvas = FindObjectOfType<Canvas>();
        
        if (canvas != null)
        {
            // Cargar y aplicar el fondo del menú principal (funciona en Edit Mode y Play Mode)
            GameObject bgObj = GameObject.Find("Background_MainMenu");
            if (bgObj == null)
            {
                // Creamos el GameObject con el componente RectTransform directamente
                bgObj = new GameObject("Background_MainMenu", typeof(RectTransform));
                bgObj.transform.SetParent(canvas.transform, false);
                bgObj.transform.SetAsFirstSibling(); // Mandar al fondo de renderizado
            }

            Image bgImage = bgObj.GetComponent<Image>();
            if (bgImage == null)
            {
                bgImage = bgObj.AddComponent<Image>();
            }

            Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/Background3");
            if (bgSprite != null)
            {
                bgImage.sprite = bgSprite;
                bgImage.preserveAspect = false; // Estirar para llenar toda la pantalla
                bgImage.color = Color.white; // Opacidad total
            }
            else
            {
                bgImage.color = new Color(0.05f, 0.05f, 0.08f, 1f); // Fallback color oscuro
            }
            
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            bgRect.anchoredPosition = Vector2.zero;

            // 2. Estilizar botones/logo SÓLO en tiempo de ejecución (Play Mode) para evitar dañar la escena estática

            if (Application.isPlaying)
            {
                AdjustButtonSize(canvas, "Button_Play");
                AdjustButtonSize(canvas, "Button_Settings");
                AdjustButtonSize(canvas, "Button_Exit");
                
                // También ajustar el botón de regreso dentro del panel de ajustes si existe
                if (settingsPanel != null)
                {
                    AdjustButtonSize(settingsPanel.transform, "Button_Back");
                }
            }
            // --- REDISEÑAR PANEL DE AJUSTES (SISTEMA CONFIGURADO) ---
            if (Application.isPlaying && settingsPanel != null)
            {
                RestyleSettingsPanel();
            }
        }
    }

    private void RestyleSettingsPanel()
    {
        Debug.Log("[MainMenuController] RestyleSettingsPanel: Aplicando rediseño retro-cyberpunk...");

        // 1. Redimensionar y reposicionar el panel de ajustes
        RectTransform panelRect = settingsPanel.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(900f, 650f);
        }

        // 2. Estilizar la imagen de fondo con un borde de neón cian
        Image bgImg = settingsPanel.GetComponent<Image>();
        if (bgImg != null)
        {
            bgImg.sprite = null; // Quitar cualquier sprite genérico
            bgImg.color = new Color(0.04f, 0.08f, 0.15f, 0.96f); // Azul oscuro cyber/transparente
        }

        // Agregar outline de neón al panel si no tiene uno
        Outline panelOutline = settingsPanel.GetComponent<Outline>();
        if (panelOutline == null)
        {
            panelOutline = settingsPanel.gameObject.AddComponent<Outline>();
        }
        panelOutline.effectColor = new Color(0f, 0.95f, 1f, 0.85f); // Neón cian
        panelOutline.effectDistance = new Vector2(4f, -4f);

        // 3. Desactivar el texto estático antiguo "SettingsText"
        Transform oldTextTr = settingsPanel.transform.Find("SettingsText");
        if (oldTextTr != null)
        {
            oldTextTr.gameObject.SetActive(false);
        }

        // 4. Limpiar UI dinámica previa para evitar duplicados
        Transform oldDynamic = settingsPanel.transform.Find("DynamicUI");
        if (oldDynamic != null)
        {
            Destroy(oldDynamic.gameObject);
        }

        // 5. Crear contenedor para la nueva interfaz
        GameObject dynamicUI = new GameObject("DynamicUI", typeof(RectTransform));
        dynamicUI.transform.SetParent(settingsPanel.transform, false);
        RectTransform dynRect = dynamicUI.GetComponent<RectTransform>();
        dynRect.anchorMin = Vector2.zero;
        dynRect.anchorMax = Vector2.one;
        dynRect.sizeDelta = Vector2.zero;

        // --- ENCABEZADO ---
        // Logo de Megacorp
        GameObject logoObj = new GameObject("MegacorpLogo", typeof(RectTransform));
        logoObj.transform.SetParent(dynamicUI.transform, false);
        Image logoImg = logoObj.AddComponent<Image>();
        logoImg.sprite = LoadSpriteFromResources("Logo_MegaCorp");
        if (logoImg.sprite != null)
        {
            logoImg.preserveAspect = true;
            logoImg.color = new Color(0f, 0.95f, 1f, 0.9f); // Teñido en cian
            RectTransform logoRect = logoObj.GetComponent<RectTransform>();
            logoRect.anchorMin = new Vector2(0.5f, 1f);
            logoRect.anchorMax = new Vector2(0.5f, 1f);
            logoRect.pivot = new Vector2(0.5f, 1f);
            logoRect.anchoredPosition = new Vector2(0f, -30f);
            logoRect.sizeDelta = new Vector2(250f, 60f);
        }

        // Título del Sistema
        Text titleText = CreateText(dynamicUI.transform, "SystemTitle", "SISTEMA CONFIGURADO", 24, new Color(0f, 0.95f, 1f, 1f), TextAnchor.MiddleCenter,
            new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -100f), new Vector2(600f, 40f));
        AddOutline(titleText.gameObject, Color.black, new Vector2(2f, -2f));

        // Subtítulo
        CreateText(dynamicUI.transform, "SystemSubtitle", "MEGACORP OS v2.0.6 - CONFIGURATION PROTOCOL", 10, new Color(0.35f, 0.5f, 0.7f, 1f), TextAnchor.MiddleCenter,
            new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -140f), new Vector2(600f, 25f));

        // Línea divisora decorativa
        GameObject lineObj = new GameObject("Divider", typeof(RectTransform));
        lineObj.transform.SetParent(dynamicUI.transform, false);
        Image lineImg = lineObj.AddComponent<Image>();
        lineImg.color = new Color(0f, 0.95f, 1f, 0.4f);
        RectTransform lineRect = lineObj.GetComponent<RectTransform>();
        lineRect.anchorMin = new Vector2(0.5f, 1f);
        lineRect.anchorMax = new Vector2(0.5f, 1f);
        lineRect.pivot = new Vector2(0.5f, 1f);
        lineRect.anchoredPosition = new Vector2(0f, -165f);
        lineRect.sizeDelta = new Vector2(800f, 2f);

        // --- COLUMNA IZQUIERDA (CONTROLES Y MISIÓN) ---
        GameObject leftCol = new GameObject("LeftColumn", typeof(RectTransform));
        leftCol.transform.SetParent(dynamicUI.transform, false);
        RectTransform leftRect = leftCol.GetComponent<RectTransform>();
        leftRect.anchorMin = new Vector2(0.06f, 0.18f);
        leftRect.anchorMax = new Vector2(0.52f, 0.70f);
        leftRect.sizeDelta = Vector2.zero;

        string controlsContent = 
            "<color=#00FF66>► CONTROLES DE SISTEMA</color>\n\n" +
            "  MOVERSE:   [W A S D] / [FLECHAS]\n" +
            "  ACCIÓN:    [CLIC IZQUIERDO]\n" +
            "  PAUSAR:    [ESC] / [Tecla P]\n\n\n" +
            "<color=#00FF66>► DIRECTRICES DE MISIÓN</color>\n\n" +
            "  1. Contén la infección de Glitch-Lord.\n" +
            "  2. Captura los archivos de datos válidos.\n" +
            "  3. Evita que los virus toquen la red.\n" +
            "  4. ¡Protege el núcleo de MegaCorp!";

        Text leftTxt = CreateText(leftCol.transform, "ControlsAndMission", controlsContent, 12, Color.white, TextAnchor.UpperLeft,
            Vector2.zero, Vector2.one, Vector2.up, Vector2.zero, Vector2.zero);
        leftTxt.lineSpacing = 1.4f;
        leftTxt.supportRichText = true;
        AddOutline(leftTxt.gameObject, Color.black, new Vector2(1.5f, -1.5f));

        // --- COLUMNA DERECHA (AVATAR Y CRÉDITOS) ---
        GameObject rightCol = new GameObject("RightColumn", typeof(RectTransform));
        rightCol.transform.SetParent(dynamicUI.transform, false);
        RectTransform rightRect = rightCol.GetComponent<RectTransform>();
        rightRect.anchorMin = new Vector2(0.58f, 0.18f);
        rightRect.anchorMax = new Vector2(0.94f, 0.70f);
        rightRect.sizeDelta = Vector2.zero;

        // Avatar Frame
        GameObject avatarFrame = new GameObject("AvatarFrame", typeof(RectTransform));
        avatarFrame.transform.SetParent(rightCol.transform, false);
        Image frameImg = avatarFrame.AddComponent<Image>();
        frameImg.color = new Color(0f, 0.95f, 1f, 0.05f); // Fondo cian muy transparente
        Outline frameOutline = avatarFrame.AddComponent<Outline>();
        frameOutline.effectColor = new Color(0f, 0.95f, 1f, 0.7f);
        frameOutline.effectDistance = new Vector2(2f, -2f);
        RectTransform frameRect = avatarFrame.GetComponent<RectTransform>();
        frameRect.anchorMin = new Vector2(0.5f, 1f);
        frameRect.anchorMax = new Vector2(0.5f, 1f);
        frameRect.pivot = new Vector2(0.5f, 1f);
        frameRect.anchoredPosition = new Vector2(0f, 0f);
        frameRect.sizeDelta = new Vector2(120f, 120f);

        // Avatar de Bit-Bot
        GameObject avatarObj = new GameObject("BitbotAvatar", typeof(RectTransform));
        avatarObj.transform.SetParent(avatarFrame.transform, false);
        Image avatarImg = avatarObj.AddComponent<Image>();
        avatarImg.sprite = LoadSpriteFromResources("Characters/bit-bot_fronta");
        if (avatarImg.sprite != null)
        {
            avatarImg.preserveAspect = true;
            RectTransform avRect = avatarObj.GetComponent<RectTransform>();
            avRect.anchorMin = Vector2.zero;
            avRect.anchorMax = Vector2.one;
            avRect.sizeDelta = new Vector2(-10f, -10f); // Pequeño margen dentro del frame
            avRect.anchoredPosition = Vector2.zero;
        }

        // Título del Asistente
        CreateText(rightCol.transform, "AssistantTitle", "ASISTENTE DE SEGURIDAD: BIT-BOT", 9, new Color(0f, 0.95f, 1f, 0.8f), TextAnchor.MiddleCenter,
            new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -130f), new Vector2(300f, 20f));

        // Créditos
        string creditsContent = 
            "<color=#FFCC00>► CRÉDITOS DE DESARROLLO</color>\n\n" +
            "  DISEÑO Y ARQUITECTURA:\n" +
            "  EQUIPO PEPSIMAN\n\n" +
            "  SECCIÓN 01 - CICLO 01/2026";

        Text rightTxt = CreateText(rightCol.transform, "Credits", creditsContent, 10, Color.white, TextAnchor.UpperLeft,
            new Vector2(0f, 0f), new Vector2(1f, 0.40f), Vector2.up, Vector2.zero, Vector2.zero);
        rightTxt.lineSpacing = 1.3f;
        rightTxt.supportRichText = true;
        AddOutline(rightTxt.gameObject, Color.black, new Vector2(1.5f, -1.5f));

        // --- BOTÓN VOLVER ---
        Transform backBtnTr = settingsPanel.transform.Find("Button_Back");
        if (backBtnTr != null)
        {
            RectTransform backRect = backBtnTr.GetComponent<RectTransform>();
            backRect.anchorMin = new Vector2(0.5f, 0f);
            backRect.anchorMax = new Vector2(0.5f, 0f);
            backRect.pivot = new Vector2(0.5f, 0f);
            backRect.anchoredPosition = new Vector2(0f, 35f);
            backRect.sizeDelta = new Vector2(240f, 65f);

            // Asegurarse de que el texto del botón de volver use PressStart2P
            Text backBtnText = backBtnTr.GetComponentInChildren<Text>();
            if (backBtnText != null)
            {
                backBtnText.text = "VOLVER";
                backBtnText.fontSize = 14;
                Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
                if (retroFont != null) backBtnText.font = retroFont;
            }
        }
    }

    private Text CreateText(Transform parent, string name, string content, int fontSize, Color color, TextAnchor alignment, 
        Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);

        Text txt = go.AddComponent<Text>();
        txt.text = content;
        txt.fontSize = fontSize;
        txt.color = color;
        txt.alignment = alignment;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        txt.verticalOverflow = VerticalWrapMode.Overflow;

        Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
        if (retroFont != null) txt.font = retroFont;
        else txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;

        return txt;
    }

    private Outline AddOutline(GameObject go, Color color, Vector2 distance)
    {
        Outline outline = go.AddComponent<Outline>();
        outline.effectColor = color;
        outline.effectDistance = distance;
        return outline;
    }

    private Sprite LoadSpriteFromResources(string path)
    {
        Sprite s = Resources.Load<Sprite>(path);
        if (s == null)
        {
            Texture2D tex = Resources.Load<Texture2D>(path);
            if (tex != null)
            {
                s = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
        }
        return s;
    }

    private void AdjustButtonSize(Component parent, string name)
    {
        Transform t = FindChildRecursive(parent.transform, name);
        if (t != null)
        {
            RectTransform rect = t.GetComponent<RectTransform>();
            if (rect != null)
            {
                // Cargar y asignar el sprite del botón correspondiente desde Resources
                Image img = t.GetComponent<Image>();
                if (img != null)
                {
                    Sprite btnSprite = Resources.Load<Sprite>("UI/Buttons/" + name);
                    if (btnSprite != null)
                    {
                        img.sprite = btnSprite;
                        img.color = Color.white; // Asegurar opacidad normal
                    }
                }

                // Ajustar fuente del texto del botón si tiene, respetando el tamaño establecido en escena
                Text txt = t.GetComponentInChildren<Text>();
                if (txt != null)
                {
                    Font retroFont = Resources.Load<Font>("Fonts/PressStart2P");
                    if (retroFont != null)
                    {
                        txt.font = retroFont;
                    }
                    txt.color = Color.white;
                }
            }
        }
    }

    private Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null) return result;
        }
        return null;
    }

    /// <summary>
    /// Abre la pantalla intermedia de selección de sistemas/niveles.
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("MainMenuController: Iniciando juego. Cargando selección de niveles...");
        SceneManager.LoadScene("LevelSelectionScene");
    }

    /// <summary>
    /// Abre el panel de ajustes de la interfaz.
    /// </summary>
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            Debug.Log("MainMenuController: Abriendo panel de ajustes.");
            settingsPanel.SetActive(true);
            if (Application.isPlaying)
            {
                RestyleSettingsPanel();
            }
        }
        else
        {
            Debug.LogWarning("MainMenuController: No se ha asignado la referencia al panel de ajustes.");
        }
    }

    /// <summary>
    /// Cierra el panel de ajustes de la interfaz.
    /// </summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            Debug.Log("MainMenuController: Cerrando panel de ajustes.");
            settingsPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Cierra la aplicación. Funciona tanto en ejecuciones compiladas como dentro del Editor.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("MainMenuController: Saliendo del juego...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
