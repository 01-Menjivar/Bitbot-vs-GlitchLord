using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// LevelSelectionController — Controla el flujo de la escena LevelSelectionScene en tiempo de ejecución.
/// Vincula los eventos de los botones que ya están pre-configurados estáticamente en la escena.
/// </summary>
public class LevelSelectionController : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("[LSC] Vinculando lógica de clics a los botones de selección...");

        // Configurar Botón Nivel 2 (carga Nivel 3 - Base de Datos Central)
        ConfigureButtonListener("BtnLevel2", () =>
        {
            Debug.Log("[LSC] Cargando Nivel 3 (Base de Datos Central)...");
            if (GameManager.Instance != null) GameManager.Instance.ResetGame();
            SceneManager.LoadScene("Level3");
        });

        // Configurar Botón Nivel 3 (carga Nivel 2 - Red de Datos)
        ConfigureButtonListener("BtnLevel3", () =>
        {
            Debug.Log("[LSC] Cargando Nivel 2 (Red de Datos)...");
            if (GameManager.Instance != null) GameManager.Instance.ResetGame();
            SceneManager.LoadScene("Level2");
        });

        // Configurar Botón Back
        ConfigureButtonListener("Button_Back", () =>
        {
            Debug.Log("[LSC] Regresando al Menú Principal...");
            SceneManager.LoadScene("MainMenuScene");
        });
    }

    private void ConfigureButtonListener(string buttonName, System.Action onClickAction)
    {
        GameObject btnObj = GameObject.Find(buttonName);
        if (btnObj != null)
        {
            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => onClickAction());

                // Transición de color retro
                btn.transition = Selectable.Transition.ColorTint;
                ColorBlock colors = btn.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
                colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
                colors.selectedColor = Color.white;
                btn.colors = colors;
            }
            else
            {
                Debug.LogWarning($"[LSC] El objeto de botón '{buttonName}' no tiene un componente Button.");
            }
        }
        else
        {
            Debug.LogWarning($"[LSC] No se encontró el objeto de botón '{buttonName}' en la escena.");
        }
    }
}
