using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class Level3Builder : EditorWindow
{
    [MenuItem("Tools/Build Level 3 Scene")]
    public static void BuildLevel3Scene()
    {
        // Forzar la actualización de la base de datos de assets
        AssetDatabase.Refresh();

        // 1. Crear una nueva escena vacía
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 2. Configurar la Cámara en 2D (Ortográfica)
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        cameraObj.tag = "MainCamera";
        camera.orthographic = true;
        camera.orthographicSize = 5f; // Valor inicial por defecto
        camera.backgroundColor = new Color(0.043f, 0.059f, 0.098f); // Color azul oscuro de fondo
        camera.clearFlags = CameraClearFlags.SolidColor;
        cameraObj.AddComponent<AudioListener>();
        cameraObj.transform.position = new Vector3(0, 0, -10f);

        // 3. Asegurar la importación e importar el Sprite del fondo de Level 3
        string bgPath = "Assets/Art/Backgrounds/level 3/level 3.png";
        Sprite bgSprite = LoadSprite(bgPath);

        // 4. Crear el GameObject del fondo y ajustar la cámara
        if (bgSprite != null)
        {
            GameObject bgObj = new GameObject("Background_Level3");
            SpriteRenderer spriteRenderer = bgObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = bgSprite;
            bgObj.transform.position = new Vector3(0, 0, 0);

            // Ajustar el tamaño de la cámara para que coincida verticalmente con el sprite del fondo
            float spriteHeightInUnits = bgSprite.rect.height / bgSprite.pixelsPerUnit;
            camera.orthographicSize = spriteHeightInUnits / 2f;
            Debug.Log("Set camera orthographicSize to: " + camera.orthographicSize + " | height units: " + spriteHeightInUnits);
        }
        else
        {
            Debug.LogError("No se pudo cargar el Sprite de fondo para Level 3 desde: " + bgPath);
        }

        // 5. Crear el GameObject del Monitor
        string monitorPath = "Assets/Art/Materials/Objetos/monitor.png";
        Sprite monitorSprite = LoadSprite(monitorPath);
        if (monitorSprite != null)
        {
            GameObject monitorObj = new GameObject("Monitor");
            monitorObj.transform.position = new Vector3(0, 0, 0);

            SpriteRenderer monitorRenderer = monitorObj.AddComponent<SpriteRenderer>();
            monitorRenderer.sprite = monitorSprite;
            monitorRenderer.sortingOrder = 5; // Dibujar sobre el fondo

            monitorObj.AddComponent<MonitorIntroAnimation>();
            monitorObj.AddComponent<DebugSmashController>();
        }
        else
        {
            Debug.LogError("No se pudo cargar el Sprite del monitor desde: " + monitorPath);
        }

        // 6. Crear el GameObject de Bitbot
        string bitbotPath = "Assets/Art/Characters/Bit-Bot/bit-bot_walking.png";
        Sprite bitbotSprite = LoadSprite(bitbotPath);
        if (bitbotSprite != null)
        {
            GameObject bitbotObj = new GameObject("bitbot");
            bitbotObj.transform.position = new Vector3(-10f, -5f, 0f);

            SpriteRenderer bitbotRenderer = bitbotObj.AddComponent<SpriteRenderer>();
            bitbotRenderer.sprite = bitbotSprite;
            bitbotRenderer.sortingOrder = 10; // Dibujar sobre el monitor (que es 5)

            bitbotObj.AddComponent<FloatAnimation>();
        }
        else
        {
            Debug.LogError("No se pudo cargar el Sprite de Bitbot desde: " + bitbotPath);
        }

        // 7. Guardar la escena en Assets/Scenes/
        string scenePath = "Assets/Scenes/Level3.unity";
        bool saveSuccess = EditorSceneManager.SaveScene(newScene, scenePath);

        if (saveSuccess)
        {
            // 8. Registrar la escena en Build Settings
            RegisterSceneInBuildSettings(scenePath);

            Debug.Log("Escena Level3 creada y guardada con éxito en: " + scenePath);
        }
        else
        {
            Debug.LogError("Error al intentar guardar la escena de Level 3.");
        }
    }

    private static void RegisterSceneInBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> currentScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        
        bool alreadyExists = false;
        foreach (var scene in currentScenes)
        {
            if (scene.path == scenePath)
            {
                alreadyExists = true;
                scene.enabled = true;
                break;
            }
        }

        if (!alreadyExists)
        {
            currentScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }

        EditorBuildSettings.scenes = currentScenes.ToArray();
        Debug.Log("Registrada la escena " + scenePath + " en Build Settings.");
    }

    private static Sprite LoadSprite(string path)
    {
        string guid = AssetDatabase.AssetPathToGUID(path);
        Debug.Log("LoadSprite: Cargando " + path + " | GUID: " + (string.IsNullOrEmpty(guid) ? "NO ENCONTRADO" : guid));

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            bool needsReimport = false;

            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                needsReimport = true;
            }
            if (importer.spriteImportMode != SpriteImportMode.Single)
            {
                importer.spriteImportMode = SpriteImportMode.Single;
                needsReimport = true;
            }
            if (importer.filterMode != FilterMode.Point)
            {
                importer.filterMode = FilterMode.Point;
                needsReimport = true;
            }
            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                needsReimport = true;
            }

            if (needsReimport)
            {
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite != null) return sprite;

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
