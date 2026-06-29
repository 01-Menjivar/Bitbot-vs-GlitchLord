using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class SetupLevel3Pause : MonoBehaviour
{
    [MenuItem("Tools/Setup Pause UI Level 3")]
    public static void SetupPause()
    {
        string scenePath = "Assets/Scenes/Level3.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        EnsureSprite("Assets/Art/UI/Buttons/button_pausa.png");
        EnsureSprite("Assets/Art/Backgrounds/pausa_level3.png");
        EnsureSprite("Assets/Art/Characters/Directora Ctrl/directora_neutral.png");
        EnsureSprite("Assets/Art/Characters/Bit-Bot/bit-bot_fronta.png");
        EnsureSprite("Assets/Art/UI/Buttons/button_resume.png");
        EnsureSprite("Assets/Art/UI/HUD/HUD_Mensaje.png");
        EnsureSprite("Assets/Resources/UI/Buttons/Button_Exit.png");

        GameObject oldCanvas = GameObject.Find("PauseCanvas");
        if (oldCanvas != null) DestroyImmediate(oldCanvas);

        // 1. Crear Canvas de Pausa
        GameObject canvasObj = new GameObject("PauseCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32000; 

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        PauseController controller = canvasObj.AddComponent<PauseController>();

        // 2. Boton HUD Pausa
        GameObject pauseBtnObj = new GameObject("PauseButton");
        pauseBtnObj.transform.SetParent(canvasObj.transform, false);
        Image btnImg = pauseBtnObj.AddComponent<Image>();
        btnImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/Buttons/button_pausa.png");
        btnImg.SetNativeSize();
        pauseBtnObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        Button btnComp = pauseBtnObj.AddComponent<Button>();
        RectTransform btnRect = pauseBtnObj.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(1, 1);
        btnRect.anchorMax = new Vector2(1, 1);
        btnRect.pivot = new Vector2(1, 1);
        btnRect.anchoredPosition = new Vector2(-30, -30);

        // 3. Panel de Pausa
        GameObject pausePanelObj = new GameObject("PausePanel");
        pausePanelObj.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = pausePanelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        Image solidBg = pausePanelObj.AddComponent<Image>();
        solidBg.color = Color.black;

        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(pausePanelObj.transform, false);
        Image panelImg = bgObj.AddComponent<Image>();
        panelImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/pausa_level3.png");
        panelImg.type = Image.Type.Simple;
        panelImg.preserveAspect = false; 
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // 4. Agregar Directora
        GameObject directoraObj = new GameObject("Directora");
        directoraObj.transform.SetParent(pausePanelObj.transform, false);
        Image dirImg = directoraObj.AddComponent<Image>();
        dirImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Directora Ctrl/directora_neutral.png");
        dirImg.preserveAspect = true;
        RectTransform dirRect = directoraObj.GetComponent<RectTransform>();
        dirRect.anchorMin = new Vector2(0.1f, 0.05f);
        dirRect.anchorMax = new Vector2(0.4f, 0.65f);
        dirRect.sizeDelta = Vector2.zero;
        dirRect.anchoredPosition = Vector2.zero;

        // 5. Agregar Bitbot
        GameObject bitbotObj = new GameObject("Bitbot");
        bitbotObj.transform.SetParent(pausePanelObj.transform, false);
        Image bbImg = bitbotObj.AddComponent<Image>();
        bbImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Bit-Bot/bit-bot_fronta.png");
        bbImg.preserveAspect = true;
        RectTransform bbRect = bitbotObj.GetComponent<RectTransform>();
        bbRect.anchorMin = new Vector2(0.6f, 0.05f);
        bbRect.anchorMax = new Vector2(0.9f, 0.65f);
        bbRect.sizeDelta = Vector2.zero;
        bbRect.anchoredPosition = Vector2.zero;

        // 6. Caja de Diálogo
        GameObject dialogBox = new GameObject("DialogBox");
        dialogBox.transform.SetParent(pausePanelObj.transform, false);
        Image dbImg = dialogBox.AddComponent<Image>();
        Sprite hudMsgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/HUD/HUD_Mensaje.png");
        if (hudMsgSprite != null)
        {
            dbImg.sprite = hudMsgSprite;
            dbImg.type = Image.Type.Simple; 
            dbImg.color = Color.white;
            dbImg.SetNativeSize();
        }
        RectTransform dbRect = dialogBox.GetComponent<RectTransform>();
        // Anclado centro-arriba matemáticamente exacto
        dbRect.anchorMin = new Vector2(0.5f, 1f); 
        dbRect.anchorMax = new Vector2(0.5f, 1f);
        dbRect.pivot = new Vector2(0.5f, 1f);
        dbRect.anchoredPosition = new Vector2(0, -30); // 30 px del techo
        dialogBox.transform.localScale = new Vector3(1.3f, 1.3f, 1f);

        // 7. Texto del diálogo
        GameObject textObj = new GameObject("DialogText");
        textObj.transform.SetParent(dialogBox.transform, false);
        Text txt = textObj.AddComponent<Text>();
        // Texto original sin saltos de línea manuales
        txt.text = "Bitbot, la Base de Datos Central está saturada de virus creados por Glitch-Lord. Elimínalos rápidamente haciendo clic sobre ellos. ¡No dejes que la barra de infección llegue al 100% o el sistema colapsará!";
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 24; 
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        txt.verticalOverflow = VerticalWrapMode.Truncate;
        RectTransform txtRect = textObj.GetComponent<RectTransform>();
        // Stretch total al HUD original, pero con offsets precisos en píxeles para encajar en el cristal interior
        txtRect.anchorMin = new Vector2(0f, 0f);
        txtRect.anchorMax = new Vector2(1f, 1f);
        txtRect.offsetMin = new Vector2(80, 40);   // Izquierda y abajo
        txtRect.offsetMax = new Vector2(-80, -90); // Derecha y arriba (evita título y marcos)

        // 8. Botón de Reanudar
        GameObject resumeBtnObj = new GameObject("ResumeButton");
        resumeBtnObj.transform.SetParent(pausePanelObj.transform, false);
        Image resImg = resumeBtnObj.AddComponent<Image>();
        Sprite resSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/Buttons/button_resume.png");
        if (resSprite != null)
        {
            resImg.sprite = resSprite;
            resImg.SetNativeSize();
            // Acción principal equilibrada: Destacada pero no exagerada
            resumeBtnObj.transform.localScale = new Vector3(0.95f, 0.95f, 1f); 
        }
        Button resBtn = resumeBtnObj.AddComponent<Button>();
        RectTransform resRect = resumeBtnObj.GetComponent<RectTransform>();
        // Centrar de forma más prominente en la zona inferior
        resRect.anchorMin = new Vector2(0.5f, 0.38f);
        resRect.anchorMax = new Vector2(0.5f, 0.38f);
        resRect.pivot = new Vector2(0.5f, 0.5f);

        // 9. Botón de Salir (Exit)
        GameObject exitBtnObj = new GameObject("ExitButton");
        exitBtnObj.transform.SetParent(pausePanelObj.transform, false);
        Image exitImg = exitBtnObj.AddComponent<Image>();
        Sprite exitSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/UI/Buttons/Button_Exit.png");
        if (exitSprite != null)
        {
            exitImg.sprite = exitSprite;
            exitImg.SetNativeSize();
            // Acción secundaria equilibrada: Legible pero discreta
            exitBtnObj.transform.localScale = new Vector3(0.45f, 0.45f, 1f); 
        }
        else 
        {
            exitImg.color = Color.red; // Fallback visual
            exitBtnObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);
        }
        Button exitBtn = exitBtnObj.AddComponent<Button>();
        RectTransform exitRect = exitBtnObj.GetComponent<RectTransform>();
        // Posicionar más abajo de forma discreta
        exitRect.anchorMin = new Vector2(0.5f, 0.12f);
        exitRect.anchorMax = new Vector2(0.5f, 0.12f);
        exitRect.pivot = new Vector2(0.5f, 0.5f);

        // Asignar referencias al controlador
        controller.pausePanel = pausePanelObj;
        controller.pauseButton = btnComp;
        controller.resumeButton = resBtn;
        controller.exitButton = exitBtn;

        pausePanelObj.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[Setup] Interfaz de Pausa FINAL con padding ajustado.");
    }

    private static void EnsureSprite(string path)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            bool modified = false;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                modified = true;
            }
            if (importer.alphaIsTransparency == false)
            {
                importer.alphaIsTransparency = true;
                modified = true;
            }
            if (modified)
            {
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }

    [MenuItem("Tools/Setup MainMenu Managers and Audio")]
    public static void SetupManagers()
    {
        string scenePath = "Assets/Scenes/MainMenuScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        // 1. Encontrar o crear _Managers
        GameObject managersObj = GameObject.Find("_Managers");
        if (managersObj == null)
        {
            managersObj = new GameObject("_Managers");
        }

        // 2. Configurar GameManager como hijo
        GameObject gmObj = GameObject.Find("_Managers/GameManager");
        if (gmObj == null) gmObj = new GameObject("GameManager");
        gmObj.transform.SetParent(managersObj.transform, false);
        if (gmObj.GetComponent<GameManager>() == null) gmObj.AddComponent<GameManager>();

        // 3. Configurar UIManager como hijo
        GameObject uiObj = GameObject.Find("_Managers/UIManager");
        if (uiObj == null) uiObj = new GameObject("UIManager");
        uiObj.transform.SetParent(managersObj.transform, false);
        UIManager uiManager = uiObj.GetComponent<UIManager>();
        if (uiManager == null) uiManager = uiObj.AddComponent<UIManager>();
        
        // Asignar los sprites de vida en el UIManager usando SerializedObject por encapsulación
        SerializedObject serializedUI = new SerializedObject(uiManager);
        serializedUI.FindProperty("coreActiveSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/HUD/HUD_ProcessorCore_Active.png");
        serializedUI.FindProperty("coreOfflineSprite").objectReferenceValue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/HUD/HUD_ProcessorCore_Offline.png");
        serializedUI.ApplyModifiedProperties();

        // 4. Configurar LifeManager como hijo
        GameObject lmObj = GameObject.Find("_Managers/LifeManager");
        if (lmObj == null) lmObj = new GameObject("LifeManager");
        lmObj.transform.SetParent(managersObj.transform, false);
        if (lmObj.GetComponent<LifeManager>() == null) lmObj.AddComponent<LifeManager>();

        // 5. Configurar AudioManager como hijo
        GameObject audioObj = GameObject.Find("_Managers/AudioManager");
        if (audioObj == null) audioObj = new GameObject("AudioManager");
        audioObj.transform.SetParent(managersObj.transform, false);
        AudioManager audioManager = audioObj.GetComponent<AudioManager>();
        if (audioManager == null) audioManager = audioObj.AddComponent<AudioManager>();

        // Asignar clips de sonido al AudioManager desde Assets/Audio/
        audioManager.menuTheme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/MenuTheme.mp3");
        audioManager.level1Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level1Theme.mp3");
        audioManager.level2Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level2Theme.mp3");
        audioManager.level3Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level3Theme.mp3");

        audioManager.sfxClick = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/click1.ogg");
        audioManager.sfxError = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/interference1.wav");
        audioManager.sfxFileCaught = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/points1.wav");
        audioManager.sfxVirusHit = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/lose.wav");
        audioManager.sfxBugDestroyed = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/pop.wav");
        audioManager.sfxVictory = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/happy.wav");
        audioManager.sfxGameOver = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/wawawawa.wav");

        EditorUtility.SetDirty(uiObj);
        EditorUtility.SetDirty(audioObj);
        EditorUtility.SetDirty(managersObj);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[Setup] Managers y AudioManager configurados con éxito en MainMenuScene.");
    }

    [MenuItem("Tools/Configure Audio in Active Scene")]
    public static void ConfigureActiveSceneAudio()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("No se encontró ningún AudioManager en la escena activa.");
            return;
        }

        audioManager.menuTheme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/MenuTheme.mp3");
        audioManager.level1Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level1Theme.mp3");
        audioManager.level2Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level2Theme.mp3");
        audioManager.level3Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level3Theme.mp3");

        audioManager.sfxClick = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/click1.ogg");
        audioManager.sfxError = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/interference1.wav");
        audioManager.sfxFileCaught = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/points1.wav");
        audioManager.sfxVirusHit = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/lose.wav");
        audioManager.sfxBugDestroyed = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/pop.wav");
        audioManager.sfxVictory = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/happy.wav");
        audioManager.sfxGameOver = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/wawawawa.wav");

        EditorUtility.SetDirty(audioManager.gameObject);
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        Debug.Log("[Setup] AudioManager de la escena activa configurado con éxito.");
    }

    [MenuItem("Tools/Setup Audio in All Scenes")]
    public static void SetupAudioInAllScenes()
    {
        string[] scenes = {
            "Assets/Scenes/MainMenuScene.unity",
            "Assets/Scenes/LevelSelectionScene.unity",
            "Assets/Scenes/Level2.unity",
            "Assets/Scenes/Level3.unity",
            "Assets/Scenes/GameOverScreen.unity",
            "Assets/Scenes/VictoryScreen.unity"
        };

        foreach (string scenePath in scenes)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                audioManager.menuTheme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/MenuTheme.mp3");
                audioManager.level1Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level1Theme.mp3");
                audioManager.level2Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level2Theme.mp3");
                audioManager.level3Theme = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Music/Level3Theme.mp3");

                audioManager.sfxClick = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/click1.ogg");
                audioManager.sfxError = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/interference1.wav");
                audioManager.sfxFileCaught = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/points1.wav");
                audioManager.sfxVirusHit = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/lose.wav");
                audioManager.sfxBugDestroyed = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/UI/pop.wav");
                audioManager.sfxVictory = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/Bitbot/happy.wav");
                audioManager.sfxGameOver = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/SFX/General/wawawawa.wav");

                EditorUtility.SetDirty(audioManager.gameObject);
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                Debug.Log($"[Setup] AudioManager configurado con éxito en: {scenePath}");
            }
        }
    }
}
