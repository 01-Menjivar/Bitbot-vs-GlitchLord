using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;

    /// <summary>
    /// Inicia el juego cargando el primer nivel (Nivel 1: Red de Datos - File Catcher).
    /// </summary>
    public void PlayGame()
    {
        Debug.Log("MainMenuController: Iniciando juego. Cargando Nivel 1...");
        // Cargar la escena del primer nivel (Level1)
        SceneManager.LoadScene("Level1");
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
