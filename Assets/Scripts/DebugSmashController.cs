using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DebugSmashController — Controla el minijuego 3: "Debug Smash — Eliminar Bugs de la Pantalla".
/// Nivel 3: Base de Datos Central.
/// </summary>
public class DebugSmashController : MonoBehaviour
{
    [Header("Configuración de Infección")]
    [SerializeField] private float gameDuration = 30f; // Tiempo límite en segundos para sobrevivir
    [SerializeField] private float maxInfection = 100f;
    [SerializeField] private float infectionRatePerBug = 1.5f; // Infección reducida
    [SerializeField] private float healPerBugKill = 15f; // Curación al matar bug (súper fácil)
    
    [Header("Referencias UI y Efectos")]
    [SerializeField] private Slider infectionBar; // Barra de progreso de infección
    [SerializeField] private Level3Effects levelEffects;

    private float currentInfection = 0f;
    private bool isMinigameActive = false;
    private BugSpawner spawner;

    // Temporizador interno (respaldo si no existe TimerController en escena)
    private float internalTimer = 0f;
    private bool usingInternalTimer = false;

    private void Awake()
    {
        spawner = GetComponent<BugSpawner>();
        Debug.Log("[DSC] Awake - spawner: " + (spawner != null ? "OK" : "NULL"));
        if (levelEffects == null) levelEffects = FindObjectOfType<Level3Effects>();
        Debug.Log("[DSC] Awake - levelEffects: " + (levelEffects != null ? "OK" : "NULL"));
        GenerateUI();
    }

    private void GenerateUI()
    {
        if (infectionBar != null) return;
        
        GameObject canvasObj = new GameObject("InfectionCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        
        GameObject sliderObj = new GameObject("InfectionSlider");
        sliderObj.transform.SetParent(canvasObj.transform, false);
        infectionBar = sliderObj.AddComponent<Slider>();
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchorMin = new Vector2(0.5f, 1f); sliderRect.anchorMax = new Vector2(0.5f, 1f);
        sliderRect.pivot = new Vector2(0.5f, 1f); sliderRect.anchoredPosition = new Vector2(0, -60);
        sliderRect.sizeDelta = new Vector2(800, 40);

        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(sliderObj.transform, false);
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero; bgRect.anchorMax = Vector2.one; bgRect.sizeDelta = Vector2.zero;

        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillAreaObj.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0.25f); fillAreaRect.anchorMax = new Vector2(1, 0.75f);
        fillAreaRect.sizeDelta = new Vector2(-10, 0);

        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform, false);
        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = Color.red;
        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.sizeDelta = Vector2.zero;

        infectionBar.fillRect = fillRect;
        infectionBar.interactable = false;
        infectionBar.transition = Selectable.Transition.None;
        infectionBar.value = 0f;
    }

    private void Update()
    {
        if (!isMinigameActive) return;

        // Temporizador interno si no hay TimerController
        if (usingInternalTimer)
        {
            internalTimer -= Time.deltaTime;
            if (internalTimer <= 0f)
            {
                internalTimer = 0f;
                usingInternalTimer = false;
                Debug.Log("[DSC] Temporizador interno expiró. Llamando OnTimerExpired().");
                OnTimerExpired();
                return;
            }
        }

        DetectClick();
        UpdateInfection();
    }

    public void StartMinigame()
    {
        Debug.Log("[DSC] StartMinigame() LLAMADO - isMinigameActive antes: " + isMinigameActive);
        isMinigameActive = true;
        currentInfection = 0f;
        UpdateInfectionUI();

        if (AudioManager.Instance != null && AudioManager.Instance.level3Theme != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.level3Theme);
            Debug.Log("[DSC] Musica iniciada.");
        }
        else
        {
            Debug.LogWarning("[DSC] AudioManager o level3Theme es NULL, no se puede reproducir musica.");
        }

        if (spawner != null) spawner.StartSpawning();
        else Debug.LogWarning("[DSC] spawner es NULL, no se pueden generar bugs.");

        if (TimerController.Instance != null)
        {
            Debug.Log("[DSC] Usando TimerController. Duracion: " + gameDuration);
            usingInternalTimer = false;
            TimerController.Instance.onTimerExpiredCallback = OnTimerExpired;
            TimerController.Instance.StartTimer(gameDuration);
        }
        else
        {
            Debug.LogWarning("[DSC] TimerController no encontrado. Usando temporizador INTERNO de " + gameDuration + " segundos.");
            usingInternalTimer = true;
            internalTimer = gameDuration;
        }

        if (UIManager.Instance != null) UIManager.Instance.ShowMinigameInstruction("¡Elimina los bugs para que la infección no llegue al 100%!");
    }

    private void DetectClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null)
            {
                BugHealth bugHealth = hit.collider.GetComponent<BugHealth>();
                if (bugHealth != null)
                {
                    bugHealth.TakeDamage(1);
                    if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Click");
                }
            }
        }
    }

    private void UpdateInfection()
    {
        if (spawner == null) return;
        
        int activeBugs = spawner.GetActiveBugs().Count;
        if (activeBugs > 0)
        {
            currentInfection += activeBugs * infectionRatePerBug * Time.deltaTime;
            currentInfection = Mathf.Clamp(currentInfection, 0, maxInfection);
            
            UpdateInfectionUI();

            if (levelEffects != null)
            {
                levelEffects.UpdateVirusOverlay(currentInfection / maxInfection);
                
                // Screen shake constante y suave si la infección supera el 80% (Peligro crítico)
                if (currentInfection >= maxInfection * 0.8f)
                {
                    levelEffects.PlayScreenShake(0.05f, 0.1f);
                }
            }

            if (currentInfection >= maxInfection)
            {
                EndMinigame(false); // Derrota por colapso del sistema
            }
        }
        else if (currentInfection > 0)
        {
            // Opcional: reducir la infección lentamente si la pantalla está limpia
            currentInfection -= infectionRatePerBug * Time.deltaTime;
            currentInfection = Mathf.Max(0, currentInfection);
            UpdateInfectionUI();
            if (levelEffects != null) levelEffects.UpdateVirusOverlay(currentInfection / maxInfection);
        }
    }

    private void UpdateInfectionUI()
    {
        if (infectionBar != null)
        {
            infectionBar.value = currentInfection / maxInfection;
        }
    }

    public void OnBugDestroyed(GameObject bug)
    {
        if (!isMinigameActive) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("BugDestroy");

        // Curar el sistema
        currentInfection -= healPerBugKill;
        currentInfection = Mathf.Max(0, currentInfection);
        UpdateInfectionUI();
        if (levelEffects != null) levelEffects.UpdateVirusOverlay(currentInfection / maxInfection);
    }

    private void EndMinigame(bool success)
    {
        isMinigameActive = false;

        if (spawner != null)
        {
            spawner.StopSpawning();
            spawner.ClearActiveBugs();
        }

        if (TimerController.Instance != null)
        {
            TimerController.Instance.StopTimer();
            TimerController.Instance.onTimerExpiredCallback = null;
        }

        if (AudioManager.Instance != null) AudioManager.Instance.StopMusic();

        if (success)
        {
            Debug.Log("DebugSmash survived successfully!");
            if (levelEffects != null) levelEffects.ShowVictoryBackground();
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("Victory");
            if (GameManager.Instance != null) GameManager.Instance.OnLevelComplete();
        }
        else
        {
            Debug.Log("System collapse! Infection reached 100%.");
            if (levelEffects != null) levelEffects.ShowFailBackground();
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX("GameOver");
            if (LifeManager.Instance != null)
            {
                LifeManager.Instance.LoseLife(); 
            }
        }
    }

    public void OnTimerExpired()
    {
        Debug.Log("[DSC] OnTimerExpired() disparado! isMinigameActive: " + isMinigameActive);
        if (!isMinigameActive) return;
        EndMinigame(true); // Victoria al sobrevivir el tiempo
    }
}