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
            resumeBtnObj.transform.localScale = new Vector3(0.75f, 0.75f, 1f); // 25% mas grande que antes (era 0.6)
        }
        Button resBtn = resumeBtnObj.AddComponent<Button>();
        RectTransform resRect = resumeBtnObj.GetComponent<RectTransform>();
        // Subir considerablemente el boton para que no se mezcle con la mesa (de 0.15 a 0.25)
        resRect.anchorMin = new Vector2(0.5f, 0.25f);
        resRect.anchorMax = new Vector2(0.5f, 0.25f);
        resRect.pivot = new Vector2(0.5f, 0.5f);

        // Asignar referencias al controlador
        controller.pausePanel = pausePanelObj;
        controller.pauseButton = btnComp;
        controller.resumeButton = resBtn;

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
}
