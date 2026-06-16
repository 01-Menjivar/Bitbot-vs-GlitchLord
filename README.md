# FinalProject - Base de Videojuego Unity 2D

Este repositorio es la plantilla base para un videojuego 2D en Unity. Contiene una estructura de carpetas limpia y organizada, una configuración inicial de Git y un script básico de movimiento utilizando las teclas **WASD** a través del *Input Manager* de Unity.

## 🛠️ Especificaciones del Proyecto
* **Versión de Unity:** Unity 6 (6000.3.5f2)
* **Plantilla:** 2D
* **Control de Versiones:** Git (rama por defecto: `main`)

---

## 📁 Estructura del Proyecto
El proyecto sigue una estructura limpia dentro de la carpeta `Assets/` para organizar los recursos:

```text
Assets/
├── Art/
│   ├── Animations/   # Clips y controladores de animación (.anim, .controller)
│   ├── Backgrounds/  # Fondos y capas de paralaje
│   ├── Characters/   # Sprites y assets de personajes
│   ├── Enemies/      # Sprites y assets de enemigos
│   ├── Materials/    # Materiales 2D / Shaders
│   └── UI/           # Elementos de la interfaz de usuario (botones, paneles)
├── Audio/
│   ├── Music/        # Pistas de música de fondo (BGM)
│   └── SFX/          # Efectos de sonido cortos
├── Documentation/    # Documentación del diseño, GDD u otros
├── Fonts/            # Tipografías utilizadas en el juego
├── Scripts/          # Código fuente en C#
├── Testing/          # Escenas de prueba o scripts de pruebas unitarias
└── Tilemaps/         # Paletas de tiles y recursos de rejilla (Grid/Tilemap)
```

> [!NOTE]
> Cada carpeta contiene un archivo oculto `.gitkeep` temporal para asegurar que Git le dé seguimiento a la estructura antes de que agregues tus propios archivos.

---

## ⌨️ Sistema de Entrada (Input Manager)
El proyecto utiliza el sistema tradicional de **Input Manager** de Unity. Los ejes preconfigurados son:
* **Horizontal:** Controlado con las teclas `A` / `D` (y las flechas `Izquierda` / `Derecha`).
* **Vertical:** Controlado con las teclas `W` / `S` (y las flechas `Arriba` / `Abajo`).

### Script de Movimiento (`PlayerMovement.cs`)
En la carpeta `Assets/Scripts/` se encuentra [PlayerMovement.cs](file:///Volumes/UTM_SSD/Unity/UnityProjects/FinalProject/Assets/Scripts/PlayerMovement.cs), un script genérico y optimizado para mover personajes en 2D:

* **Compatibilidad doble:** Funciona tanto si el personaje usa física (`Rigidbody2D`) como si se mueve de forma puramente matemática (`transform.Translate` como fallback).
* **Normalización de vectores:** Evita que el personaje se mueva más rápido en diagonal.
* **Fácil de configurar:** Solo arrastra el script a tu personaje y ajusta la velocidad (`moveSpeed`) en el inspector.

---

## 🚀 Cómo subir este repositorio a GitHub
Para subir este proyecto a tu propio repositorio de GitHub, ejecuta los siguientes comandos en tu terminal dentro de la carpeta del proyecto:

1. Crea un repositorio vacío en GitHub.
2. Vincula este repositorio local con tu repositorio de GitHub:
   ```bash
   git remote add origin <URL_DE_TU_REPOSITORIO_GITHUB>
   ```
3. Sube la rama principal:
   ```bash
   git branch -M main
   # Si ya está en main, solo haz push:
   git push -u origin main
   ```
