using UnityEngine;

/// <summary>
/// GlitchParticle — Partícula digital retro (dígitos '0' y '1') que sale disparada
/// de forma radial y con física cuando el jugador recibe daño.
/// </summary>
public class GlitchParticle : MonoBehaviour
{
    private static Sprite zeroSprite;
    private static Sprite oneSprite;

    private Vector3 velocity;
    private float rotationSpeed;
    private float lifetime;
    private float maxLifetime;
    private SpriteRenderer sr;

    /// <summary>
    /// Genera una explosión radial de dígitos glitch en la posición especificada.
    /// </summary>
    public static void SpawnBurst(Vector3 position, int count)
    {
        InitializeSprites();

        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject("GlitchParticle_Binary");
            go.transform.position = position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);
            
            GlitchParticle particle = go.AddComponent<GlitchParticle>();
            particle.Setup();
        }
    }

    private static void InitializeSprites()
    {
        if (zeroSprite != null && oneSprite != null) return;

        Color transparent = new Color(0, 0, 0, 0);
        Color white = Color.white;

        // 1. Crear sprite para el dígito '0' (5x5 pixels)
        Texture2D tex0 = new Texture2D(5, 5, TextureFormat.RGBA32, false);
        tex0.filterMode = FilterMode.Point;
        
        // Limpiar fondo
        for (int y = 0; y < 5; y++)
            for (int x = 0; x < 5; x++)
                tex0.SetPixel(x, y, transparent);

        // Dibujar contorno del '0'
        for (int x = 1; x <= 3; x++) { tex0.SetPixel(x, 0, white); tex0.SetPixel(x, 4, white); }
        for (int y = 1; y <= 3; y++) { tex0.SetPixel(1, y, white); tex0.SetPixel(3, y, white); }

        tex0.Apply();
        zeroSprite = Sprite.Create(tex0, new Rect(0, 0, 5, 5), new Vector2(0.5f, 0.5f), 10f); // 10 pixels per unit -> 0.5 units size

        // 2. Crear sprite para el dígito '1' (5x5 pixels)
        Texture2D tex1 = new Texture2D(5, 5, TextureFormat.RGBA32, false);
        tex1.filterMode = FilterMode.Point;
        
        // Limpiar fondo
        for (int y = 0; y < 5; y++)
            for (int x = 0; x < 5; x++)
                tex1.SetPixel(x, y, transparent);

        // Dibujar '1'
        for (int y = 0; y < 5; y++) tex1.SetPixel(2, y, white);
        tex1.SetPixel(1, 3, white);
        tex1.SetPixel(1, 0, white);
        tex1.SetPixel(3, 0, white);

        tex1.Apply();
        oneSprite = Sprite.Create(tex1, new Rect(0, 0, 5, 5), new Vector2(0.5f, 0.5f), 10f); // 10 pixels per unit -> 0.5 units size
    }

    private void Setup()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
        
        // Alternar aleatoriamente entre '0' y '1'
        sr.sprite = Random.value > 0.5f ? zeroSprite : oneSprite;
        
        // Paleta retro-glitch: Rojo brillante (#FF3333) o Cian eléctrico (#33E6FF)
        sr.color = Random.value > 0.5f ? new Color(1f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.9f, 1f, 1f);
        
        // Asegurar que renderice por encima de otros sprites del jugador
        sr.sortingOrder = 15;

        // Movimiento inicial de explosión (principalmente hacia arriba y a los lados)
        float angle = Random.Range(45f, 135f) * Mathf.Deg2Rad;
        float speed = Random.Range(3f, 7f);
        velocity = new Vector3(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed, 0);

        rotationSpeed = Random.Range(-360f, 360f);
        maxLifetime = Random.Range(0.5f, 0.8f);
        lifetime = 0f;

        // Escala inicial aleatoria para dar variedad
        transform.localScale = Vector3.one * Random.Range(0.6f, 1.2f);
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        // 1. Aplicar movimiento
        transform.position += velocity * Time.deltaTime;
        
        // 2. Gravedad ligera y arrastre
        velocity.y -= 12f * Time.deltaTime; // Fuerza de gravedad digital
        velocity.x *= Mathf.Exp(-1.5f * Time.deltaTime); // Fricción del aire

        // 3. Rotación continua
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // 4. Desvanecimiento (fade out) y encogimiento (scale down)
        float ratio = lifetime / maxLifetime;
        
        Color c = sr.color;
        c.a = Mathf.Clamp01(1f - ratio);
        sr.color = c;

        transform.localScale = Vector3.one * (1f - ratio) * 1.2f;
    }
}
