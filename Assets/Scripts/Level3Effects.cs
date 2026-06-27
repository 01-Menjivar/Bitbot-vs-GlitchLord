using UnityEngine;
using System.Collections;

/// <summary>
/// Level3Effects — Gestiona los efectos visuales específicos del Nivel 3 (Debug Smash).
/// Incluye efectos de daño (parpadeo + deformación) y destrucción (explosión de píxeles glitch).
/// </summary>
public class Level3Effects : MonoBehaviour
{
    private static Sprite pixelSprite;

    [Header("Efectos de Pantalla (Pulido)")]
    [SerializeField] private SpriteRenderer virusOverlay;
    private Vector3 originalCameraPos;
    private bool isShaking = false;

    [SerializeField] private SpriteRenderer backgroundRenderer;
    private Sprite originalBg;
    [SerializeField] private Sprite vireusesBg;
    [SerializeField] private Sprite failBg;
    [SerializeField] private Sprite winBg;

    private void Start()
    {
        if (Camera.main != null)
        {
            originalCameraPos = Camera.main.transform.localPosition;
        }

        if (virusOverlay == null)
        {
            GameObject overlayObj = new GameObject("VirusOverlay");
            overlayObj.transform.SetParent(transform);
            overlayObj.transform.position = new Vector3(0, 0, -8f);
            virusOverlay = overlayObj.AddComponent<SpriteRenderer>();
            virusOverlay.sprite = GetOrCreatePixelSprite();
            virusOverlay.color = new Color(1f, 0f, 0f, 0f);
            overlayObj.transform.localScale = new Vector3(30f, 30f, 1f);
            virusOverlay.sortingOrder = 20;
        }

        if (backgroundRenderer == null)
        {
            GameObject bgObj = GameObject.Find("Background_Level3");
            if (bgObj == null) bgObj = GameObject.Find("Background");
            if (bgObj == null) bgObj = GameObject.Find("Level 3");
            if (bgObj != null) backgroundRenderer = bgObj.GetComponent<SpriteRenderer>();
        }

        if (backgroundRenderer != null) 
        {
            originalBg = backgroundRenderer.sprite;
        }

#if UNITY_EDITOR
        vireusesBg = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/level 3/vireuses.png");
        failBg = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/level 3/level 3 fail.png");
        winBg = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/level 3/level 3 win.png");
#endif
    }

    /// <summary>
    /// Genera o recupera una textura blanca de 4x4 píxeles convertida en Sprite de forma procedural.
    /// Esto evita la necesidad de importar archivos de imagen adicionales para las partículas.
    /// </summary>
    public static Sprite GetOrCreatePixelSprite()
    {
        if (pixelSprite == null)
        {
            Texture2D tex = new Texture2D(4, 4);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    tex.SetPixel(x, y, Color.white);
                }
            }
            tex.Apply();
            tex.filterMode = FilterMode.Point;
            pixelSprite = Sprite.Create(tex, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 100f);
        }
        return pixelSprite;
    }

    /// <summary>
    /// Reproduce el efecto de daño: hace parpadear el bug en rojo y lo deforma (aplastamiento) temporalmente.
    /// </summary>
    public void PlayDamageEffect(GameObject target)
    {
        if (target == null) return;
        StartCoroutine(FlashAndSquashRoutine(target));
        PlayScreenShake(0.08f, 0.15f); // Shake breve al hacer daño
    }

    /// <summary>
    /// Actualiza la opacidad del efecto visual de infección (Virus Overlay).
    /// </summary>
    public void UpdateVirusOverlay(float normalizedInfection)
    {
        if (virusOverlay != null)
        {
            Color c = virusOverlay.color;
            // Crecimiento exponencial para que el efecto sea drástico hacia el final
            c.a = Mathf.Pow(normalizedInfection, 2.5f) * 0.85f; 
            virusOverlay.color = c;
        }

        if (backgroundRenderer != null && vireusesBg != null && originalBg != null)
        {
            if (normalizedInfection >= 0.7f)
            {
                // Efecto de parpadeo de glitch
                backgroundRenderer.sprite = (Time.frameCount % 20 < 10) ? vireusesBg : originalBg;
            }
            else
            {
                backgroundRenderer.sprite = originalBg;
            }
        }
    }

    public void ShowFailBackground()
    {
        if (backgroundRenderer != null && failBg != null)
        {
            backgroundRenderer.sprite = failBg;
            backgroundRenderer.sortingOrder = 100; // Ponerlo por encima de TODO
        }
    }

    public void ShowVictoryBackground()
    {
        if (backgroundRenderer != null && winBg != null)
        {
            backgroundRenderer.sprite = winBg;
            backgroundRenderer.sortingOrder = 100; // Ponerlo por encima de TODO
        }
    }

    /// <summary>
    /// Dispara un temblor de cámara (Screen Shake) temporal.
    /// </summary>
    public void PlayScreenShake(float intensity, float duration)
    {
        if (!isShaking && Camera.main != null)
        {
            StartCoroutine(ScreenShakeRoutine(intensity, duration));
        }
    }

    private IEnumerator ScreenShakeRoutine(float intensity, float duration)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (Camera.main != null)
            {
                Vector3 randomOffset = Random.insideUnitSphere * intensity;
                randomOffset.z = 0f; // Mantener la profundidad intacta en 2D
                Camera.main.transform.localPosition = originalCameraPos + randomOffset;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (Camera.main != null)
        {
            Camera.main.transform.localPosition = originalCameraPos;
        }
        isShaking = false;
    }

    /// <summary>
    /// Reproduce el efecto de destrucción: genera una explosión de partículas glitch de colores aleatorios.
    /// </summary>
    public void PlayDestructionEffect(Vector3 position)
    {
        int particleCount = Random.Range(10, 16);
        Color[] glitchColors = { Color.cyan, Color.magenta, Color.yellow, Color.red, Color.white };

        for (int i = 0; i < particleCount; i++)
        {
            GameObject particleGo = new GameObject("Lvl3Debris");
            particleGo.transform.position = position;
            particleGo.transform.localScale = new Vector3(Random.Range(0.6f, 1.3f), Random.Range(0.6f, 1.3f), 1f);

            SpriteRenderer sr = particleGo.AddComponent<SpriteRenderer>();
            sr.sprite = GetOrCreatePixelSprite();
            sr.sortingOrder = 7; // Capa superior a los bugs (6) y al monitor (5)

            Level3DebrisParticle particle = particleGo.AddComponent<Level3DebrisParticle>();

            // Calcular dirección y velocidad de dispersión aleatoria
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float speed = Random.Range(3.0f, 6.0f);
            Color pColor = glitchColors[Random.Range(0, glitchColors.Length)];

            particle.Initialize(dir, speed, pColor);
        }
    }

    private IEnumerator FlashAndSquashRoutine(GameObject target)
    {
        if (target == null) yield break;

        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        Vector3 originalScale = target.transform.localScale;

        if (sr == null) yield break;

        // 1. Color de daño (rojo)
        sr.color = Color.red;

        // 2. Efecto de deformación (Squash & Stretch)
        float elapsed = 0f;
        float duration = 0.12f;
        Vector3 targetScale = new Vector3(originalScale.x * 1.35f, originalScale.y * 0.65f, originalScale.z);

        while (elapsed < duration)
        {
            if (target == null) yield break; // Evitar error si el bug es destruido durante el parpadeo

            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            if (t < 0.5f)
            {
                target.transform.localScale = Vector3.Lerp(originalScale, targetScale, t * 2f);
            }
            else
            {
                target.transform.localScale = Vector3.Lerp(targetScale, originalScale, (t - 0.5f) * 2f);
            }
            yield return null;
        }

        // Restaurar estado e iluminación original si el objeto sigue existiendo
        if (target != null)
        {
            target.transform.localScale = originalScale;
            if (sr != null)
            {
                sr.color = Color.white;
            }
        }
    }
}

/// <summary>
/// Level3DebrisParticle — Controla el comportamiento físico de cada partícula de la explosión del Level 3.
/// </summary>
public class Level3DebrisParticle : MonoBehaviour
{
    private Vector2 velocity;
    private float rotationSpeed;
    private float fadeSpeed;
    private SpriteRenderer sr;
    private Color color;

    public void Initialize(Vector2 direction, float speed, Color particleColor)
    {
        velocity = direction * speed;
        rotationSpeed = Random.Range(-450f, 450f);
        fadeSpeed = Random.Range(2.0f, 4.0f);
        color = particleColor;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = color;
        }
    }

    private void Update()
    {
        // Movimiento lineal con fricción
        transform.position += (Vector3)velocity * Time.deltaTime;
        velocity *= 0.94f; // Desaceleración progresiva

        // Rotación
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        // Desvanecimiento e iluminación
        if (sr != null)
        {
            color.a -= fadeSpeed * Time.deltaTime;
            sr.color = color;
            if (color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
