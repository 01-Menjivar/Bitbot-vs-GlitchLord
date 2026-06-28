using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class SetupLevel2UI : MonoBehaviour
{
    [MenuItem("Tools/Setup UI Level 2")]
    public static void SetupUI()
    {
        string scenePath = "Assets/Scenes/Level2.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        EnsureSprite("Assets/Art/UI/Buttons/button_pausa.png");
        EnsureSprite("Assets/Art/Backgrounds/pausa_level2.png");
        EnsureSprite("Assets/Art/Backgrounds/level 2/level2.png");
        EnsureSprite("Assets/Art/Characters/Directora Ctrl/directora_hablando.png");
        EnsureSprite("Assets/Art/Characters/Bit-Bot/bit-bot_sad.png");
        EnsureSprite("Assets/Art/Characters/Bit-Bot/bit-bot_fronta.png");
        EnsureSprite("Assets/Art/Characters/Glitch-Lord/glith-lord_riendose.png");
        EnsureSprite("Assets/Art/Enemies/Virus/Idle_Virus.png");
        EnsureSprite("Assets/Art/UI/Buttons/button_resume.png");
        EnsureSprite("Assets/Resources/UI/Buttons/Button_Exit.png");
        EnsureSprite("Assets/Resources/UI/Buttons/Button_Continue.png");
        EnsureSprite("Assets/Art/UI/HUD/HUD_Mensaje.png");

        GameObject oldCanvas = GameObject.Find("PauseCanvas");
        if (oldCanvas != null) DestroyImmediate(oldCanvas);
        GameObject oldIntro = GameObject.Find("IntroCanvas");
        if (oldIntro != null) DestroyImmediate(oldIntro);

        // --- PAUSE CANVAS ---
        GameObject canvasObj = new GameObject("PauseCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32000; 

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        PauseController controller = canvasObj.AddComponent<PauseController>();

        // Boton HUD Pausa
        GameObject pauseBtnObj = new GameObject("PauseButton");
        pauseBtnObj.transform.SetParent(canvasObj.transform, false);
        Image btnImg = pauseBtnObj.AddComponent<Image>();
        btnImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/Buttons/button_pausa.png");
        btnImg.SetNativeSize();
        pauseBtnObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        Button btnComp = pauseBtnObj.AddComponent<Button>();
        RectTransform pBtnRect = pauseBtnObj.GetComponent<RectTransform>();
        pBtnRect.anchorMin = new Vector2(0f, 1f); pBtnRect.anchorMax = new Vector2(0f, 1f);
        pBtnRect.pivot = new Vector2(0f, 1f); pBtnRect.anchoredPosition = new Vector2(250f, -50f);

        // Panel de Pausa
        GameObject pausePanelObj = new GameObject("PausePanel");
        pausePanelObj.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = pausePanelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero; panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        Image solidBg = pausePanelObj.AddComponent<Image>();
        solidBg.color = Color.black;

        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(pausePanelObj.transform, false);
        Image panelImg = bgObj.AddComponent<Image>();
        panelImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/pausa_level2.png");
        panelImg.type = Image.Type.Simple;
        panelImg.preserveAspect = false; 
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero; bgRect.anchorMax = Vector2.one; bgRect.sizeDelta = Vector2.zero;

        // Bitbot (Izquierda en Nivel 2)
        GameObject bitbotObj = new GameObject("Bitbot");
        bitbotObj.transform.SetParent(pausePanelObj.transform, false);
        Image bbImg = bitbotObj.AddComponent<Image>();
        bbImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Bit-Bot/bit-bot_sad.png");
        bbImg.preserveAspect = true;
        RectTransform bbRect = bitbotObj.GetComponent<RectTransform>();
        bbRect.anchorMin = new Vector2(0.1f, 0.05f);
        bbRect.anchorMax = new Vector2(0.4f, 0.65f);
        bbRect.sizeDelta = Vector2.zero; bbRect.anchoredPosition = Vector2.zero;

        // Directora (Derecha en Nivel 2)
        GameObject directoraObj = new GameObject("Directora");
        directoraObj.transform.SetParent(pausePanelObj.transform, false);
        Image dirImg = directoraObj.AddComponent<Image>();
        dirImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Directora Ctrl/directora_hablando.png");
        dirImg.preserveAspect = true;
        RectTransform dirRect = directoraObj.GetComponent<RectTransform>();
        dirRect.anchorMin = new Vector2(0.6f, 0.05f);
        dirRect.anchorMax = new Vector2(0.9f, 0.65f);
        dirRect.sizeDelta = Vector2.zero; dirRect.anchoredPosition = Vector2.zero;
        // Flipear Directora para que mire a BitBot
        directoraObj.transform.localScale = new Vector3(-1, 1, 1);

        // Caja de Diálogo
        GameObject dialogBox = new GameObject("DialogBox");
        dialogBox.transform.SetParent(pausePanelObj.transform, false);
        Image dbImg = dialogBox.AddComponent<Image>();
        dbImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/HUD/HUD_Mensaje.png");
        dbImg.type = Image.Type.Simple; 
        dbImg.color = Color.white;
        dbImg.SetNativeSize();
        RectTransform dbRect = dialogBox.GetComponent<RectTransform>();
        dbRect.anchorMin = new Vector2(0.5f, 1f); dbRect.anchorMax = new Vector2(0.5f, 1f);
        dbRect.pivot = new Vector2(0.5f, 1f); dbRect.anchoredPosition = new Vector2(0, -30);
        dialogBox.transform.localScale = new Vector3(1.3f, 1.3f, 1f);

        // Texto del diálogo
        GameObject textObj = new GameObject("DialogText");
        textObj.transform.SetParent(dialogBox.transform, false);
        Text txt = textObj.AddComponent<Text>();
        txt.text = "¡La infección no se detiene! Analiza bien los paquetes de datos y no dejes que los archivos caigan. ¡Concéntrate!";
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 24; 
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.horizontalOverflow = HorizontalWrapMode.Wrap;
        txt.verticalOverflow = VerticalWrapMode.Truncate;
        RectTransform txtRect = textObj.GetComponent<RectTransform>();
        txtRect.anchorMin = new Vector2(0f, 0f); txtRect.anchorMax = new Vector2(1f, 1f);
        txtRect.offsetMin = new Vector2(80, 40); txtRect.offsetMax = new Vector2(-80, -90);

        // Botón Reanudar
        GameObject resumeBtnObj = new GameObject("ResumeButton");
        resumeBtnObj.transform.SetParent(pausePanelObj.transform, false);
        Image resImg = resumeBtnObj.AddComponent<Image>();
        resImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/Buttons/button_resume.png");
        resImg.SetNativeSize();
        resumeBtnObj.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
        Button resBtn = resumeBtnObj.AddComponent<Button>();
        RectTransform resRect = resumeBtnObj.GetComponent<RectTransform>();
        resRect.anchorMin = new Vector2(0.5f, 0.38f); resRect.anchorMax = new Vector2(0.5f, 0.38f);
        resRect.pivot = new Vector2(0.5f, 0.5f); resRect.anchoredPosition = Vector2.zero;

        // Botón Salir
        GameObject exitBtnObj = new GameObject("ExitButton");
        exitBtnObj.transform.SetParent(pausePanelObj.transform, false);
        Image exitImg = exitBtnObj.AddComponent<Image>();
        exitImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/UI/Buttons/Button_Exit.png");
        exitImg.SetNativeSize();
        exitBtnObj.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
        Button exitBtn = exitBtnObj.AddComponent<Button>();
        RectTransform exitRect = exitBtnObj.GetComponent<RectTransform>();
        exitRect.anchorMin = new Vector2(0.5f, 0.12f); exitRect.anchorMax = new Vector2(0.5f, 0.12f);
        exitRect.pivot = new Vector2(0.5f, 0.5f); exitRect.anchoredPosition = Vector2.zero;

        controller.pausePanel = pausePanelObj;
        controller.pauseButton = btnComp;
        controller.resumeButton = resBtn;
        controller.exitButton = exitBtn;
        pausePanelObj.SetActive(false);

        // --- INTRO CANVAS ---
        GameObject introObj = new GameObject("IntroCanvas");
        Canvas introCanvas = introObj.AddComponent<Canvas>();
        introCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        introCanvas.sortingOrder = 31000; 

        CanvasScaler introScaler = introObj.AddComponent<CanvasScaler>();
        introScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        introScaler.referenceResolution = new Vector2(1920, 1080);
        
        introObj.AddComponent<GraphicRaycaster>();

        GameObject introPanelObj = new GameObject("IntroPanel");
        introPanelObj.transform.SetParent(introObj.transform, false);
        RectTransform introPanelRect = introPanelObj.AddComponent<RectTransform>();
        introPanelRect.anchorMin = Vector2.zero; introPanelRect.anchorMax = Vector2.one;
        introPanelRect.sizeDelta = Vector2.zero;

        Image introSolidBg = introPanelObj.AddComponent<Image>();
        introSolidBg.color = new Color(0, 0, 0, 0.4f); // Más suave para que luzca el fondo

        GameObject introBgObj = new GameObject("Background");
        introBgObj.transform.SetParent(introPanelObj.transform, false);
        Image introPanelImg = introBgObj.AddComponent<Image>();
        introPanelImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Backgrounds/level 2/level2.png");
        introPanelImg.type = Image.Type.Simple;
        introPanelImg.preserveAspect = false; 
        RectTransform introBgRect = introBgObj.GetComponent<RectTransform>();
        introBgRect.anchorMin = Vector2.zero; introBgRect.anchorMax = Vector2.one; introBgRect.sizeDelta = Vector2.zero;

        // Glitch-Lord (Arriba al Centro, emergiendo e invadiendo)
        GameObject glitchObj = new GameObject("GlitchLord");
        glitchObj.transform.SetParent(introPanelObj.transform, false);
        Image glitchImg = glitchObj.AddComponent<Image>();
        glitchImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Glitch-Lord/glith-lord_riendose.png");
        glitchImg.preserveAspect = true;
        RectTransform glitchRect = glitchObj.GetComponent<RectTransform>();
        glitchRect.anchorMin = new Vector2(0.35f, 0.65f); glitchRect.anchorMax = new Vector2(0.65f, 0.95f);
        glitchRect.sizeDelta = Vector2.zero; glitchRect.anchoredPosition = Vector2.zero;

        // Virus (Enjambre de 8 descendiendo en cascada desde Glitch-Lord)
        Vector2[] virusAnchors = new Vector2[] {
            new Vector2(0.35f, 0.85f),
            new Vector2(0.6f, 0.85f),
            new Vector2(0.28f, 0.72f),
            new Vector2(0.67f, 0.72f),
            new Vector2(0.2f, 0.58f),
            new Vector2(0.75f, 0.58f),
            new Vector2(0.32f, 0.5f),
            new Vector2(0.62f, 0.5f)
        };
        for(int i=0; i<8; i++)
        {
            GameObject vObj = new GameObject("Virus_" + i);
            vObj.transform.SetParent(introPanelObj.transform, false);
            Image vImg = vObj.AddComponent<Image>();
            vImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Enemies/Virus/Idle_Virus.png");
            vImg.preserveAspect = true;
            RectTransform vRect = vObj.GetComponent<RectTransform>();
            vRect.anchorMin = virusAnchors[i];
            vRect.anchorMax = new Vector2(virusAnchors[i].x + 0.05f, virusAnchors[i].y + 0.08f);
            vRect.sizeDelta = Vector2.zero; vRect.anchoredPosition = Vector2.zero;
        }

        // Directora (Abajo a la Izquierda, más grande y proporcionada)
        GameObject introDirObj = new GameObject("Directora");
        introDirObj.transform.SetParent(introPanelObj.transform, false);
        Image introDirImg = introDirObj.AddComponent<Image>();
        introDirImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Directora Ctrl/directora_hablando.png");
        introDirImg.preserveAspect = true;
        RectTransform introDirRect = introDirObj.GetComponent<RectTransform>();
        introDirRect.anchorMin = new Vector2(0.08f, 0.08f); introDirRect.anchorMax = new Vector2(0.3f, 0.48f);
        introDirRect.sizeDelta = Vector2.zero; introDirRect.anchoredPosition = Vector2.zero;
        introDirObj.transform.localScale = new Vector3(-1, 1, 1); // Mirando hacia Bitbot

        // BitBot (Abajo a la Derecha, más grande y proporcionado)
        GameObject introBbObj = new GameObject("Bitbot");
        introBbObj.transform.SetParent(introPanelObj.transform, false);
        Image introBbImg = introBbObj.AddComponent<Image>();
        introBbImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Characters/Bit-Bot/bit-bot_fronta.png");
        introBbImg.preserveAspect = true;
        RectTransform introBbRect = introBbObj.GetComponent<RectTransform>();
        introBbRect.anchorMin = new Vector2(0.7f, 0.08f); introBbRect.anchorMax = new Vector2(0.92f, 0.48f);
        introBbRect.sizeDelta = Vector2.zero; introBbRect.anchoredPosition = Vector2.zero;
        introBbObj.transform.localScale = new Vector3(-1, 1, 1); // Mirando hacia la Directora

        // Dialogo Intro (Elevado un poco y ligeramente reducido para dar espacio)
        GameObject introDialogBox = new GameObject("DialogBox");
        introDialogBox.transform.SetParent(introPanelObj.transform, false);
        Image idbImg = introDialogBox.AddComponent<Image>();
        idbImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/UI/HUD/HUD_Mensaje.png");
        idbImg.type = Image.Type.Simple; 
        idbImg.color = Color.white;
        idbImg.SetNativeSize();
        RectTransform idbRect = introDialogBox.GetComponent<RectTransform>();
        idbRect.anchorMin = new Vector2(0.5f, 0.52f); idbRect.anchorMax = new Vector2(0.5f, 0.52f);
        idbRect.pivot = new Vector2(0.5f, 0.5f); idbRect.anchoredPosition = Vector2.zero;
        introDialogBox.transform.localScale = new Vector3(0.9f, 0.9f, 1f); 

        GameObject introTextObj = new GameObject("DialogText");
        introTextObj.transform.SetParent(introDialogBox.transform, false);
        Text itxt = introTextObj.AddComponent<Text>();
        itxt.text = "¡Bitbot, la red de datos está siendo vulnerada! Atrapa los archivos válidos antes de que caigan al vacío y esquiva los virus.";
        itxt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        itxt.fontSize = 24; 
        itxt.color = Color.white;
        itxt.alignment = TextAnchor.MiddleCenter;
        itxt.horizontalOverflow = HorizontalWrapMode.Wrap;
        itxt.verticalOverflow = VerticalWrapMode.Truncate;
        RectTransform itxtRect = introTextObj.GetComponent<RectTransform>();
        itxtRect.anchorMin = new Vector2(0f, 0f); itxtRect.anchorMax = new Vector2(1f, 1f);
        itxtRect.offsetMin = new Vector2(80, 40); itxtRect.offsetMax = new Vector2(-80, -90);

        // Boton Continuar (Centrado Abajo)
        GameObject contBtnObj = new GameObject("ContinueButton");
        contBtnObj.transform.SetParent(introPanelObj.transform, false);
        Image contImg = contBtnObj.AddComponent<Image>();
        contImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Resources/UI/Buttons/Button_Continue.png");
        contImg.SetNativeSize();
        contBtnObj.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        Button contBtn = contBtnObj.AddComponent<Button>();
        RectTransform contRect = contBtnObj.GetComponent<RectTransform>();
        contRect.anchorMin = new Vector2(0.5f, 0.1f); contRect.anchorMax = new Vector2(0.5f, 0.1f);
        contRect.pivot = new Vector2(0.5f, 0.5f); contRect.anchoredPosition = Vector2.zero;
        
        // Asignar el controlador
        IntroController introCtl = introObj.AddComponent<IntroController>();
        introCtl.continueButton = contBtn;
        introCtl.introPanel = introPanelObj;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[Setup] Interfaz de Intro y Pausa Nivel 2 CREADA EXITOSAMENTE.");
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
}
