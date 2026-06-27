# Reglas y Contexto de Desarrollo - Bit-Bot vs Glitch-Lord

Este archivo define las reglas de desarrollo, estilo de código y contexto del proyecto para los agentes de Inteligencia Artificial que colaboren en este repositorio. Cualquier asistente de IA que abra este proyecto leerá y aplicará estas pautas de forma automática.

## Información del Proyecto
* **Nombre del Juego**: Bit-Bot vs Glitch-Lord
* **Género**: Minijuegos / Arcade 2D de Acción Rápida.
* **Estética**: Pixel Art con iluminación de neón.
* **Equipo de Desarrollo**: Pepsiman (Sección 01, Ciclo 01/2026).
* **Integrantes**:
  * Gabriela Quinteros (Diseño de Arte, Personajes y Storyboards)
  * Oscar Ayala (Formatos y Arquitectura Lógica de Juego)
  * Raúl Caprile (Diseño de Escenarios y Sistema de Progresión)
  * Fabio Mijango (Análisis de Mercado y Benchmarking)
  * Andreé Torres (Configuración de Interfaz, Controles y Marco Teórico)

## Flujo y Alcance del Juego
El juego ha sido simplificado y consta únicamente de **dos niveles** y el Menú Principal (el Nivel 1 original de unir cables ha sido descartado):
1. **Menú Principal (mainMenu)**: Permite iniciar el juego, abrir el panel de ajustes (con controles y créditos) y salir de la aplicación de forma segura.
2. **Nivel 1 (Red de Datos - File Catcher)**: Bit-Bot se mueve horizontalmente en la parte inferior para atrapar archivos válidos (azules/verdes) y esquivar virus (violetas de Glitch-Lord).
3. **Nivel 2 (Base de Datos Central - Debug Smash)**: El jugador debe hacer clic rápidamente sobre los bugs que aparecen de forma aleatoria en la pantalla antes de que se multipliquen o desaparezcan.

## Pautas de Programación en Unity (C#)
Cualquier agente de IA que trabaje en este código debe respetar las siguientes directrices:
* **Físicas y Matemáticas en 2D**: Usar componentes bidimensionales estrictamente (`Rigidbody2D`, `BoxCollider2D`, `Vector2`).
* **Estilo de Código**:
  * Utilizar la arquitectura Singleton para los gestores globales (`GameManager`, `UIManager`, `AudioManager`, `TimerController`, `LifeManager`).
  * Mantener los métodos de control limpios y documentados con comentarios claros en español.
  * Utilizar `[SerializeField] private` para exponer variables en el Inspector en lugar de variables públicas, respetando la encapsulación.
* **Estética de Pixel Art**:
  * Todos los Sprites del juego deben importarse con `Filter Mode = Point (no filter)` y `Compression = Uncompressed` para asegurar la nitidez de los bordes del pixel art en las pantallas de alta resolución.
* **Interfaz de Usuario (UI)**:
  * Utilizar el sistema de interfaz `uGUI` (Canvas, Image, Button, Text) con el paquete local configurado en el manifiesto.
  * El `CanvasScaler` de las escenas debe estar configurado en modo `ScaleWithScreenSize` con resolución de referencia `1920x1080`.
