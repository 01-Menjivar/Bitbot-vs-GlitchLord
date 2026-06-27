#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

/// <summary>
/// ScreenSetupHelper — Script del Editor para generar de forma estática la UI 
/// de GameOverScreen y VictoryScreen de modo que se puedan ver y editar visualmente
/// en el Scene view/Inspector de Unity.
/// </summary>
public class ScreenSetupHelper : EditorWindow
{
    [MenuItem("Tools/Setup Victory and GameOver Screens")]
    public static void SetupScreens()
    {
        SetupGameOverScene();
        SetupVictoryScene();
        Debug.Log("[ScreenSetupHelper] Pantallas de GameOver y Victoria configuradas estáticamente con éxito.");
    }

    private static void SetupGameOverScene()
    {
        // 1. Abrir la escena
        Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/GameOverScreen.unity", OpenSceneMode.Single);
        
        // 2. Buscar o crear Canvas
        Canvas canvas = FindOrCreateCanvas();
        
        // Limpiar elementos antiguos de UI
        ClearCanvas(canvas);

        // 3. Crear Fondo (BSOD)
        CreateBackground(canvas, "Assets/Resources/UI/Screens/GameOverScreen.png");

        // 4. Crear Botón Reintentar
        GameObject btnRetry = CreateButton(canvas, "BtnRetry", "Assets/Resources/UI/Buttons/Button_Retry.png", new Vector2(-300, -320));
        
        // 5. Crear Botón Menú
        GameObject btnExit = CreateButton(canvas, "BtnExit", "Assets/Resources/UI/Buttons/Button_Exit.png", new Vector2(300, -320));

        // 6. Guardar escena
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static void SetupVictoryScene()
    {
        // 1. Abrir la escena
        Scene scene = EditorSceneManager.OpenScene("Assets/Scenes/VictoryScreen.unity", OpenSceneMode.Single);
        
        // 2. Buscar o crear Canvas
        Canvas canvas = FindOrCreateCanvas();
        
        // Limpiar elementos antiguos de UI
        ClearCanvas(canvas);

        // 3. Crear Fondo de Victoria
        CreateBackground(canvas, "Assets/Resources/UI/Screens/VictoryScreen.png");

        // 4. Crear Botón Menú (Centrado)
        GameObject btnExit = CreateButton(canvas, "BtnExit", "Assets/Resources/UI/Buttons/Button_Exit.png", new Vector2(0, -320));

        // 5. Guardar escena
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
    }

    private static Canvas FindOrCreateCanvas()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        return canvas;
    }

    private static void ClearCanvas(Canvas canvas)
    {
        // Destruir hijos para iniciar limpio
        for (int i = canvas.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(canvas.transform.GetChild(i).gameObject);
        }
    }

    private static void CreateBackground(Canvas canvas, string path)
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvas.transform, false);
        
        Image img = bgObj.AddComponent<Image>();
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }
        
        RectTransform rect = bgObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
    }

    private static GameObject CreateButton(Canvas canvas, string name, string spritePath, Vector2 position)
    {
        GameObject btnObj = new GameObject(name, typeof(RectTransform));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(510f, 165f); // Tamaño retro estándar

        Image img = btnObj.AddComponent<Image>();
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite != null)
        {
            img.sprite = sprite;
            img.color = Color.white;
        }

        Button btn = btnObj.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        ColorBlock colors = btn.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        colors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        colors.selectedColor = Color.white;
        btn.colors = colors;

        return btnObj;
    }
}
#endif
