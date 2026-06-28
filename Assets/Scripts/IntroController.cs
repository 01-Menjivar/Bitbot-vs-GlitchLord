using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public Button continueButton;
    public GameObject introPanel;

    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        // Ocultar la pantalla de introducción
        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Click");
        }

        // Buscar el controlador del minijuego e iniciarlo
        FileCatcherController minigameController = FindObjectOfType<FileCatcherController>();
        if (minigameController != null)
        {
            minigameController.StartMinigame();
        }
        else
        {
            Debug.LogError("[IntroController] No se encontró FileCatcherController en la escena.");
        }
    }
}
