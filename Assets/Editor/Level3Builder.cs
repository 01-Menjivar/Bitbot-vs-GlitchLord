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

        // Crear el objeto padre "Lvl3" y añadirle el controlador del nivel
        GameObject rootObj = new GameObject("Lvl3");
        rootObj.AddComponent<DebugSmashController>();

        // 2. Configurar la Cámara en 2D (Ortográfica)
        GameObject cameraObj = new GameObject("Main Camera");
        cameraObj.transform.SetParent(rootObj.transform);
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
            bgObj.transform.SetParent(rootObj.transform);
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
            monitorObj.transform.SetParent(rootObj.transform);
            monitorObj.transform.position = new Vector3(0, 0, 0);

            SpriteRenderer monitorRenderer = monitorObj.AddComponent<SpriteRenderer>();
            monitorRenderer.sprite = monitorSprite;
            monitorRenderer.sortingOrder = 5; // Dibujar sobre el fondo

            monitorObj.AddComponent<MonitorIntroAnimation>();
        }
        else
        {
            Debug.LogError("No se pudo cargar el Sprite del monitor desde: " + monitorPath);
        }

        // Añadir y configurar el BugSpawner en el objeto raíz Lvl3
        BugSpawner bugSpawner = rootObj.AddComponent<BugSpawner>();
        bugSpawner.SetSpawnPadding(2.4f, 1.6f, 3.0f);
        
        // Cargar los prefabs de los bugs desde Assets/Prefabs
        GameObject bug1 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/bug_1.prefab");
        GameObject bug2 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/bug_2.prefab");
        GameObject bug3 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/bug_3.prefab");

        if (bug1 != null && bug2 != null && bug3 != null)
        {
            bugSpawner.SetBugPrefabs(new GameObject[] { bug1, bug2, bug3 });
            Debug.Log("BugSpawner configurado en Lvl3 con los prefabs bug_1, bug_2 y bug_3.");
        }
        else
        {
            Debug.LogError("BugSpawner: No se pudieron cargar los prefabs de bugs desde Assets/Prefabs/ (¿Ejecutó 'Tools -> Create Bug Prefabs' primero?)");
        }

        // 6. Crear el GameObject de Bitbot
        string bitbotPath = "Assets/Art/Characters/Bit-Bot/bit-bot_walking.png";
        Sprite bitbotSprite = LoadSprite(bitbotPath);
        if (bitbotSprite != null)
        {
            GameObject bitbotObj = new GameObject("bitbot");
            bitbotObj.transform.SetParent(rootObj.transform);
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

        // 7. Crear el texto de error parpadeante en el centro de la pantalla
        GameObject errorTextObj = new GameObject("ErrorText");
        errorTextObj.transform.SetParent(rootObj.transform);
        errorTextObj.transform.position = new Vector3(0f, 0f, -2f); // Centrado en la pantalla

        TextMesh textMesh = errorTextObj.AddComponent<TextMesh>();
        MeshRenderer meshRenderer = errorTextObj.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 11;

        // Cargar fuente PressStart2P
        Font pressStartFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/press-start-2p/PressStart2P.ttf");
        if (pressStartFont != null)
        {
            textMesh.font = pressStartFont;
            meshRenderer.sharedMaterial = pressStartFont.material;
        }
        else
        {
            Debug.LogError("Level3Builder: No se pudo cargar la fuente PressStart2P.ttf en Assets/Fonts/press-start-2p/");
        }

        textMesh.text = "ERROR DE NUCLEO\nEN LA BASE DE DATOS";
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.fontSize = 80;
        textMesh.characterSize = 0.08f; // Nitidez para fuentes pixel art en 2D
        textMesh.color = Color.red;

        // Crear el borde negro para el texto (outline)
        CreateTextOutline(errorTextObj, textMesh, pressStartFont);

        // 7b. Crear el texto del contador (CountdownText)
        GameObject countdownTextObj = new GameObject("CountdownText");
        countdownTextObj.transform.SetParent(rootObj.transform);
        countdownTextObj.transform.position = new Vector3(0f, 0f, -2f); // Centrado en la pantalla
        countdownTextObj.SetActive(false); // Inicialmente inactivo

        TextMesh countdownMesh = countdownTextObj.AddComponent<TextMesh>();
        MeshRenderer countdownRenderer = countdownTextObj.GetComponent<MeshRenderer>();
        countdownRenderer.sortingOrder = 11;

        if (pressStartFont != null)
        {
            countdownMesh.font = pressStartFont;
            countdownRenderer.sharedMaterial = pressStartFont.material;
        }

        countdownMesh.text = "3";
        countdownMesh.alignment = TextAlignment.Center;
        countdownMesh.anchor = TextAnchor.MiddleCenter;
        countdownMesh.fontSize = 120; // Un poco más grande para el contador
        countdownMesh.characterSize = 0.08f; // Nitidez para pixel art
        countdownMesh.color = Color.white;

        // Crear el borde negro para el texto del contador
        CreateTextOutline(countdownTextObj, countdownMesh, pressStartFont);

        // 8. Guardar la escena en Assets/Scenes/
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

    private static void CreateTextOutline(GameObject parentTextObj, TextMesh parentTextMesh, Font font)
    {
        float offset = 0.035f; // Ancho del borde en coordenadas del mundo
        Vector3[] offsets = {
            new Vector3(-offset, -offset, 0.005f),
            new Vector3(offset, -offset, 0.005f),
            new Vector3(-offset, offset, 0.005f),
            new Vector3(offset, offset, 0.005f),
            new Vector3(0f, -offset, 0.005f),
            new Vector3(0f, offset, 0.005f),
            new Vector3(-offset, 0f, 0.005f),
            new Vector3(offset, 0f, 0.005f)
        };

        for (int i = 0; i < offsets.Length; i++)
        {
            GameObject outlineObj = new GameObject("Outline_" + i);
            outlineObj.transform.SetParent(parentTextObj.transform);
            outlineObj.transform.localPosition = offsets[i];
            outlineObj.transform.localRotation = Quaternion.identity;
            outlineObj.transform.localScale = Vector3.one;

            TextMesh outlineTM = outlineObj.AddComponent<TextMesh>();
            MeshRenderer mr = outlineObj.GetComponent<MeshRenderer>();
            
            // Copiar el sortingOrder del padre, restándole 1 para que quede justo detrás del texto principal
            MeshRenderer parentMR = parentTextObj.GetComponent<MeshRenderer>();
            if (parentMR != null)
            {
                mr.sortingOrder = parentMR.sortingOrder - 1;
            }
            
            if (font != null)
            {
                outlineTM.font = font;
                mr.sharedMaterial = font.material;
            }

            outlineTM.text = parentTextMesh.text;
            outlineTM.alignment = parentTextMesh.alignment;
            outlineTM.anchor = parentTextMesh.anchor;
            outlineTM.fontSize = parentTextMesh.fontSize;
            outlineTM.characterSize = parentTextMesh.characterSize;
            outlineTM.color = Color.black; // Color negro para el borde
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
