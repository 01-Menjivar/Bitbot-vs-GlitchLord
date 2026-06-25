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
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------------------------------------------------------
    // VARIABLES DE AUDIO
    // -------------------------------------------------------
    private AudioSource musicSource; // Para música de fondo (BGM)
    private AudioSource sfxSource;   // Para efectos de sonido (SFX)

    [Header("Música de Fondo (BGM)")]
    [SerializeField] private AudioClip menuTheme;
    [SerializeField] private AudioClip level1Theme; // Red de Datos (File Catcher)
    [SerializeField] private AudioClip level2Theme; // Base de Datos Central (Debug Smash)

    [Header("Efectos de Sonido (SFX)")]
    [SerializeField] private AudioClip sfxClick;
    [SerializeField] private AudioClip sfxError;
    [SerializeField] private AudioClip sfxFileCaught;
    [SerializeField] private AudioClip sfxVirusHit;
    [SerializeField] private AudioClip sfxBugDestroyed;
    [SerializeField] private AudioClip sfxVictory;
    [SerializeField] private AudioClip sfxGameOver;

    private void Start()
    {
        // Inicializar los AudioSource como componentes del GameObject
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
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

    // PENDIENTE: ¿se necesita control de volumen separado para BGM y SFX?
    // Ejemplo:
    // public void SetMusicVolume(float volume) => musicSource.volume = volume;
    // public void SetSFXVolume(float volume) => sfxSource.volume = volume;
}