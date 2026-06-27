using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupLevel2HUD_Final
{
    [MenuItem("Tools/Setup Level 2 HUD Final")]
    public static void RunSetup()
    {
        // Si el editor está en Play Mode, desactivarlo y salir para evitar errores de carga
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            Debug.LogWarning("Se desactivó el Play Mode para realizar la configuración. Ejecute el comando de menú nuevamente.");
            return;
        }

        // 1. Cargar la escena Level2
        string scenePath = "Assets/Scenes/Level2.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        // 2. Buscar el Canvas y el HUD_Timer
        GameObject hudTimerObj = GameObject.Find("HUD_Timer");
        if (hudTimerObj != null)
        {
            RectTransform rect = hudTimerObj.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = new Vector3(0.55f, 0.55f, 1f);
                Debug.Log("Scaled HUD_Timer to 0.55 in Level2.unity");
            }
        }
        else
        {
            Debug.LogError("No se encontró HUD_Timer en la escena Level2.unity.");
        }

        // 3. Buscar _Managers
        GameObject managersObj = GameObject.Find("_Managers");
        if (managersObj != null)
        {
            // Buscar o crear TimerController
            TimerController timerCtrl = managersObj.GetComponentInChildren<TimerController>(true);
            if (timerCtrl == null)
            {
                GameObject timerObj = new GameObject("TimerController");
                timerObj.transform.SetParent(managersObj.transform);
                timerObj.AddComponent<TimerController>();
                Debug.Log("TimerController agregado a _Managers en Level2.unity");
            }
            else
            {
                Debug.Log("TimerController ya existe en _Managers en Level2.unity");
            }

            // Buscar o crear LifeManager
            LifeManager lifeMgr = managersObj.GetComponentInChildren<LifeManager>(true);
            if (lifeMgr == null)
            {
                GameObject lifeObj = new GameObject("LifeManager");
                lifeObj.transform.SetParent(managersObj.transform);
                lifeObj.AddComponent<LifeManager>();
                Debug.Log("LifeManager agregado a _Managers en Level2.unity");
            }
            else
            {
                Debug.Log("LifeManager ya existe en _Managers en Level2.unity");
            }
        }
        else
        {
            Debug.LogError("No se encontró _Managers en la escena Level2.unity.");
        }

        // 4. Guardar la escena
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Escena Level2 configurada y guardada con éxito.");
    }
}
