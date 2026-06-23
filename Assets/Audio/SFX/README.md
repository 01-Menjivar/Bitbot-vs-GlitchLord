# Audio


##   Bitbot
Contiene los sonidos vocales y de reacción específicos del protagonista.

* **`dramatic.wav`**: Sonido de reacción exagerada, ideal para cuando Bit-Bot se encuentra con Glitch-Lord en las cinemáticas o cuando un minijuego entra en los últimos 3 segundos.
* **`happy.wav`**: Emisión alegre y aguda. Úsalo cuando su pantalla-rostro muestre una expresión de victoria al completar un minijuego.
* **`laugh.wav`**: Risita robótica o de victoria. 
* **`lose.wav`**: Sonido de decepción o falla técnica personal. Se reproduce cuando Bit-Bot comete un error crítico y muestra la pantalla de error.
* **`panic.wav`**: Tono tembloroso y eléctrico. Perfecto para el momento en que el sistema entra en estado "CRITICAL" (ej. Nivel 1 - Servidores).
* **`talking.mp3`**: "Blips" de diálogo para simular la voz de Bit-Bot durante las secuencias de texto. (Es Isabella de Animal Crossing)

---

## General
Agrupa los efectos de sonido de los minijuegos, interacciones del entorno, recompensas, penalizaciones y antagonistas.

**Alertas y Entorno:**
* **`alert1.wav`**: Sirena de advertencia. Ideal para el parpadeo rojo de "¡Alerta!" en el Nivel 3 (Saturación de bugs) o "¡Sistema Crítico!".
* **`ringtone.wav`**: Tono de llamada corporativo. Puede usarse para las interrupciones de la Directora CTRL o elementos de oficina en los menús.
* **`tetric.wav`**: Sonido ambiental oscuro o de tensión, excelente para el búnker subterráneo del Nivel 1.

**Enemigos y Fallos (Glitch-Lord / Bugs):**
* **`alien1.wav` - `alien3.wav`**: Apariciones de virus, movimientos erráticos de bugs rápidos o la presencia física de Glitch-Lord.
* **`glitch1.mp3` - `glitch3.mp3`**: Distorsiones y estática digital pesada. Úsalos para la animación de la "Pantalla Azul de la Muerte" y la corrupción del sistema.
* **`interference1.wav`**: Ruido eléctrico corto para cuando un cable se conecta incorrectamente o un archivo corrupto entra en pantalla.
* **`boom1.wav` / `boom2.wav` / `explosion1.wav` - `explosion3.wav`**: Impactos fuertes para cuando un bug se sale de control o el sistema colapsa al perder las 3 vidas ("núcleos de procesador").

**Feedback de Partida (Aciertos y Errores):**
* **`points1.wav` / `powerup1.wav` / `powerup2.wav` / `up1.wav` / `up2.wav`**: Feedback positivo instantáneo. Perfectos para "Archivo Recogido" (Nivel 2) o al restaurar el sistema con éxito.
* **`death1.wav` / `lose1.wav` - `lose3.wav` / `powerdown1.wav` / `powerdown2.wav`**: Sonidos de pérdida de vida (apagado de núcleo) y fallo de nivel.
* **`wawawawa.wav`**: El clásico trombón triste retro. Úsalo para la pantalla de "¡DERROTA!" o "¡TIEMPO AGOTADO!".
**`wii.wav` / `wiwiwi.wav`**: Zumbidos rápidos. Excelentes para el movimiento de arrastre rápido de Bit-Bot de izquierda a derecha en el Nivel 2.

---

## UI
Sonidos destinados a la Interfaz de Usuario (Menús, HUD, Cronómetros y clics).

**Interacción Estándar:**
* **`bip1.wav` - `bip3.wav`**: Tics cortos. Funcionan perfecto para el `Hover` de los botones o para el sonido del cronómetro contando los segundos.
* **`blip1.wav`**: Sonido de interfaz rápido, útil para diálogos o aparición de cuadros de texto.
* **`click1.ogg` / `confirmation.ogg`**: Clics sólidos para aceptar opciones en el menú o alinear los cables correctamente en el minijuego Nivel 1. 

**Interacciones Específicas:**
* **`boom.wav`**: Sonido sordo para un clic inválido o botón bloqueado.
* **`pop.wav`**: Sonido clave para la mecánica de *Debug Smash* (Nivel 3). Debe sonar cada vez que el usuario hace clic sobre un bug y este explota.
* **`musicsound.wav` / `musicsound2.wav`**: Pequeños jingles (ráfagas musicales) para transiciones entre escenas o aparición del logo.
* **`ramdon.wav` / `ramdon2.wav`**: Clics alternativos para variar el feedback auditivo y no fatigar el oído del jugador en menús extensos.
