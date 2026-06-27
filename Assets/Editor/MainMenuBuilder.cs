using UnityEngine;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuBuilder : EditorWindow
{
    [MenuItem("Tools/Build Main Menu Scene")]
    public static void BuildMainMenuScene()
    {
        // Forzar al motor a escanear el disco y sincronizar la base de datos de assets
        AssetDatabase.Refresh();

        // 1. Crear una nueva escena vacía
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        
        // 2. Configurar la Cámara en 2D (Ortográfica)
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        cameraObj.tag = "MainCamera";
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.backgroundColor = new Color(0.043f, 0.059f, 0.098f); // Color azul oscuro corporativo (#0B0F19)
        camera.clearFlags = CameraClearFlags.SolidColor;
        cameraObj.AddComponent<AudioListener>();
        
        // 3. Crear el GameObject del Controlador del Menú
        GameObject controllerObj = new GameObject("MainMenuController");
        MainMenuController controller = controllerObj.AddComponent<MainMenuController>();
        
        // 4. Crear el Canvas de Interfaz y el EventSystem
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        // Configurar CanvasScaler para escalar proporcionalmente con la resolución de pantalla
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f); // Resolución base Full HD
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f; // Equilibrio entre ancho y alto
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        
        // 5. Asegurar que las imágenes estén configuradas como Sprites 2D y cargarlas
        string[] texturePaths = new string[]
        {
            "Assets/Art/UI/Logos/Logo_BitBotVsGlitchLord.png",
            "Assets/Art/UI/Buttons/Button_Play.png",
            "Assets/Art/UI/Buttons/Button_Settings.png",
            "Assets/Art/UI/Buttons/Button_Exit.png",
            "Assets/Art/UI/Buttons/Button_Back.png"
        };

        foreach (string path in texturePaths)
        {
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                if (importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.SaveAndReimport();
                }
                // Forzar la importación síncrona para asegurar que esté listo inmediatamente
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        Sprite logoSprite = LoadSprite("Assets/Art/UI/Logos/Logo_BitBotVsGlitchLord.png");
        Sprite playSprite = LoadSprite("Assets/Art/UI/Buttons/Button_Play.png");
        Sprite settingsSprite = LoadSprite("Assets/Art/UI/Buttons/Button_Settings.png");
        Sprite exitSprite = LoadSprite("Assets/Art/UI/Buttons/Button_Exit.png");
        Sprite backSprite = LoadSprite("Assets/Art/UI/Buttons/Button_Back.png");

        // Diagnóstico en consola para verificar si los Sprites se cargan con éxito
        Debug.Log("SceneBuilder Diagnóstico de Sprites (con fallback síncrono en memoria):");
        Debug.Log("- Logo Sprite: " + (logoSprite != null ? logoSprite.name : "NULO (Error al cargar)"));
        Debug.Log("- Play Sprite: " + (playSprite != null ? playSprite.name : "NULO (Error al cargar)"));
        Debug.Log("- Settings Sprite: " + (settingsSprite != null ? settingsSprite.name : "NULO (Error al cargar)"));
        Debug.Log("- Exit Sprite: " + (exitSprite != null ? exitSprite.name : "NULO (Error al cargar)"));
        Debug.Log("- Back Sprite: " + (backSprite != null ? backSprite.name : "NULO (Error al cargar)"));
        
        // 6. Crear el Logotipo en el Canvas
        GameObject logoObj = new GameObject("Logo");
        logoObj.transform.SetParent(canvasObj.transform, false);
        Image logoImage = logoObj.AddComponent<Image>();
        logoImage.sprite = logoSprite;
        logoImage.preserveAspect = true;
        logoImage.SetNativeSize(); // Obtiene la resolución nativa exacta de la imagen
        RectTransform logoRect = logoObj.GetComponent<RectTransform>();
        logoRect.sizeDelta = logoRect.sizeDelta * 0.5f; // Reduce a la mitad conservando el aspect ratio perfecto
        logoRect.anchoredPosition = new Vector2(0f, 220f);
        
        // 7. Crear el contenedor para los Botones
        GameObject menuButtonsObj = new GameObject("MenuButtons");
        menuButtonsObj.transform.SetParent(canvasObj.transform, false);
        RectTransform menuButtonsRect = menuButtonsObj.AddComponent<RectTransform>();
        menuButtonsRect.anchoredPosition = new Vector2(0f, -80f);
        
        // --- BOTÓN JUGAR ---
        GameObject playBtnObj = new GameObject("Button_Play");
        playBtnObj.transform.SetParent(menuButtonsObj.transform, false);
        Image playImage = playBtnObj.AddComponent<Image>();
        playImage.sprite = playSprite;
        playImage.preserveAspect = true;
        playImage.SetNativeSize();
        Button playButton = playBtnObj.AddComponent<Button>();
        RectTransform playRect = playBtnObj.GetComponent<RectTransform>();
        playRect.sizeDelta = playRect.sizeDelta * 0.4f; // Escala proporcional del 40% del tamaño nativo
        playRect.anchoredPosition = new Vector2(0f, 160f);
        UnityEventTools.AddPersistentListener(playButton.onClick, new UnityEngine.Events.UnityAction(controller.PlayGame));
        
        // --- BOTÓN AJUSTES ---
        GameObject settingsBtnObj = new GameObject("Button_Settings");
        settingsBtnObj.transform.SetParent(menuButtonsObj.transform, false);
        Image settingsImage = settingsBtnObj.AddComponent<Image>();
        settingsImage.sprite = settingsSprite;
        settingsImage.preserveAspect = true;
        settingsImage.SetNativeSize();
        Button settingsButton = settingsBtnObj.AddComponent<Button>();
        RectTransform settingsRect = settingsBtnObj.GetComponent<RectTransform>();
        settingsRect.sizeDelta = settingsRect.sizeDelta * 0.4f; // Escala proporcional del 40% del tamaño nativo
        settingsRect.anchoredPosition = new Vector2(0f, 0f);
        UnityEventTools.AddPersistentListener(settingsButton.onClick, new UnityEngine.Events.UnityAction(controller.OpenSettings));
        
        // --- BOTÓN SALIR ---
        GameObject exitBtnObj = new GameObject("Button_Exit");
        exitBtnObj.transform.SetParent(menuButtonsObj.transform, false);
        Image exitImage = exitBtnObj.AddComponent<Image>();
        exitImage.sprite = exitSprite;
        exitImage.preserveAspect = true;
        exitImage.SetNativeSize();
        Button exitButton = exitBtnObj.AddComponent<Button>();
        RectTransform exitRect = exitBtnObj.GetComponent<RectTransform>();
        exitRect.sizeDelta = exitRect.sizeDelta * 0.4f; // Escala proporcional del 40% del tamaño nativo
        exitRect.anchoredPosition = new Vector2(0f, -160f);
        UnityEventTools.AddPersistentListener(exitButton.onClick, new UnityEngine.Events.UnityAction(controller.QuitGame));
        
        // 8. Crear el Panel de Ajustes (Settings Panel)
        GameObject settingsPanelObj = new GameObject("SettingsPanel");
        settingsPanelObj.transform.SetParent(canvasObj.transform, false);
        Image panelImage = settingsPanelObj.AddComponent<Image>();
        panelImage.color = new Color(0.07f, 0.11f, 0.18f, 0.95f); // Fondo azul oscuro opaco
        RectTransform panelRect = settingsPanelObj.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(500f, 350f);
        panelRect.anchoredPosition = Vector2.zero;
        
        // Texto de información en Ajustes
        GameObject settingsTextObj = new GameObject("SettingsText");
        settingsTextObj.transform.SetParent(settingsPanelObj.transform, false);
        Text settingsText = settingsTextObj.AddComponent<Text>();
        settingsText.text = "SISTEMA CONFIGURADO\n\nDesarrollado por: Equipo Pepsiman\n\nCONTROLES:\nTeclado: WASD / Flechas para moverse\nRatón: Clic e Interacciones\n\nObjetivo: ¡Evita los virus de Glitch-Lord y salva a MegaCorp!";
        settingsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        settingsText.fontSize = 18;
        settingsText.alignment = TextAnchor.MiddleCenter;
        settingsText.color = Color.white;
        RectTransform textRect = settingsTextObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(440f, 240f);
        textRect.anchoredPosition = new Vector2(0f, 30f);
        
        // Botón Regresar (Cerrar Ajustes)
        GameObject backBtnObj = new GameObject("Button_Back");
        backBtnObj.transform.SetParent(settingsPanelObj.transform, false);
        Image backImage = backBtnObj.AddComponent<Image>();
        backImage.sprite = backSprite;
        backImage.preserveAspect = true;
        Button backButton = backBtnObj.AddComponent<Button>();
        RectTransform backRect = backBtnObj.GetComponent<RectTransform>();
        backRect.sizeDelta = new Vector2(180f, 50f);
        backRect.anchoredPosition = new Vector2(0f, -120f);
        UnityEventTools.AddPersistentListener(backButton.onClick, new UnityEngine.Events.UnityAction(controller.CloseSettings));
        
        // Vincular el panel de ajustes al script del controlador de forma persistente
        SerializedObject serializedController = new SerializedObject(controller);
        serializedController.FindProperty("settingsPanel").objectReferenceValue = settingsPanelObj;
        serializedController.ApplyModifiedProperties();
        
        // Desactivar el panel de ajustes por defecto
        settingsPanelObj.SetActive(false);
        
        // 9. Guardar la escena en Assets/Scenes/
        string scenePath = "Assets/Scenes/MainMenuScene.unity";
        bool saveSuccess = EditorSceneManager.SaveScene(newScene, scenePath);
        
        if (saveSuccess)
        {
            // 10. Registrar automáticamente las escenas en la configuración de compilación de Unity (Build Settings)
            RegisterScenesInBuildSettings(scenePath);
            
            Debug.Log("MainMenuScene creada y guardada con éxito en: " + scenePath);
            EditorUtility.DisplayDialog("Éxito", "La escena del Menú Principal fue construida con éxito y añadida a los Build Settings.", "Aceptar");
        }
        else
        {
            Debug.LogError("Error al intentar guardar la escena del Menú Principal.");
        }
    }
    
    private static void RegisterScenesInBuildSettings(string mainMenuPath)
    {
        var scenes = new List<EditorBuildSettingsScene>();
        
        // Registrar escena del menú principal en el índice 0
        scenes.Add(new EditorBuildSettingsScene(mainMenuPath, true));
        
        // Rutas teóricas para los dos minijuegos (se agregarán como activas para que SceneManager pueda cargarlas)
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Level1.unity", true));
        scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Level2.unity", true));
        
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log("Escenas registradas en Build Settings: MainMenuScene (0), Level1 (1), Level2 (2)");
    }

    private static Sprite LoadSprite(string path)
    {
        string guid = AssetDatabase.AssetPathToGUID(path);
        Debug.Log("LoadSprite: Cargando " + path + " | GUID en AssetDatabase: " + (string.IsNullOrEmpty(guid) ? "NO ENCONTRADO" : guid));

        // 1. Configurar el importador de texturas para Pixel Art (Filtro Point y sin compresión)
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            bool needsReimport = false;

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                needsReimport = true;
            }
            if (importer.filterMode != FilterMode.Point)
            {
                importer.filterMode = FilterMode.Point; // Mantiene los bordes de pixel art perfectamente afilados y nítidos
                needsReimport = true;
            }
            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed; // Evita artefactos borrosos de compresión
                needsReimport = true;
            }

            if (needsReimport)
            {
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        // 2. Intentar cargar como Sprite directamente
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite != null) return sprite;

        // 3. Si falla, cargar como Texture2D y crear el Sprite en memoria
        Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (texture != null)
        {
            Sprite createdSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            createdSprite.name = texture.name;
            return createdSprite;
        }

        return null;
    }
}
