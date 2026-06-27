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
        }
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
