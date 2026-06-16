using UnityEngine;

/// <summary>
/// ReconnectController — Controla el minijuego 1: "Reconnect — Unir Cables del Sistema".
/// Nivel 1: Sala de Servidores.
///
/// Mecánica base (según reporte):
/// - Dos columnas de puertos de colores (izquierda y derecha).
/// - El jugador hace clic en un puerto izquierdo y arrastra el cable
///   hasta el puerto del mismo color en la columna derecha.
/// - Los cables NO pueden cruzarse entre sí.
/// - El jugador debe planificar las conexiones antes de realizarlas.
/// - El minijuego se completa cuando todos los pares están conectados correctamente.
/// </summary>
public class ReconnectController : MonoBehaviour
{
    // -------------------------------------------------------
    // VARIABLES DE ESTADO
    // -------------------------------------------------------
    private bool isMinigameActive = false;

    // PENDIENTE: Oscar define cuántos pares de cables hay por nivel
    // y si la cantidad varía con la dificultad progresiva.
    // private int totalPairs;
    // private int connectedPairs = 0;

    // PENDIENTE: definir la estructura de un "puerto" y un "cable".
    // Opciones: ScriptableObject, clase serializable, o componente independiente (CableController.cs).
    // Ejemplo orientativo:
    // private CableController selectedCable; // Cable actualmente siendo arrastrado

    // -------------------------------------------------------
    // INICIO DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Inicializa el minijuego. Llamar desde GameManager al cargar el Nivel 1.
    /// </summary>
    public void StartMinigame()
    {
        isMinigameActive = true;
        // connectedPairs = 0;

        // PENDIENTE: instanciar o activar los puertos y cables en pantalla.
        // PENDIENTE: ¿los colores de los cables son fijos o se generan aleatoriamente?
        // TimerController.Instance.StartTimer(tiempoDefinidoPorEquipo);
        // UIManager.Instance.ShowMinigameInstruction("¡Conecta los cables del mismo color!");
    }

    // -------------------------------------------------------
    // INTERACCIÓN DEL JUGADOR (MOUSE)
    // -------------------------------------------------------

    /// <summary>
    /// Detectar clic sobre un puerto para iniciar el arrastre del cable.
    /// PENDIENTE: definir si la detección es por Raycast, OnMouseDown o sistema de eventos UI.
    /// </summary>
    private void OnCableSelected(/* CableController cable */)
    {
        // selectedCable = cable;
        // PENDIENTE: activar renderizado visual del cable mientras se arrastra.
    }

    /// <summary>
    /// Mientras se arrastra el cable, dibujarlo siguiendo el cursor.
    /// PENDIENTE: definir si se usa LineRenderer u otro método visual.
    /// </summary>
    private void OnCableDragging()
    {
        // if (selectedCable == null) return;
        // Actualizar posición visual del cable hacia el cursor
        // PENDIENTE: implementar detección de cruce entre cables.
    }

    /// <summary>
    /// Al soltar el clic, verificar si el cable llegó al puerto correcto.
    /// </summary>
    private void OnCableReleased(/* CableController targetPort */)
    {
        // if (selectedCable == null) return;

        // PENDIENTE: verificar si el color del cable coincide con el puerto destino.
        // PENDIENTE: verificar que el cable no se cruce con otros ya conectados.

        // Si conexión correcta:
        // connectedPairs++;
        // AudioManager.Instance.PlaySFX(sfxCableConnected);
        // CheckMinigameComplete();

        // Si conexión incorrecta:
        // AudioManager.Instance.PlaySFX(sfxCableWrong);
        // Revertir cable a posición original

        // selectedCable = null;
    }

    // -------------------------------------------------------
    // CONDICIÓN DE VICTORIA DEL MINIJUEGO
    // -------------------------------------------------------

    /// <summary>
    /// Verifica si todos los pares han sido conectados correctamente.
    /// </summary>
    private void CheckMinigameComplete()
    {
        // if (connectedPairs >= totalPairs)
        // {
        //     isMinigameActive = false;
        //     TimerController.Instance.StopTimer();
        //     GameManager.Instance.OnLevelComplete();
        // }
    }

    // -------------------------------------------------------
    // CONDICIÓN DE DERROTA (Timer expirado)
    // -------------------------------------------------------

    /// <summary>
    /// Llamar desde TimerController cuando el tiempo llega a 0.
    /// PENDIENTE: definir si el minijuego se reinicia o se pasa al siguiente nivel con penalización.
    /// </summary>
    public void OnTimerExpired()
    {
        isMinigameActive = false;
        // LifeManager.Instance.LoseLife();
    }
}