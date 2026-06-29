using UnityEngine;

/// <summary>
/// AudioManager — Gestiona toda la reproducción de audio del juego.
/// Maneja tanto música de fondo (BGM) como efectos de sonido (SFX).
/// Los clips de audio los define Fabio en Assets/Audio.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // -------------------------------------------------------
    // SINGLETON
    // -------------------------------------------------------
    public static AudioManager Instance;



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
        // Asegurar que los componentes estén creados
        EnsureAudioSources();

        // Cambiar música de fondo según la escena
        switch (scene.name)
        {
            case "MainMenuScene":
            case "LevelSelectionScene":
                if (musicSource != null && musicSource.clip != menuTheme)
                {
                    PlayMusic(menuTheme);
                }
                break;
            case "Level2":
                if (musicSource != null && musicSource.clip != level2Theme)
                {
                    PlayMusic(level2Theme);
                }
                break;
            case "Level3":
                // Detener música de menú. Level3 iniciará su música cuando comience el juego
                StopMusic();
                break;
            case "GameOverScreen":
            case "VictoryScreen":
                StopMusic();
                break;
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE AUDIO
    // -------------------------------------------------------
    private AudioSource musicSource; // Para música de fondo (BGM)
    private AudioSource sfxSource;   // Para efectos de sonido (SFX)

    [Header("Música de Fondo (BGM)")]
    [SerializeField] public AudioClip menuTheme;
    [SerializeField] public AudioClip level1Theme; // Red de Datos (File Catcher)
    [SerializeField] public AudioClip level2Theme; // Base de Datos Central (Debug Smash)
    [SerializeField] public AudioClip level3Theme; 

    [Header("Efectos de Sonido (SFX)")]
    [SerializeField] public AudioClip sfxClick;
    [SerializeField] public AudioClip sfxError;
    [SerializeField] public AudioClip sfxFileCaught;
    [SerializeField] public AudioClip sfxVirusHit;
    [SerializeField] public AudioClip sfxBugDestroyed;
    [SerializeField] public AudioClip sfxVictory;
    [SerializeField] public AudioClip sfxGameOver;

    private void Start()
    {
        EnsureAudioSources();

        // Reproducir música del menú si estamos en el menú principal o selección al arrancar
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenuScene" || currentScene == "LevelSelectionScene")
        {
            PlayMusic(menuTheme);
        }
    }

    private void EnsureAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
        }
    }

    // -------------------------------------------------------
    // MÚSICA DE FONDO
    // -------------------------------------------------------

    /// <summary>
    /// Reproduce un track de música de fondo.
    /// Llamar al cargar cada nivel desde GameManager.
    /// </summary>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // PENDIENTE: definir si hay transición/fade entre tracks al cambiar de nivel.

    // -------------------------------------------------------
    // EFECTOS DE SONIDO (SFX)
    // -------------------------------------------------------

    /// <summary>
    /// Reproduce un efecto de sonido puntual.
    /// Llamar desde los controladores de minijuego según el evento.
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Reproduce un efecto de sonido por nombre.
    /// </summary>
    public void PlaySFX(string soundName)
    {
        AudioClip clipToPlay = null;
        switch (soundName)
        {
            case "Click": clipToPlay = sfxClick; break;
            case "Error": clipToPlay = sfxError; break;
            case "FileCaught": clipToPlay = sfxFileCaught; break;
            case "VirusHit": clipToPlay = sfxVirusHit; break;
            case "BugDestroy": clipToPlay = sfxBugDestroyed; break;
            case "Victory": clipToPlay = sfxVictory; break;
            case "GameOver": clipToPlay = sfxGameOver; break;
            case "BugSpawn": clipToPlay = sfxError; break; // Fallback
        }

        if (clipToPlay != null) sfxSource.PlayOneShot(clipToPlay);
        else Debug.LogWarning("AudioManager: SFX no asignado en el Inspector para: " + soundName);
    }

    // PENDIENTE: ¿se necesita control de volumen separado para BGM y SFX?
    // Ejemplo:
    // public void SetMusicVolume(float volume) => musicSource.volume = volume;
    // public void SetSFXVolume(float volume) => sfxSource.volume = volume;
}