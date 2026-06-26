using UnityEngine;

/// <summary>
/// FallingObject — Controla el comportamiento individual de cada objeto que cae (archivos y virus).
/// Maneja el movimiento de caída, la rotación sutil y el ciclo de vida (destrucción por límite de pantalla).
/// </summary>
public class FallingObject : MonoBehaviour
{
    private float fallSpeed;
    private bool isValid;
    private float destroyYThreshold = -6f; // Límite inferior de la pantalla para destruir el objeto
    private float rotationSpeed;

    /// <summary>
    /// Inicializa las propiedades del objeto cuando es instanciado por el FileSpawner.
    /// </summary>
    /// <param name="speed">Velocidad de caída.</param>
    /// <param name="valid">Indica si el archivo es válido (true) o corrupto/virus (false).</param>
    public void Initialize(float speed, bool valid)
    {
        fallSpeed = speed;
        isValid = valid;
        
        // Rotación sutil aleatoria para darle dinamismo a la caída
        rotationSpeed = Random.Range(-40f, 40f);
    }

    private void Update()
    {
        // Mover el objeto hacia abajo en espacio del mundo
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);

        // Aplicar rotación sutil en el eje Z
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Auto-destruirse si cae por debajo del límite vertical establecido
        if (transform.position.y < destroyYThreshold)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Retorna si el objeto es un archivo válido o un virus.
    /// </summary>
    public bool IsValid() => isValid;

    private void OnBecameInvisible()
    {
        // Medida de seguridad adicional para prevenir fugas de memoria si el objeto sale de la cámara
        Destroy(gameObject);
    }
}
