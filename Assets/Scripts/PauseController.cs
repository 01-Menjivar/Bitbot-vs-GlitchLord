using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Referencias de UI")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button exitButton;

    private bool isPaused = false;

    public bool canPause = false;

    private void Start()
    {
        EnsureEventSystem();

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (pauseButton != null)
        {
            pauseButton.gameObject.SetActive(false); // Oculto durante la intro
            pauseButton.onClick.AddListener(TogglePause);
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(TogglePause);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitToMenu);
        }
    }

    private void EnsureEventSystem()
    {
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            Debug.Log("[PauseController] Creando EventSystem automático...");
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
            esObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    private void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void ShowPauseButton()
    {
        canPause = true;
        if (pauseButton != null && !isPaused)
        {
            pauseButton.gameObject.SetActive(true);
        }
    }

    public void TogglePause()
    {
        if (!canPause) return;
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
            if (pauseButton != null) pauseButton.gameObject.SetActive(false);
            
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Click");
            }
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
            if (pauseButton != null) pauseButton.gameObject.SetActive(true);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("Click");
            }
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f; // Restaurar el tiempo antes de salir
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Click");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    private void OnDestroy()
    {
        // Asegurar que el tiempo vuelve a la normalidad si se cambia de escena
        Time.timeScale = 1f;
    }
}
