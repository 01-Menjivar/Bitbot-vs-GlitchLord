# Fuentes

## Catálogo de Fuentes e Implementación

### 1. Press Start 2P
* **Tipo:** Pixel / 8-bit (Moniespaciada)
* **Estilo:** Retro Arcade clásico.
* **Uso Principal:** Textos de alto impacto visual, títulos principales y alertas críticas del sistema.
* **Casos de Uso Específicos:**
  * Títulos de pantallas principales (Menú de Inicio, Selección de Niveles).
  * Mensajes de estado crítico en tipografía pixel art grande: `SYSTEM RESTORED`, `GAME OVER!`, `¡TIEMPO AGOTADO!`, `¡DERROTA!`.
  * Números grandes en interfaces (conteo regresivo inicial 3, 2, 1).
* **Notas de Diseño:** Al ser una fuente muy densa y blocky, pierde legibilidad rápidamente en tamaños pequeños o textos de más de tres líneas. Evitar su uso en párrafos.

### 2. Pixel Operator
* **Tipo:** Pixel / Proporcional (Sans-Serif Pixel)
* **Estilo:** Digital limpio y estilizado.
* **Uso Principal:** Interfaz de usuario activa (HUD), subtítulos, cuadros de diálogo y textos informativos intermedios.
* **Casos de Uso Específicos:**
  * Cronómetro visible en pantalla y barra de progreso.
  * Instrucciones breves antes de iniciar cada minijuego (ej. *"Unir los puertos del mismo color sin cruzarlos"*).
  * Nombres de los personajes en diálogos o cinemáticas (`Bit-Bot`, `Directora CTRL`, `Glitch-Lord`).
  * Descripciones breves en el menú de configuración o créditos del equipo.
* **Notas de Diseño:** Ofrece una excelente legibilidad en formato pixel art incluso en tamaños moderados (12pt - 16pt), lo que la hace perfecta para mantener la cohesión estética sin cansar la vista del jugador.

### 3. Roboto (Fuente No Pixel)
* **Tipo:** Sans-Serif Geométrica
* **Estilo:** Moderno y limpio.
* **Uso Principal:** Textos extensos, documentación en el juego o menús secundarios técnicos.
* **Casos de Uso Específicos:**
  * Textos largos de términos y condiciones, EULA o políticas de privacidad exigidas en plataformas de distribución.
  * Consola de errores/depuración interna (Testing logs) para facilitar el análisis técnico durante las pruebas.
  * Menús densos de configuración gráfica, de audio o asignación de teclas complejas.
* **Notas de Diseño:** Rompe la estética de los 8 bits de manera intencional para proporcionar un descanso visual y asegurar que los elementos administrativos o de accesibilidad sean 100% legibles. Su diseño limpio encaja de forma excelente con la temática de "software de oficina" y corporativo de *MegaCorp Systems*.