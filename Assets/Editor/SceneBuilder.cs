using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneBuilder : EditorWindow
{
    [MenuItem("Tools/Create Base Scene")]
    public static void CreateBaseScene()
    {
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
        
        // Agregar el componente AudioListener a la Cámara
        cameraObj.AddComponent<AudioListener>();
        
        // 3. Crear el contenedor de Managers
        GameObject managersObj = new GameObject("_Managers");
        
        GameObject gameManagerObj = new GameObject("GameManager");
        gameManagerObj.transform.parent = managersObj.transform;
        gameManagerObj.AddComponent<GameManager>();
        
        GameObject uiManagerObj = new GameObject("UIManager");
        uiManagerObj.transform.parent = managersObj.transform;
        uiManagerObj.AddComponent<UIManager>();
        
        GameObject audioManagerObj = new GameObject("AudioManager");
        audioManagerObj.transform.parent = managersObj.transform;
        audioManagerObj.AddComponent<AudioManager>();
        
        // 4. Crear el Canvas de Interfaz y el EventSystem
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemObj.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        
        // 5. Guardar la Escena en la carpeta Assets/Scenes/
        string scenePath = "Assets/Scenes/BaseMinigameScene.unity";
        bool saveSuccess = EditorSceneManager.SaveScene(newScene, scenePath);
        
        if (saveSuccess)
        {
            Debug.Log("Escena creada y guardada con éxito en: " + scenePath);
            EditorUtility.DisplayDialog("Éxito", "La escena base en 2D fue creada y guardada con éxito en Assets/Scenes/BaseMinigameScene.unity", "Aceptar");
        }
        else
        {
            Debug.LogError("Error al intentar guardar la escena base.");
        }
    }
}
